using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PieceDragHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private GraphicRaycaster graphicRaycaster;
    private Canvas canvas;



    public void OnDrag(PointerEventData eventData)
    {
        transform.right = SliceManager.instance.gameObject.transform.position - transform.position;

        transform.position = eventData.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameplayController.instance.originalPieceRotation = transform.localRotation;
        GameplayController.instance.originalParent = transform.parent;

        GameplayController.instance.draggingPiece = transform.GetComponent<Piece>();
        GameplayController.instance.originalPiecePos = transform.GetComponent<RectTransform>().anchoredPosition;


        GameplayController.instance.draggingPiece.rightChild.DisableRaycast();
        GameplayController.instance.draggingPiece.leftChild.DisableRaycast();

        canvas = GetComponentInParent<Canvas>();
        graphicRaycaster = canvas.GetComponent<GraphicRaycaster>(); /// this helps us detect what we landed on later

        transform.SetParent(GameplayController.instance.dragParent); // this is the gameplay canvas - we do this so the piece renders over all other 2D elements


        transform.right = SliceManager.instance.gameObject.transform.position - transform.position;

        transform.position = eventData.position;


        Debug.Log(transform.name);


    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameplayController.instance.draggingPiece.rightChild.EnableRaycast();
        GameplayController.instance.draggingPiece.leftChild.EnableRaycast();
       
        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(eventData, results);
        // Check all hits

        foreach (var hit in results)
        {
            // If we found a cell
            Cell cell = hit.gameObject.GetComponent<Cell>();

            if (cell)
            {
                return;
            }
        }

        GameplayController.instance.ReturnHome();

        Debug.Log("pointer up");
    }
}
