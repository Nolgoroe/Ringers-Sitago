using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum PieceColor
{
    Purple,
    Blue,
    Yellow,
    Green,
    Pink,
    Joker
}
[System.Serializable]
public enum PieceSymbol
{
    FireFly,
    Badger,
    Goat,
    Turtle,
    Joker
}

public class Piece : MonoBehaviour
{
    public SubPiece rightChild, leftChild;

    //public bool isLocked;
    //public bool isStone;
    //public bool isDuringConnectionAnim;
    //public bool partOfBoard;

    public void SetPieces()
    {
        bool isSamePiece = true;
        int repeatIndicator = 0;


        while (isSamePiece)
        {
            rightChild.SetPiece();
            leftChild.SetPiece();

            isSamePiece = CheckNoRepeatPieceClip();


            if (GameManager.instance.currentMap.levelAvailableColors.Length == 1 && GameManager.instance.currentMap.levelAvailableSymbols.Length == 1)
            {
                isSamePiece = false;
            }
            else
            {
                //// To make sure unity doesnt get stuck because of human error - we check to see if we need to default same piece to false.
                //// we default same piece to false when the color OR symbol arrays contain only 1 element.. since all pieces will be the same
                ////  we default same piece to false when the color OR symbol arrays contain more than 1 element, BUT ALL ELEMENTS ARE THE SAME!
                if (GameManager.instance.currentMap.levelAvailableColors.Length > 1)
                {
                    for (int i = 0; i < System.Enum.GetValues(typeof(PieceColor)).Length; i++)
                    {
                        int same = 0;
                        foreach (PieceColor PC in GameManager.instance.currentMap.levelAvailableColors)
                        {
                            if (PC == (PieceColor)i)
                            {
                                same++;
                            }

                            if (same > 1)
                            {
                                Debug.LogError("Found duplicates in the level Available colors array!");
                            }

                            if (same == GameManager.instance.currentMap.levelAvailableColors.Length)
                            {
                                isSamePiece = false;
                                break;
                            }
                        }
                    }
                }

                //// To make sure unity doesnt get stuck because of human error - we check to see if we need to default same piece to false.
                //// we default same piece to false when the color AND symbol arrays contain only 1 element.. since all pieces will be the same
                ////  we default same piece to false when the color OR symbol arrays contain more than 1 element, BUT ALL ELEMENTS ARE THE SAME!
                if (GameManager.instance.currentMap.levelAvailableSymbols.Length > 1)
                {
                    for (int i = 0; i < System.Enum.GetValues(typeof(PieceSymbol)).Length; i++)
                    {
                        int same = 0;
                        foreach (PieceSymbol PS in GameManager.instance.currentMap.levelAvailableSymbols)
                        {
                            if (PS == (PieceSymbol)i)
                            {
                                same++;
                            }

                            if (same > 1)
                            {
                                Debug.LogError("Found duplicates in the level Available symbols array!");
                            }

                            if (same == GameManager.instance.currentMap.levelAvailableSymbols.Length)
                            {
                                isSamePiece = false;
                                break;
                            }
                        }
                    }
                }
            }

            if (isSamePiece)
            {
                repeatIndicator++;
            }
        }
    }

    public bool CheckNoRepeatPieceClip()
    {
        Piece currectCheckPiece = GetComponent<Piece>();

        for (int i = 0; i < ClipManager.instance.slots.Length; i++)
        {
            Piece p = ClipManager.instance.slots[i].pieceInside;

            if (p)
            {
                if (p != currectCheckPiece)
                {
                    bool isSame = ClipManager.instance.ComparerPiece(currectCheckPiece, p);

                    if (isSame)
                    {
                        return true; //// found a repeat so can't continue, is same piece = true
                    }
                }

            }

        }

        return false; //// There was no repeat, is same piece = false
    }


    //public void SetStonePiece(stonePieceDataStruct SPDS)
    //{
    //    bool isRepeatPieceSides = true;
    //    bool isRepeatPieceOnBoard = true;
    //    int repeatIndicator = 0;

    //    rightChild.SetStonePiece(SPDS);
    //    leftChild.SetStonePiece(SPDS);

    //    while (isRepeatPieceSides || isRepeatPieceOnBoard)
    //    {
    //        if (repeatIndicator > 0)
    //        {
    //            rightChild.SetStonePiece(SPDS);
    //            leftChild.SetStonePiece(SPDS);

    //            repeatIndicator = 0;
    //        }

    //        isRepeatPieceSides = CheckNoRepeatPieceSides();
    //        isRepeatPieceOnBoard = ConnectionManager.Instance.CheckRepeatingStonePieces(this);

    //        //Debug.LogError("Same Pieces? " + isRepeatPieceSides);
    //        Debug.LogError("Same Pieces on board? " + isRepeatPieceOnBoard);

    //        if (GameManager.Instance.currentLevel.levelAvailablesymbols.Length > 0)
    //        {
    //            if (GameManager.Instance.currentLevel.levelAvailablesymbols.Length == 1)
    //            {
    //                isRepeatPieceSides = false;
    //            }
    //            else
    //            {
    //                // This loop checkes to see if someone accidentaly filled the "levelAvailablesymbols" list
    //                // with multiples of the same symbol - like 3 times goat.
    //                for (int i = 0; i < System.Enum.GetValues(typeof(PieceSymbol)).Length; i++)
    //                {
    //                    int same = 0;
    //                    foreach (PieceSymbol PS in GameManager.Instance.currentLevel.levelAvailablesymbols)
    //                    {
    //                        if (PS == (PieceSymbol)i)
    //                        {
    //                            same++;
    //                        }

    //                        if (same > 1)
    //                        {
    //                            Debug.LogError("Found duplicates in the level Available symbols array!");
    //                        }


    //                        if (same == GameManager.Instance.currentLevel.levelAvailablesymbols.Length)
    //                        {
    //                            isRepeatPieceSides = false;
    //                            break;
    //                        }
    //                    }

    //                    //if (!isSamePiece)
    //                    //{
    //                    //    break;
    //                    //}
    //                }
    //            }
    //        }

    //        if (isRepeatPieceSides || isRepeatPieceOnBoard)
    //        {
    //            repeatIndicator++;
    //        }
    //    }

    //    //rightChild.SetStonePiece(SPDS, true);
    //    //leftChild.SetStonePiece(SPDS, false);
    //}

    //public bool CheckNoRepeatPieceSides() // for now it's only for stone pieces
    //{
    //    Piece currectCheckPiece = GetComponent<Piece>();

    //    if (currectCheckPiece.rightChild.symbolOfPiece == currectCheckPiece.leftChild.symbolOfPiece)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }

    //}


}
