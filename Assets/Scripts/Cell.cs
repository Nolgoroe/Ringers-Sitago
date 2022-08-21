using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour
{
    public int cellIndex;
    public bool isFull;

    public Piece heldPiece;
    public Slice leftSlice, rightSlice;

    public Animator goodConnectLeft, goodConnectRight;
    public Animator badConnectLeft, badConnectRight;
    public void SnapFollowerToCell()
    {
        GameplayController.instance.draggingPiece.transform.localPosition = Vector3.zero;
        GameplayController.instance.draggingPiece.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0f));
    }

    public void PopulateCellHeldPiece(Piece p)
    {
        heldPiece = p;

        ConnectionManager.instance.subPiecesOnBoardTempAlgoritm[cellIndex * 2] = heldPiece.leftChild;
        ConnectionManager.instance.subPiecesOnBoardTempAlgoritm[cellIndex * 2 + 1] = heldPiece.rightChild;
    }
    public void ResetCellHeldPiece()
    {
        heldPiece = null;
    }
    

    public void ResetConnectionDisplays()
    {
        goodConnectLeft.gameObject.SetActive(false);
        goodConnectRight.gameObject.SetActive(false);
        badConnectLeft.gameObject.SetActive(false);
        badConnectRight.gameObject.SetActive(false);
    }
}
