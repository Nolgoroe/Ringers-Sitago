using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class PieceDragHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private GraphicRaycaster graphicRaycaster;
    private Canvas canvas;
    private Piece relatedPiece;

    private void Start()
    {
        relatedPiece = GetComponent<Piece>();
    }



    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer down");

        if (PowerUpManager.IsUsingPowerUp)
        {
            return;
        }

        GameplayController.instance.originalPieceRotation = transform.localRotation;
        GameplayController.instance.originalParent = transform.parent;

        GameplayController.instance.draggingPiece = relatedPiece;
        GameplayController.instance.originalPiecePos = transform.GetComponent<RectTransform>().anchoredPosition;

        if (GameplayController.instance.CheckOriginalParentIsCell())
        {
            Cell myCell = GameplayController.instance.originalParent.GetComponent<Cell>();
            int myCellIndex = System.Array.IndexOf(SliceManager.instance.boardCells, myCell);

            ConnectionManager.instance.CheckConnectionsOnPickup(myCell, myCellIndex); // check connection here to see how many "bad connections" are left

            myCell.isFull = false;
            myCell.heldPiece = null;
        }

        relatedPiece.rightChild.DisableRaycast(); // so we don't check against ourselves what we hit
        relatedPiece.leftChild.DisableRaycast();

        canvas = GetComponentInParent<Canvas>();
        graphicRaycaster = canvas.GetComponent<GraphicRaycaster>(); /// this helps us detect what we landed on later

        transform.SetParent(GameplayController.instance.dragParent); // this is the gameplay canvas - we do this so the piece renders over all other 2D elements


        transform.right = SliceManager.instance.gameObject.transform.position - transform.position;

        transform.position = eventData.position;


        Debug.Log(transform.name);


    }

    public void OnDrag(PointerEventData eventData)
    {
        if (PowerUpManager.IsUsingPowerUp)
        {
            return;
        }

        transform.right = SliceManager.instance.gameObject.transform.position - transform.position;

        transform.position = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (PowerUpManager.IsUsingPowerUp)
        {
            PowerUpManager.instance.ObjectToUsePowerUpOn = relatedPiece;

            PowerUpManager.HasUsedPowerUp = true;

            Debug.Log("Used power");
            return;
        }

        relatedPiece.rightChild.EnableRaycast();
        relatedPiece.leftChild.EnableRaycast();
       
        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(eventData, results);
        // Check all hits

        foreach (var hit in results)
        {
            // If we found a cell
            Cell myCell = hit.gameObject.GetComponent<Cell>();

            if (myCell)
            {
                if (GameplayController.instance.CheckOriginalParentIsCell()) // if I hit the cell I was already on
                {
                    if (myCell == GameplayController.instance.originalParent.GetComponent<Cell>())
                    {
                        GameplayController.instance.ReturnHome();
                        return;
                    }
                }


                if (!myCell.isFull && GameplayController.instance.CheckOriginalParentIsClip()) // check if we came from clip. if we did we need to repopulate the clip
                {
                    GameManager.instance.totalPlacedPieces++; // to check for end of level

                    if (GameManager.instance.totalPlacedPieces == GameManager.instance.currentMap.cellsCountInLevel)
                    {
                        myCell.PopulateCellHeldPiece(relatedPiece);

                        LastPieceLogic(myCell);

                        return;
                    }

                    relatedPiece.partOfBoard = true;

                    ClipManager.instance.PopulateSlot(GameplayController.instance.originalParent.GetComponent<Clip>());

                }


                if (myCell.isFull) // check if going in a cell that is already full
                {
                    GameplayController.instance.ReturnHome();
                }
                else // fill cell with piece
                {

                    myCell.isFull = true;                    
                    relatedPiece.transform.SetParent(myCell.transform);
                    myCell.SnapFollowerToCell();
                    myCell.PopulateCellHeldPiece(relatedPiece);

                    // Send to check connection here

                    int myCellIndex = System.Array.IndexOf(SliceManager.instance.boardCells, myCell);

                    ConnectionManager.instance.CheckConnection(myCell, myCellIndex);
                }

                if (GameManager.instance.totalPlacedPieces == GameManager.instance.currentMap.cellsCountInLevel)
                {
                    GameManager.instance.CheckEndLevel();
                    return;
                }

                GameplayController.instance.ResetControlData();


                return;
            }
        }

        GameplayController.instance.ReturnHome();

        Debug.Log("pointer up");
    }

    private void LastPieceLogic(Cell myCell)
    {
        int myCellIndex = System.Array.IndexOf(SliceManager.instance.boardCells, myCell);

        ConnectionManager.instance.CheckConnection(myCell, myCellIndex);

        bool goodFinish = GameManager.instance.CheckEndLevel();

        if (goodFinish)
        {
            myCell.isFull = true;
            relatedPiece.transform.SetParent(myCell.transform);
            myCell.SnapFollowerToCell();

            GameManager.instance.currentMapIndex++;

            GameManager.instance.ResetDataStartLevelStartNormal();
        }
        else
        {
            GameManager.instance.totalPlacedPieces--;

            ConnectionManager.instance.CheckConnectionsOnPickup(myCell, myCellIndex); // check connection here to see how many "bad connections" are left

            myCell.ResetCellHeldPiece();
            GameplayController.instance.ReturnHome();

            UIManager.instance.RingCouldNotBeCompletedText();
        }
    }
}
