using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    public static ConnectionManager instance;
    private void Awake()
    {
        instance = this;
    }

    public void CheckConnectionsOnPickup(Cell myCell, int myCellIndex)
    {
        Cell leftCell = GetCell(myCellIndex - 1);
        Cell rightCell = GetCell(myCellIndex + 1);


        Debug.Log("PICKUP My cell index " + myCell);
        Debug.Log("PICKUP Left cell index " + leftCell);
        Debug.Log("PICKUP Right cell index " + rightCell);

        CheckConnectionsPickupPiece(myCell, myCellIndex, leftCell, rightCell);
    }

    private void CheckConnectionsPickupPiece(Cell myCell, int myCellIndex, Cell leftCell, Cell rightCell)
    {
        if (myCell.heldPiece.leftChild.isGoodConnected)
        {
            GameManager.instance.unsuccessfullConnectionsCount++; // we just deleted a "good" connection from the board

            myCell.heldPiece.leftChild.isGoodConnected = false;
            leftCell.heldPiece.rightChild.isGoodConnected = false;

        }
        else
        {
            GameManager.instance.unsuccessfullConnectionsCount--; // we just deleted a "bad" connection from the board

        }

        if (myCell.heldPiece.rightChild.isGoodConnected)
        {
            GameManager.instance.unsuccessfullConnectionsCount++; // we just deleted a "good" connection from the board

            myCell.heldPiece.rightChild.isGoodConnected = false;
            rightCell.heldPiece.leftChild.isGoodConnected = false;
        }
        else
        {
            GameManager.instance.unsuccessfullConnectionsCount--; // we just deleted a "bad" connection from the board

        }
    }

    public void CheckConnection(Cell myCell, int myCellIndex)
    {
        Cell leftCell = GetCell(myCellIndex - 1);
        Cell rightCell = GetCell(myCellIndex + 1);


        Debug.Log("My cell index " + myCell);
        Debug.Log("Left cell index " + leftCell);
        Debug.Log("Right cell index " + rightCell);

        CheckConnectionsPlacePiece(myCell, myCellIndex, leftCell, rightCell);
    }

    public Cell GetCell(int index)
    {

        if (index < 0)
        {
            index = SliceManager.instance.boardCells.Length - 1;
        }

        if(index > SliceManager.instance.boardCells.Length - 1)
        {
            index = 0;
        }


        Cell cellOut = SliceManager.instance.boardCells[index];
        return cellOut;
    }
    private void CheckConnectionsPlacePiece(Cell myCell, int myCellIndex, Cell leftCell, Cell rightCell)
    {
        bool SuccessfulConnectLeft = false;
        bool SuccessfulConnectRight = false;

        if (leftCell.heldPiece)
        {
            BoardConnectionChecker leftResult = TotalCheck(myCell.heldPiece.leftChild, leftCell.heldPiece.rightChild);

            Slice leftSlice = CheckSliceExsists(myCellIndex);
            if (leftSlice.childSlice) // this is left Slice
            {
                SuccessfulConnectLeft = CheckSliceConditions(leftSlice, leftResult); // check good connection with left slice
            }
            else
            {
                if (leftResult.goodColorMatch || leftResult.goodSymbolMatch) // check good connection without slice
                {
                    SuccessfulConnectLeft = true;
                }
            }
        }

        if (rightCell.heldPiece)
        {
            BoardConnectionChecker rightResult = TotalCheck(myCell.heldPiece.rightChild, rightCell.heldPiece.leftChild);


            Slice rightSlice = CheckSliceExsists(myCellIndex + 1);
            if (rightSlice.childSlice) // this is right Slice
            {
                SuccessfulConnectRight = CheckSliceConditions(rightSlice, rightResult);// check good connection with right slice
            }
            else
            {
                if (rightResult.goodColorMatch || rightResult.goodSymbolMatch) // check good connection without slice
                {
                    SuccessfulConnectRight = true;
                }
            }
        }

        //// do something here after detecting good or bad connections        
        if (!SuccessfulConnectLeft)
        {
            GameManager.instance.unsuccessfullConnectionsCount++; // used for end level to see if win or lose
            myCell.heldPiece.leftChild.isGoodConnected = false; // used when we pick up the piece to decide how many bad connections remain on board

            if (leftCell.heldPiece)
            {
                leftCell.heldPiece.rightChild.isGoodConnected = false; // used when we pick up the piece to decide how many bad connections remain on board
            }
        }
        else
        {
            // we just "fixed" a bad connection here
            GameManager.instance.unsuccessfullConnectionsCount--; // used for end level to see if win or lose
            myCell.heldPiece.leftChild.isGoodConnected = true; // used when we pick up the piece to decide how many bad connections remain on board

            if (leftCell.heldPiece)
            {
                leftCell.heldPiece.rightChild.isGoodConnected = true; // used when we pick up the piece to decide how many bad connections remain on board
            }
        }

        if (!SuccessfulConnectRight)
        {
            GameManager.instance.unsuccessfullConnectionsCount++; // used for end level to see if win or lose
            myCell.heldPiece.rightChild.isGoodConnected = false; // used when we pick up the piece to decide how many bad connections remain on board

            if (rightCell.heldPiece)
            {
                rightCell.heldPiece.leftChild.isGoodConnected = false; // used when we pick up the piece to decide how many bad connections remain on board
            }
        }
        else
        {
            // we just "fixed" a bad connection here
            GameManager.instance.unsuccessfullConnectionsCount--; // used for end level to see if win or lose
            myCell.heldPiece.rightChild.isGoodConnected = true; // used when we pick up the piece to decide how many bad connections remain on board

            if (rightCell.heldPiece)
            {
                rightCell.heldPiece.leftChild.isGoodConnected = true; // used when we pick up the piece to decide how many bad connections remain on board
            }

        }
    }

    private bool CheckSliceConditions(Slice toCheck, BoardConnectionChecker result)
    {
        switch (toCheck.sliceCatagory)
        {
            case SliceCatagory.Shape:
                if (result.goodSymbolMatch)
                {
                    return true;
                }
                break;
            case SliceCatagory.Color:
                if (result.goodColorMatch)
                {
                    return true;
                }
                break;
            default:
                Debug.LogError("Slices Error");
                return false;
        }

        return false;
    }

    public BoardConnectionChecker TotalCheck(SubPiece current, SubPiece contested)
    {
        BoardConnectionChecker result = new BoardConnectionChecker();

        result.goodColorMatch = EqualColorOrJoker(current.colorOfPiece, contested.colorOfPiece);
        result.goodSymbolMatch = EqualSymbolOrJoker(current.symbolOfPiece, contested.symbolOfPiece);

        return result;
    }

    public bool EqualColorOrJoker(PieceColor colA, PieceColor colB) //Check colors match
    {
        if ((colA == colB || (colA == PieceColor.Joker || colB == PieceColor.Joker)))
        {
            return true;
        }

        return false;
    }

    public bool EqualSymbolOrJoker(PieceSymbol symA, PieceSymbol symB) //Check symbols match
    {
        if (symA == symB || (symA == PieceSymbol.Joker || symB == PieceSymbol.Joker))
        {
            return true;
        }

        return false;

    }

    private Slice CheckSliceExsists(int index)
    {
        if(index > SliceManager.instance.boardSlices.Length - 1)
        {
            index = 0;
        }

        Slice sliceOut = SliceManager.instance.boardSlices[index];
        return sliceOut;
    }
}
