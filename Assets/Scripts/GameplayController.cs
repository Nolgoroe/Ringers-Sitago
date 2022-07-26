using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class GameplayController: MonoBehaviour
{
    public static GameplayController instance;
    public static bool canMovePieces;

    [Header("Required Elements")]
    public GameObject gameBoard;
    public GameObject gameClip;
    public Transform dragParent;

    [Header("Control Data")]
    public Piece draggingPiece;
    public Transform originalParent;
    public Quaternion originalPieceRotation;
    public Vector3 originalPiecePos;
    public float planeDistanceCamera;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        planeDistanceCamera = UIManager.instance.gameplayCanvas.GetComponent<Canvas>().planeDistance;
    }
    public void InitLeve()
    {
        gameBoard = SliceManager.instance.gameObject;
        gameClip = ClipManager.instance.gameObject;
        canMovePieces = true;
    }

    public void ReturnHome()
    {
        SoundManager.instance.FindSoundToPlay(AllGameSoundsEnums.TilePlacement);

        draggingPiece.transform.SetParent(originalParent);

        if (CheckOriginalParentIsCell())
        {
            Cell myCell = originalParent.GetComponent<Cell>();

            myCell.isFull = true;
            myCell.heldPiece = draggingPiece;

            int myCellIndex = System.Array.IndexOf(SliceManager.instance.boardCells, myCell); // we need to check connection again here if we just return to the same cell

            ConnectionManager.instance.CheckConnection(myCell, myCellIndex);
        }

        draggingPiece.GetComponent<RectTransform>().anchoredPosition = originalPiecePos;
        draggingPiece.transform.localRotation = originalPieceRotation; ///// reset piece rotation to it's original local rotation

        ResetControlData();
    }

    public void ResetControlData()
    {
        draggingPiece = null;
        originalParent = null;
        originalPieceRotation = Quaternion.Euler(Vector3.zero);
        originalPiecePos = Vector3.zero;
    }

    public bool CheckOriginalParentIsClip()
    {
        if (originalParent.GetComponent<Clip>())
        {
            return true;
        }

        return false;
    }
    public bool CheckOriginalParentIsCell()
    {
        if (originalParent.GetComponent<Cell>())
        {
            return true;
        }

        return false;
    }
}
