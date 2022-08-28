using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClipManager : MonoBehaviour
{
    [Serializable]
    public class ColorsToSprites
    {
        public PieceColor matColor;
        public Sprite[] symbolSprites;
    }

    public static ClipManager instance;
    public Clip[] slots;
    public ColorsToSprites[] colorsToSprites;
    public Sprite jokerSprite;




    [Header("Prefabs")]
    public GameObject piecePrefab;
    public GameObject corruptedPiecePrefab;


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
    }


    public void PopulateSlot(Clip c)
    {
        GameObject go = Instantiate(piecePrefab, c.transform);
        Piece p = go.GetComponent<Piece>();
        p.SetPieces();

        c.pieceInside = p;

        p.GetComponent<Animator>().SetTrigger("Appear");
    }

    public bool ComparerPiece(Piece currectCheckPiece, Piece p)
    {
        if ((currectCheckPiece.rightChild.colorOfPiece == p.rightChild.colorOfPiece) && (currectCheckPiece.rightChild.symbolOfPiece == p.rightChild.symbolOfPiece))
        {
            if ((currectCheckPiece.leftChild.colorOfPiece == p.leftChild.colorOfPiece) && (currectCheckPiece.leftChild.symbolOfPiece == p.leftChild.symbolOfPiece))
            {
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

            p.GetComponent<Animator>().SetBool("IsJoker", false);
            p.SetPieceAsNormal();

            RerollSlotPieceData(p);
        }
    }

    public void RerollSlotPieceData(Piece p)
    {
        p.SetPieces();
    }

    public void RefreshSpecificSlot(int index, EdgePathFoundData dataNeeded)
    {
        Piece p = slots[index].GetComponentInChildren<Piece>();
        SetPieceSpecificData(p, dataNeeded);
    }
    public void SetPieceSpecificData(Piece p, EdgePathFoundData dataNeeded)
    {
        p.SetPiecesSpecificData(dataNeeded);
    }
}
