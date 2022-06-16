using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour
{    
    public bool isFull;

    public Piece heldPiece;
    public Slice[] connectedSlices;

    public void SnapFollowerToCell()
    {
        GameplayController.instance.draggingPiece.transform.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        GameplayController.instance.draggingPiece.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0f));
    }

    public void PopulateCellHeldPiece(Piece p)
    {
        heldPiece = p;
    }
    public void ResetCellHeldPiece()
    {
        heldPiece = null;
    }
}
