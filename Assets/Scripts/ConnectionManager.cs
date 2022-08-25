using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class CellsObjetData
{
    public List<Cell> possibleCells = new List<Cell>();
    public CellsObjetData()
    {
        possibleCells = new List<Cell>();
    }
}

[System.Serializable]
public class PossibleCellPathList
{
    public List<CellsObjetData> possibleCellsPath = new List<CellsObjetData>();
}

[System.Serializable]
public class EdgePathFoundData
{
    public List<Cell> foundCells = new List<Cell>();
    public Cell lastCellToMove;

    public PieceSymbol leftAnimalSymbolNeeded;
    public PieceColor leftColorNeeded;

    public PieceSymbol rightAnimalSymbolNeeded;
    public PieceColor rightColorNeeded;

    public EdgePathFoundData()
    {
        foundCells = new List<Cell>();
    }
}

[System.Serializable]
public class FoundCellPath
{
    public List<EdgePathFoundData> foundCellPath = new List<EdgePathFoundData>();
}

public class ConnectionManager : MonoBehaviour
{
    public static ConnectionManager instance;
    public List<Cell> cells;

    [Header("Last clip algoritm zone")]
    public int movesMade;
    public bool hasFinishedAlgorithm;
    public float TEMPDelayTime;


    public EdgePathFoundData decidedAlgoritmPath = null;

    public PossibleCellPathList cellsThatCanConnect;
    public FoundCellPath pathsFound;
    public SubPiece[] subPiecesOnBoardTempAlgoritm;

    private List<Cell> bufferList;
    private List<Cell> previousEmptyCells;
    private Cell originalMissingCell;
    private int mostCurrentListIndex;

    private void Awake()
    {
        instance = this;

    }

    private void Start()
    {
        subPiecesOnBoardTempAlgoritm = new SubPiece[16];
        previousEmptyCells = new List<Cell>();
        bufferList = new List<Cell>();

        pathsFound.foundCellPath = new List<EdgePathFoundData>();
        cellsThatCanConnect.possibleCellsPath = new List<CellsObjetData>();

    }

    public void GrabCellList(Transform gb)
    {
        cells.Clear();

        foreach (Cell c in gb.GetComponentsInChildren<Cell>())
        {
            cells.Add(c);
        }
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

            myCell.ResetConnectionDisplays();

            SoundManager.instance.PlaySound(SoundType.TileUnmatch);
        }
        else
        {
            GameManager.instance.unsuccessfullConnectionsCount--; // we just deleted a "bad" connection from the board
            myCell.ResetConnectionDisplays();

        }

        if (myCell.heldPiece.rightChild.isGoodConnected)
        {
            GameManager.instance.unsuccessfullConnectionsCount++; // we just deleted a "good" connection from the board

            myCell.heldPiece.rightChild.isGoodConnected = false;
            rightCell.heldPiece.leftChild.isGoodConnected = false;
            myCell.ResetConnectionDisplays();

            SoundManager.instance.PlaySound(SoundType.TileUnmatch);
        }
        else
        {
            GameManager.instance.unsuccessfullConnectionsCount--; // we just deleted a "bad" connection from the board
            myCell.ResetConnectionDisplays();

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

        myCell.ResetConnectionDisplays();

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
                myCell.badConnectLeft.gameObject.SetActive(true);
                myCell.badConnectLeft.GetComponent<Animator>().enabled = true;
                myCell.badConnectLeft.SetTrigger("Activate");
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
                myCell.goodConnectLeft.gameObject.SetActive(true);
                myCell.badConnectLeft.GetComponent<Animator>().enabled = true;
                myCell.goodConnectLeft.SetTrigger("Activate");

                SoundManager.instance.PlaySound(SoundType.TileMatch);
            }

        }

        if (!SuccessfulConnectRight)
        {
            GameManager.instance.unsuccessfullConnectionsCount++; // used for end level to see if win or lose
            myCell.heldPiece.rightChild.isGoodConnected = false; // used when we pick up the piece to decide how many bad connections remain on board

            if (rightCell.heldPiece)
            {
                rightCell.heldPiece.leftChild.isGoodConnected = false; // used when we pick up the piece to decide how many bad connections remain on board
                myCell.badConnectRight.gameObject.SetActive(true);

                myCell.badConnectRight.SetTrigger("Activate");
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
                myCell.goodConnectRight.gameObject.SetActive(true);

                myCell.goodConnectRight.SetTrigger("Activate");

                SoundManager.instance.PlaySound(SoundType.TileMatch);
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







    public void StartLastClipAlgoritm()
    {
        ResetAllLastPieceAlgoritmData();

        foreach (Cell cell in cells)
        {
            if (!cell.isFull)
            {
                Debug.LogError("Origin cell is: " + cell.name);
                originalMissingCell = cell;

                //StartCoroutine(RecursiveCheckes(cell));
                RecursiveCheckes(cell);

                break;
            }
        }

    }

    public void RecursiveCheckes(Cell CurrentEmptyCell)
    {
        CheckAllPossibleMovesToEmptyCell(CurrentEmptyCell);
    }

    public void CheckAllPossibleMovesToEmptyCell(Cell EmptyCell)
    {
        if (GameManager.instance.currentMap.algoritmStepsWanted == 0)
        {
            SetDataEndPathNoSteps(EmptyCell, 0);

            hasFinishedAlgorithm = true;

            return;
        }



        bool hasFoundOptions = false;

        cellsThatCanConnect.possibleCellsPath.Add(new CellsObjetData());
        mostCurrentListIndex = cellsThatCanConnect.possibleCellsPath.Count - 1;

        Cell previousCell = null;

        if (previousEmptyCells.Count > 0)
        {
            previousCell = previousEmptyCells.Last();
        }

        foreach (Cell cellCompareTo in cells)
        {
            if (cellCompareTo != previousCell && cellCompareTo != EmptyCell)
            {
                if (CheckConnectionsAlgoritm(EmptyCell, cellCompareTo))
                {
                    cellsThatCanConnect.possibleCellsPath[mostCurrentListIndex].possibleCells.Add(cellCompareTo);
                }
            }
        }


        if (cellsThatCanConnect.possibleCellsPath[mostCurrentListIndex].possibleCells.Count == 0)
        {
            hasFoundOptions = false;

            Debug.LogError("Called from second!!");

            MoveBackInList(EmptyCell);

        }
        else
        {
            hasFoundOptions = true;
        }

        if (mostCurrentListIndex >= 0)
        {
            movesMade++;

            int tempCheck = 0;

            if (movesMade == GameManager.instance.currentMap.algoritmStepsWanted)
            {
                foreach (Cell edgeCell in cellsThatCanConnect.possibleCellsPath[mostCurrentListIndex].possibleCells)
                {
                    tempCheck++;

                    if (pathsFound.foundCellPath.Count == 500)
                    {
                        Debug.LogError("Done with alogoritm, no more needed");

                        decidedAlgoritmPath = ChooseRandomFoundPathAlgo();
                        hasFinishedAlgorithm = true;
                        return;
                    }


                    pathsFound.foundCellPath.Add(new EdgePathFoundData());
                    int lastIndex = pathsFound.foundCellPath.Count - 1;

                    pathsFound.foundCellPath[lastIndex].foundCells.AddRange(bufferList);
                    pathsFound.foundCellPath[lastIndex].foundCells.Add(edgeCell);
                    pathsFound.foundCellPath[lastIndex].lastCellToMove = edgeCell;

                    ChangeSubPiecesAlgorithmData(edgeCell, EmptyCell);

                    Debug.LogError("Setting end data path!");

                    SetDataEndPath(edgeCell, lastIndex);

                    ChangeSubPiecesAlgorithmData(EmptyCell, edgeCell);

                }

                MoveBackInList(EmptyCell);

                if (cellsThatCanConnect.possibleCellsPath.Count == 0)
                {
                    Debug.LogError("Done with algoritm not reached max options");

                    if (pathsFound.foundCellPath.Count > 0)
                    {
                        decidedAlgoritmPath = ChooseRandomFoundPathAlgo();
                    }
                    else
                    {
                        Debug.LogError("Can't find path - no path in list");
                    }

                    hasFinishedAlgorithm = true;

                    return;
                }

                if (mostCurrentListIndex >= 0)
                {
                    Cell newEmptyCell = cellsThatCanConnect.possibleCellsPath[mostCurrentListIndex].possibleCells[0];
                    bufferList.Add(newEmptyCell);



                    ChangeSubPiecesAlgorithmData(newEmptyCell, previousEmptyCells.Last());

                    RecursiveCheckes(newEmptyCell);
                }
                else
                {
                    Debug.Log("Done with alogoritm, no more options");
                    return;
                }
            }
            else
            {
                if (hasFoundOptions)
                {
                    if (previousEmptyCells.Count == 0)
                    {
                        previousEmptyCells.Add(originalMissingCell);
                    }
                    else
                    {
                        previousEmptyCells.Add(EmptyCell); // remeber the cell that we just moved to since we don't want to move from that cell.
                    }
                }

                Cell newEmptyCell = cellsThatCanConnect.possibleCellsPath[mostCurrentListIndex].possibleCells[0];
                bufferList.Add(newEmptyCell);

                Debug.LogError("Updated buffer");

                ChangeSubPiecesAlgorithmData(newEmptyCell, previousEmptyCells.Last());

                Debug.LogError("Moved Piece");

                RecursiveCheckes(newEmptyCell);
            }

        }
        else
        {
            Debug.LogError("Done!!!");

            if (pathsFound.foundCellPath.Count > 0)
            {
                decidedAlgoritmPath = ChooseRandomFoundPathAlgo();
            }
            else
            {
                Debug.LogError("Can't find path - no path in list");
            }

            hasFinishedAlgorithm = true;

        }
    }

    void ChangeSubPiecesAlgorithmData(Cell cellFrom, Cell cellTo)
    {
        int fromIndex = cellFrom.cellIndex;
        int toIndex = cellTo.cellIndex;

        int CellFromSubPieceLeft = CheckIntRangeSubPiecesAlgoritm(fromIndex * 2);
        int CellFromSubPieceRight = CheckIntRangeSubPiecesAlgoritm((fromIndex * 2) + 1);

        int CellToSubPieceLeft = CheckIntRangeSubPiecesAlgoritm(toIndex * 2);
        int CellToSubPieceRight = CheckIntRangeSubPiecesAlgoritm((toIndex * 2) + 1);

        subPiecesOnBoardTempAlgoritm[CellToSubPieceLeft] = subPiecesOnBoardTempAlgoritm[CellFromSubPieceLeft];
        subPiecesOnBoardTempAlgoritm[CellToSubPieceRight] = subPiecesOnBoardTempAlgoritm[CellFromSubPieceRight];

        subPiecesOnBoardTempAlgoritm[CellFromSubPieceLeft] = null;
        subPiecesOnBoardTempAlgoritm[CellFromSubPieceRight] = null;
    }


    void MoveBackInList(Cell currentEmptyCell)
    {
        do
        {
            cellsThatCanConnect.possibleCellsPath.Remove(cellsThatCanConnect.possibleCellsPath[mostCurrentListIndex]);

            mostCurrentListIndex--;
            movesMade--;

            if (mostCurrentListIndex >= 0)
            {
                cellsThatCanConnect.possibleCellsPath[mostCurrentListIndex].possibleCells.RemoveAt(0);

                if (previousEmptyCells.Count > 0)
                {
                    ChangeSubPiecesAlgorithmData(previousEmptyCells.Last(), bufferList.Last());
                }

                Debug.LogError("Right after move pieces from in list");

                if (cellsThatCanConnect.possibleCellsPath[mostCurrentListIndex].possibleCells.Count == 0)
                {
                    previousEmptyCells.RemoveAt(previousEmptyCells.Count - 1);
                }

                bufferList.RemoveAt(bufferList.Count - 1);
            }
            else
            {
                break;
            }

        } while (cellsThatCanConnect.possibleCellsPath[mostCurrentListIndex].possibleCells.Count == 0);
    }

    bool CheckConnectionsAlgoritm(Cell EmptyCell, Cell checkAgainst)
    {
        int mycellIndex = EmptyCell.cellIndex;
        int againstCellIndex = checkAgainst.cellIndex;

        int leftContested = CheckIntRangeSubPiecesAlgoritm((mycellIndex * 2) - 1);
        int rightContested = CheckIntRangeSubPiecesAlgoritm((mycellIndex * 2) + 2);

        int currentCellLeft = CheckIntRangeSubPiecesAlgoritm(againstCellIndex * 2);
        int currentCellRight = CheckIntRangeSubPiecesAlgoritm((againstCellIndex * 2) + 1);

        bool conditionmet;
        bool isGoodConnect;

        bool adjacentLeft;
        bool adjacentRight;

        CheckCellAdjecency(EmptyCell, checkAgainst, out adjacentLeft, out adjacentRight);

        if (adjacentLeft)
        {
            if (CheckSubPieceConnectionAlgoritm(false, subPiecesOnBoardTempAlgoritm[rightContested], subPiecesOnBoardTempAlgoritm[currentCellRight], EmptyCell, out conditionmet, out isGoodConnect))
            {
                if (!conditionmet)
                {

                }
                else
                {
                    return true;
                }
            }
        }

        if (adjacentRight)
        {
            if (CheckSubPieceConnectionAlgoritm(true, subPiecesOnBoardTempAlgoritm[leftContested], subPiecesOnBoardTempAlgoritm[currentCellLeft], EmptyCell, out conditionmet, out isGoodConnect))
            {
                if (!conditionmet)
                {

                }
                else
                {
                    return true;
                }
            }
        }


        if (CheckSubPieceConnectionAlgoritm(true, subPiecesOnBoardTempAlgoritm[leftContested], subPiecesOnBoardTempAlgoritm[currentCellLeft], EmptyCell, out conditionmet, out isGoodConnect))
        {
            if (CheckSubPieceConnectionAlgoritm(false, subPiecesOnBoardTempAlgoritm[rightContested], subPiecesOnBoardTempAlgoritm[currentCellRight], EmptyCell, out conditionmet, out isGoodConnect))
            {
                if (!conditionmet)
                {

                }
                else
                {
                    return true;
                }
            }
        }

        return false;

    }

    void CheckCellAdjecency(Cell currentEmptyCell, Cell cellToCheck, out bool adjacentLeft, out bool adjacentRight)
    {
        int emptyCellIndex = currentEmptyCell.cellIndex;
        int cellToCheckIndex = cellToCheck.cellIndex;

        adjacentLeft = false;
        adjacentRight = false;

        if (cellToCheckIndex == CheckIntRangeCells(emptyCellIndex - 1))
        {
            adjacentLeft = true;
            return;
        }

        if (cellToCheckIndex == CheckIntRangeCells(emptyCellIndex + 1))
        {
            adjacentRight = true;
            return;
        }
    }

    void SetDataEndPath(Cell edgeCell, int index)
    {
        int edgeCellIndex = edgeCell.cellIndex;

        int leftContested = CheckIntRangeSubPiecesAlgoritm((edgeCellIndex * 2) - 1);
        int rightContested = CheckIntRangeSubPiecesAlgoritm((edgeCellIndex * 2) + 2);

        pathsFound.foundCellPath[index].leftAnimalSymbolNeeded = subPiecesOnBoardTempAlgoritm[leftContested].symbolOfPiece;
        pathsFound.foundCellPath[index].leftColorNeeded = subPiecesOnBoardTempAlgoritm[leftContested].colorOfPiece;

        pathsFound.foundCellPath[index].rightAnimalSymbolNeeded = subPiecesOnBoardTempAlgoritm[rightContested].symbolOfPiece;
        pathsFound.foundCellPath[index].rightColorNeeded = subPiecesOnBoardTempAlgoritm[rightContested].colorOfPiece;

    }

    void SetDataEndPathNoSteps(Cell edgeCell, int index)
    {
        int edgeCellIndex = edgeCell.cellIndex;

        int leftContested = CheckIntRangeSubPiecesAlgoritm((edgeCellIndex * 2) - 1);
        int rightContested = CheckIntRangeSubPiecesAlgoritm((edgeCellIndex * 2) + 2);

        EdgePathFoundData path = new EdgePathFoundData();

        path.foundCells.Add(edgeCell);

        path.leftAnimalSymbolNeeded = subPiecesOnBoardTempAlgoritm[leftContested].symbolOfPiece;
        path.leftColorNeeded = subPiecesOnBoardTempAlgoritm[leftContested].colorOfPiece;

        path.rightAnimalSymbolNeeded = subPiecesOnBoardTempAlgoritm[rightContested].symbolOfPiece;
        path.rightColorNeeded = subPiecesOnBoardTempAlgoritm[rightContested].colorOfPiece;

        decidedAlgoritmPath = path;

    }

    public int CheckIntRangeSubPiecesAlgoritm(int num)
    {
        if (num < 0)
        {
            return subPiecesOnBoardTempAlgoritm.Length - 1;
        }

        if (num >= subPiecesOnBoardTempAlgoritm.Length)
        {
            return 0;
        }

        return num;
    }

    public int CheckIntRangeCells(int num)
    {
        if (num < 0)
        {
            return cells.Count - 1;
        }

        if (num >= cells.Count)
        {
            return 0;
        }


        return num;
    }

    public bool CheckSubPieceConnectionAlgoritm(bool isLeft, SubPiece currentSide, SubPiece contestedSide, Cell cellTo, out bool conditionMet, out bool isGoodConnect)
    {
        conditionMet = true;
        isGoodConnect = false;

        if (isLeft)
        {
            if (cellTo.leftSlice.sliceCatagory != SliceCatagory.None)
            {
                BoardConnectionChecker result = TotalCheck(currentSide, contestedSide);

                if (result.goodColorMatch)
                {
                    isGoodConnect = true;
                }

                if (result.goodSymbolMatch)
                {
                    isGoodConnect = true;
                }

                conditionMet = CheckFulfilledSliceCondition(cellTo.leftSlice, result, currentSide, contestedSide);

                if (conditionMet)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                BoardConnectionChecker result = TotalCheck(currentSide, contestedSide);

                if (result.goodColorMatch)
                {
                    isGoodConnect = true;
                }

                if (result.goodSymbolMatch)
                {
                    isGoodConnect = true;
                }

                if (isGoodConnect)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        else
        {

            if (cellTo.rightSlice.sliceCatagory != SliceCatagory.None)
            {
                BoardConnectionChecker result = TotalCheck(currentSide, contestedSide);

                if (result.goodColorMatch)
                {
                    isGoodConnect = true;
                }

                if (result.goodSymbolMatch)
                {
                    isGoodConnect = true;
                }

                conditionMet = CheckFulfilledSliceCondition(cellTo.rightSlice, result, currentSide, contestedSide);

                if (conditionMet)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                BoardConnectionChecker result = TotalCheck(currentSide, contestedSide);

                if (result.goodColorMatch)
                {
                    isGoodConnect = true;
                }

                if (result.goodSymbolMatch)
                {
                    isGoodConnect = true;
                }

                if (isGoodConnect)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }

    public EdgePathFoundData ChooseRandomFoundPathAlgo()
    {
        EdgePathFoundData path = null;

        int randomNum = Random.Range(0, pathsFound.foundCellPath.Count);

        path = pathsFound.foundCellPath[randomNum];

        if (path == null)
        {
            Debug.LogError("Big problem here");
            return null;
        }
        else
        {
            return path;
        }
    }

    public void ResetAllLastPieceAlgoritmData()
    {
        cellsThatCanConnect.possibleCellsPath.Clear();
        pathsFound.foundCellPath.Clear();
        bufferList.Clear();


        previousEmptyCells.Clear();

        mostCurrentListIndex = 0;
        movesMade = 0;

        originalMissingCell = null;
        decidedAlgoritmPath = null;

        hasFinishedAlgorithm = false;
    }

    public bool CheckFulfilledSliceCondition(Slice relevent, BoardConnectionChecker result, SubPiece a, SubPiece b)
    {
        bool isConditionMet = false;

        switch (relevent.sliceCatagory)
        {
            case SliceCatagory.Shape:
                if (result.goodSymbolMatch)
                {
                    isConditionMet = true;
                }
                break;
            case SliceCatagory.Color:
                if (result.goodColorMatch)
                {
                    isConditionMet = true;
                }
                break;
            default:
                isConditionMet = false;
                break;
        }

        return isConditionMet;
    }

}
