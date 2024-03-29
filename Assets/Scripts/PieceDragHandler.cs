﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Rendering;

public class PieceDragHandler : MonoBehaviour
{
    //private GraphicRaycaster graphicRaycaster;
    //private Canvas canvas;
    private Piece relatedPiece;

    public Vector3 dragRotationOffset;
    RectTransform rect;

    public LayerMask cellLayer;

    public SortingGroup sortingGroup;

    private void Start()
    {
        relatedPiece = GetComponent<Piece>();
        rect = transform.GetComponent<RectTransform>();
        sortingGroup = GetComponent<SortingGroup>();
    }

    private void OnMouseDown()
    {
        if (PowerUpManager.IsUsingPowerUp)
        {
            return;
        }

        GameplayController.instance.originalPieceRotation = transform.localRotation;
        GameplayController.instance.originalParent = transform.parent;

        GameplayController.instance.draggingPiece = relatedPiece;
        GameplayController.instance.originalPiecePos = transform.position;

        sortingGroup.sortingOrder = 20;

        if (GameplayController.instance.CheckOriginalParentIsCell())
        {
            Cell myCell = GameplayController.instance.originalParent.GetComponent<Cell>();
            int myCellIndex = System.Array.IndexOf(SliceManager.instance.boardCells, myCell);

            ConnectionManager.instance.CheckConnectionsOnPickup(myCell, myCellIndex); // check connection here to see how many "bad connections" are left

            myCell.isFull = false;
            myCell.heldPiece = null;
        }

        transform.SetParent(GameplayController.instance.dragParent); // this is the gameplay canvas - we do this so the piece renders over all other 2D elements


        transform.up = -(SliceManager.instance.gameObject.transform.position - transform.position);

        Vector3 screenPoint = Input.mousePosition;
        screenPoint.z = -GameplayController.instance.planeDistanceCamera;
        transform.position = Camera.main.ScreenToWorldPoint(screenPoint);

        GetComponent<Animator>().ResetTrigger("Put Down");

        GetComponent<Animator>().SetTrigger("Pick Up");
    }

    public void OnMouseDrag()
    {
        if (PowerUpManager.IsUsingPowerUp)
        {
            return;
        }
        sortingGroup.sortingOrder = 20;

        transform.up = -(SliceManager.instance.gameObject.transform.position - transform.position);

        Vector3 screenPoint = Input.mousePosition;
        screenPoint.z = -GameplayController.instance.planeDistanceCamera;
        transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
    }

    public void OnMouseUp()
    {
        GetComponent<Animator>().ResetTrigger("Pick Up");

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit2D hit2D = Physics2D.GetRayIntersection(ray, Mathf.Infinity, cellLayer);

        if (hit2D && hit2D.transform.GetComponent<Cell>())
        {
            Cell myCell = null;

            myCell = hit2D.transform.GetComponent<Cell>();

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


                //this is to check if we're putting the last piece
                if (!myCell.isFull && GameplayController.instance.CheckOriginalParentIsClip()) // check if we came from clip. if we did we need to repopulate the clip
                {
                    GameManager.instance.totalPlacedPieces++; // to check for end of level

                    if (GameManager.instance.totalPlacedPieces == GameManager.instance.currentMap.cellsCountInLevel)
                    {
                        GetComponent<Animator>().SetTrigger("Put Down");

                        myCell.PopulateCellHeldPiece(relatedPiece);

                        LastPieceLogic(myCell);

                        return;
                    }

                    relatedPiece.partOfBoard = true;

                    ClipManager.instance.PopulateSlot(GameplayController.instance.originalParent.GetComponent<Clip>());

                }


                // normal place check
                if (myCell.isFull) // check if going in a cell that is already full
                {
                    GameplayController.instance.ReturnHome();
                }
                else // fill cell with piece
                {
                    SoundManager.instance.PlaySound(SoundType.TilePlace);

                    GetComponent<Animator>().SetTrigger("Put Down");

                    myCell.isFull = true;
                    relatedPiece.transform.SetParent(myCell.transform);
                    myCell.SnapFollowerToCell();
                    myCell.PopulateCellHeldPiece(relatedPiece);

                    // Send to check connection here

                    int myCellIndex = System.Array.IndexOf(SliceManager.instance.boardCells, myCell);

                    ConnectionManager.instance.CheckConnection(myCell, myCellIndex);
                }

                GameplayController.instance.ResetControlData();


                return;
            }
        }

        GameplayController.instance.ReturnHome();

    }

    private void LastPieceLogic(Cell myCell)
    {
        int myCellIndex = System.Array.IndexOf(SliceManager.instance.boardCells, myCell);

        ConnectionManager.instance.CheckConnection(myCell, myCellIndex);

        bool goodFinish = GameManager.instance.CheckEndLevel();

        if (goodFinish)
        {
            ScoreManager.instance.AddRingCompletionScore();

            ScoreManager.instance.hasClickedDeal = false;
            PowerUpManager.instance.timesClickedDeal = 0;

            myCell.isFull = true;
            relatedPiece.transform.SetParent(myCell.transform);
            myCell.SnapFollowerToCell();

            GameManager.instance.currentMapIndex++;

            SliceManager.instance.GetComponent<Animator>().SetTrigger("Ring Complete");
        }
        else
        {
            GameManager.instance.totalPlacedPieces--;

            ConnectionManager.instance.CheckConnectionsOnPickup(myCell, myCellIndex); // check connection here to see how many "bad connections" are left

            myCell.ResetCellHeldPiece();
            GameplayController.instance.ReturnHome();

           StartCoroutine(UIManager.instance.HeaderFadeInText("The ring cannot be completed!"));

            SliceManager.instance.GetComponent<Animator>().SetTrigger("Incorrect Complete");
        }
    }
}
