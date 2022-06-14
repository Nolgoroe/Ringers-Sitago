using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClipManager : MonoBehaviour
{
    public static ClipManager instance;
    public Clip[] slots;




    [Header("Prefabs")]
    public GameObject piecePrefab;
    public GameObject corruptedPiecePrefab;

    //public Transform emptyClip;
    //public Transform latestPiece;

    //public Vector3 originalPiecePos;
    //public Quaternion originalPieceRot;

    ////public Material[] gameColors;
    ////public Texture2D[] gameSymbols;

    //public ColorsAndMats[] colorsToMats;
    //public Texture[] corruptedColorsToMatsLeft;
    //public Texture[] corruptedColorsToMatsRight;
    //public SymbolToMat[] symbolToMat;

    ////public ColorsAndMats[] corruptedColorsToMats;
    ////public SymbolToMat[] corruptedSymbolToMat;

    //public int clipCount;

    //public Material generalPieceMat;

    //public Color darkTintedColor;


    //public Vector3 pieceScaleOnBoard;

    //public float delaySpecialPowerFirefly;
    //[Serializable]
    //public class ColorsAndMats
    //{
    //    public PieceColor matColor;
    //    //public Material[] colorMats;
    //    public Texture[] colorTex;
    //}

    //[Serializable]
    //public class SymbolToMat
    //{
    //    public PieceSymbol mat;
    //    //public Material symbolMat;
    //    public Texture symbolTex;
    //}

    //private void Awake()
    //{
    //    //GameManager.Instance.clipManager = this;
    //}

    private void Awake()
    {
        instance = this;
    }

    public void InitLevel()
    {
        foreach (Clip s in slots)
        {
            PopulateSlot(s);
        }

        //originalPiecePos = piece.transform.position;
        //originalPieceRot = piece.transform.rotation;
    }


    public void PopulateSlot(Clip c)
    {
        GameObject go = Instantiate(piecePrefab, c.transform);
        Piece p = go.GetComponent<Piece>();
        p.SetPieces();

        c.pieceInside = p;
    }

    public bool ComparerPiece(Piece currectCheckPiece, Piece p)
    {
        if ((currectCheckPiece.rightChild.colorOfPiece == p.rightChild.colorOfPiece) && (currectCheckPiece.rightChild.symbolOfPiece == p.rightChild.symbolOfPiece))
        {
            if ((currectCheckPiece.leftChild.colorOfPiece == p.leftChild.colorOfPiece) && (currectCheckPiece.leftChild.symbolOfPiece == p.leftChild.symbolOfPiece))
            {
                Debug.Log("Pieces were the same!" + currectCheckPiece + " " + p);
                return true; ///// Pieces are the same
            }
        }

        return false; //// Pieces are not the same
    }


    public void DealAnimClipLogic()
    {
        RefreshSlots();
    }

    public void RefreshSlots()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            Piece p = slots[i].GetComponentInChildren<Piece>();
            RerollSlotPieceData(p);
        }
    }
    public void RerollSlotPieceData(Piece p)
    {
        p.SetPieces();
    }

    //public void RepopulateLatestClip() //// In case player clicked the hell no option after placing last piece
    //{
    //    if (latestPiece.GetComponent<Piece>())
    //    {
    //        Piece p = latestPiece.GetComponent<Piece>();

    //        if (p.rightChild.relevantSlice)
    //        {
    //            Slice relevantSlice = p.rightChild.relevantSlice;

    //            if (relevantSlice.isLock)
    //            {
    //                relevantSlice.lockSpriteHeighlightAnim.gameObject.SetActive(false);
    //            }
    //        }

    //        if (p.leftChild.relevantSlice)
    //        {
    //            Slice relevantSlice = p.leftChild.relevantSlice;

    //            if (relevantSlice.isLock)
    //            {
    //                relevantSlice.lockSpriteHeighlightAnim.gameObject.SetActive(false);
    //            }
    //        }

    //        if (p.transform.parent.GetComponent<Cell>())
    //        {
    //            Cell c = p.transform.parent.GetComponent<Cell>();

    //            c.RemovePiece(false);
    //        }
    //        else
    //        {
    //            Debug.LogError("No supposed to be like this - this is supposed to be a cell!");
    //        }

    //        p.transform.SetParent(emptyClip);
    //        p.transform.localPosition = originalPiecePos;
    //        p.transform.localRotation = originalPieceRot;

    //        p.partOfBoard = false;
    //        p.isLocked = false;
    //        p.isTutorialLocked = false;

    //        GameManager.Instance.currentFilledCellCount--;

    //        if (GameManager.Instance.currentLevel.is12PieceRing)
    //        {
    //            p.transform.localScale = new Vector3(1.85f, 1.85f, 1);
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError("No supposed to be like this - this is supposed to be a piece!");
    //    }

    //}
}
