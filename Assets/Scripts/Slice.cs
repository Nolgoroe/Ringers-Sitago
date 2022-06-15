using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slice : MonoBehaviour
{
    public GameObject childSlice;

    public SliceCatagory sliceCatagory;

    public PieceColor sliceColor;
    public PieceSymbol sliceSymbol;
    //public SliceCatagory sliceCatagory;
    //public GameObject lootIcon;

    //public GameObject child;

    //public bool hasSlice;
    //public bool isLock;
    //public bool isLoot;
    //public bool isLimiter;
    //public bool fulfilledCondition;
    //public bool isKey;

    //public int sliceIndex;

    //public Animator anim;
    ////public List<Cell> connectedCells;
    //public Animator lockSpriteAnim;
    //public Animator lockSpriteHeighlightAnim;


    public void SetSliceData(Transform parent, SliceCatagory sc)
    {
        sliceCatagory = sc;

        InstantiateSlice();

        //connectedCells.Add(CheckIntRangeSliceCells(sliceIndex)); ///// CHECK TO SEE IF CAN DO BETTER
        //connectedCells.Add(CheckIntRangeSliceCells(sliceIndex-1));  ///// CHECK TO SEE IF CAN DO BETTER
    }

    public void InstantiateSlice()
    {
        GameObject go = Instantiate(SliceManager.instance.slicePrefab, transform);

        Image imageComponent = go.transform.GetComponent<Image>();

        childSlice = go;

        SetRendereData(imageComponent);
    }

    public void SetRendereData(Image imageComponent)
    {
        switch (sliceCatagory)
        {
            case SliceCatagory.Shape:
                sliceSymbol = PieceSymbol.General;

                imageComponent.sprite = SliceManager.instance.limiterSliceSymbolToSprite[sliceSymbol];

                break;
            case SliceCatagory.Color:
                sliceColor = PieceColor.General;

                imageComponent.sprite = SliceManager.instance.limiterSlicecolorToSprite[sliceColor];

                break;
            default:
                break;
        }

        //}
        //void InstantiateNonLimiterSlice(int pieceSymbolEnumCount, int pieceColorEnumCount)
        //{
        //    GameObject go = Instantiate(GameManager.Instance.sliceManager.slicePrefab, transform);
        //    Renderer rend = go.transform.GetChild(0).GetComponent<Renderer>();
        //    Material mat = rend.material;
        //    anim = go.GetComponent<Animator>();
        //    //rend.material = mat;
        //    GameObject ropeChild = go.transform.GetChild(1).gameObject;
        //    ropeChild.SetActive(false);

        //    //SpriteRenderer sr = go.transform.GetChild(0).GetComponent<SpriteRenderer>();
        //    child = go;

        //    SetRendererDataNonLimiter(mat, pieceSymbolEnumCount, pieceColorEnumCount);

        //    if (!GameManager.Instance.currentLevel.allowRepeatSlices)
        //    {
        //        if (sliceCatagory == SliceCatagory.SpecificColor || sliceCatagory == SliceCatagory.SpecificShape)
        //        {
        //            bool repetingSlices = CheckRepeatingSlices(this);

        //            int tries = 0;

        //            while (repetingSlices)
        //            {

        //                tries++;

        //                if (tries == 1000)
        //                {
        //                    Debug.LogError("There are repeat slices but code won't allow");
        //                    break;
        //                }

        //                SetRendererDataNonLimiter(mat, pieceSymbolEnumCount, pieceColorEnumCount);
        //                repetingSlices = CheckRepeatingSlices(this);
        //            }
        //        }
        //    }
        //}

        //public void SetRendererDataNonLimiter(Material matArray, int pieceSymbolEnumCount, int pieceColorEnumCount)
        //{
        //    if (!GameManager.Instance.isDisableTutorials && GameManager.Instance.currentLevel.isTutorial)
        //    {
        //        switch (sliceCatagory)
        //        {
        //            case SliceCatagory.Shape:
        //                sliceSymbol = PieceSymbol.None;
        //                //sr.sprite = GameManager.Instance.sliceManager.pieceSymbolToSprite[sliceSymbol];
        //                matArray.SetTexture("_BaseMap", GameManager.Instance.sliceManager.sliceSymbolToSprite[sliceSymbol]);
        //                //sr.sprite = GameManager.Instance.sliceManager.lootSliceSymbolDict[sliceSymbol];
        //                break;
        //            case SliceCatagory.Color:
        //                sliceColor = PieceColor.None;
        //                //sr.sprite = GameManager.Instance.sliceManager.piececolorToSprite[sliceColor];
        //                matArray.SetTexture("_BaseMap", GameManager.Instance.sliceManager.slicecolorToSprite[sliceColor]);
        //                //sr.color = GameManager.Instance.sliceManager.pieceColorToColor[PieceColor.None];
        //                //sr.sprite = GameManager.Instance.sliceManager.lootSliceColorDict[sliceColor];

        //                break;
        //            case SliceCatagory.SpecificShape:
        //                sliceSymbol = GameManager.Instance.copyOfSpecificSliceSymbolsTutorial[0];
        //                //sr.sprite = GameManager.Instance.sliceManager.pieceSymbolToSprite[sliceSymbol];
        //                matArray.SetTexture("_BaseMap", GameManager.Instance.sliceManager.sliceSymbolToSprite[sliceSymbol]);
        //                //sr.sprite = GameManager.Instance.sliceManager.lootSliceSymbolDict[sliceSymbol];

        //                GameManager.Instance.copyOfSpecificSliceSymbolsTutorial.RemoveAt(0);
        //                break;
        //            case SliceCatagory.SpecificColor:
        //                sliceColor = GameManager.Instance.copyOfSpecificSliceColorsTutorial[0];
        //                //sr.sprite = GameManager.Instance.sliceManager.piececolorToSprite[sliceColor];
        //                matArray.SetTexture("_BaseMap", GameManager.Instance.sliceManager.slicecolorToSprite[sliceColor]);
        //                //sr.color = GameManager.Instance.sliceManager.pieceColorToColor[sliceColor];
        //                //sr.sprite = GameManager.Instance.sliceManager.lootSliceColorDict[sliceColor];

        //                GameManager.Instance.copyOfSpecificSliceColorsTutorial.RemoveAt(0);
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        switch (sliceCatagory)
        //        {
        //            case SliceCatagory.Shape:
        //                sliceSymbol = PieceSymbol.None;
        //                //sr.sprite = GameManager.Instance.sliceManager.pieceSymbolToSprite[sliceSymbol];
        //                matArray.SetTexture("_BaseMap", GameManager.Instance.sliceManager.sliceSymbolToSprite[sliceSymbol]);
        //                //sr.sprite = GameManager.Instance.sliceManager.lootSliceSymbolDict[sliceSymbol];
        //                break;
        //            case SliceCatagory.Color:
        //                sliceColor = PieceColor.None;
        //                //sr.sprite = GameManager.Instance.sliceManager.piececolorToSprite[sliceColor];
        //                matArray.SetTexture("_BaseMap", GameManager.Instance.sliceManager.slicecolorToSprite[sliceColor]);
        //                //sr.color = GameManager.Instance.sliceManager.pieceColorToColor[PieceColor.None];
        //                //sr.sprite = GameManager.Instance.sliceManager.lootSliceColorDict[sliceColor];

        //                break;
        //            case SliceCatagory.SpecificShape:
        //                if (GameManager.Instance.currentLevel.levelAvailablesymbols.Length > 0)
        //                {
        //                    int rand = Random.Range(0, GameManager.Instance.currentLevel.levelAvailablesymbols.Length);
        //                    sliceSymbol = GameManager.Instance.currentLevel.levelAvailablesymbols[rand];

        //                    matArray.SetTexture("_BaseMap", GameManager.Instance.sliceManager.sliceSymbolToSprite[sliceSymbol]);
        //                }
        //                else
        //                {
        //                    sliceSymbol = (PieceSymbol)Random.Range(0, pieceSymbolEnumCount - 2);

        //                    matArray.SetTexture("_BaseMap", GameManager.Instance.sliceManager.sliceSymbolToSprite[sliceSymbol]);
        //                }
        //                break;
        //            case SliceCatagory.SpecificColor:
        //                if (GameManager.Instance.currentLevel.levelAvailableColors.Length > 0)
        //                {
        //                    int rand = Random.Range(0, GameManager.Instance.currentLevel.levelAvailableColors.Length);
        //                    sliceColor = GameManager.Instance.currentLevel.levelAvailableColors[rand];

        //                    matArray.SetTexture("_BaseMap", GameManager.Instance.sliceManager.slicecolorToSprite[sliceColor]);
        //                }
        //                else
        //                {
        //                    sliceColor = (PieceColor)Random.Range(0, pieceColorEnumCount - 2);

        //                    matArray.SetTexture("_BaseMap", GameManager.Instance.sliceManager.slicecolorToSprite[sliceColor]);
        //                }
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //}
        ////public void InstantiateLootSlice(int pieceSymbolEnumCount, int pieceColorEnumCount)
        ////{
        ////    //GameObject go = Instantiate(GameManager.Instance.sliceManager.lootSlicePrefab, transform);
        ////    GameObject go = Instantiate(GameManager.Instance.sliceManager.slicePrefab, transform);
        ////    Renderer rend = go.GetComponent<Renderer>();
        ////    Material[] matArray = rend.materials;
        ////    rend.materials = matArray;

        ////    //SpriteRenderer sr = go.transform.GetChild(0).GetComponent<SpriteRenderer>();

        ////    child = go;
        ////    switch (sliceCatagory)
        ////    {
        ////        case SliceCatagory.Shape:
        ////            sliceSymbol = PieceSymbol.None;
        ////            //sr.sprite = GameManager.Instance.sliceManager.pieceSymbolToSprite[sliceSymbol];
        ////            matArray[1].SetTexture("_BaseMap", GameManager.Instance.sliceManager.pieceSymbolToSprite[sliceSymbol]);
        ////            //sr.sprite = GameManager.Instance.sliceManager.lootSliceSymbolDict[sliceSymbol];
        ////            break;
        ////        case SliceCatagory.Color:
        ////            sliceColor = PieceColor.None;
        ////            //sr.sprite = GameManager.Instance.sliceManager.piececolorToSprite[sliceColor];
        ////            matArray[1].SetTexture("_BaseMap", GameManager.Instance.sliceManager.piececolorToSprite[sliceColor]);
        ////            //sr.color = GameManager.Instance.sliceManager.pieceColorToColor[PieceColor.None];
        ////            //sr.sprite = GameManager.Instance.sliceManager.lootSliceColorDict[sliceColor];

        ////            break;
        ////        case SliceCatagory.SpecificShape:
        ////            sliceSymbol = (PieceSymbol)Random.Range(0, pieceSymbolEnumCount - 2);
        ////            //sr.sprite = GameManager.Instance.sliceManager.pieceSymbolToSprite[sliceSymbol];
        ////            matArray[1].SetTexture("_BaseMap", GameManager.Instance.sliceManager.pieceSymbolToSprite[sliceSymbol]);
        ////            //sr.sprite = GameManager.Instance.sliceManager.lootSliceSymbolDict[sliceSymbol];
        ////            break;
        ////        case SliceCatagory.SpecificColor:
        ////            sliceColor = (PieceColor)Random.Range(0, pieceColorEnumCount - 2);
        ////            //sr.sprite = GameManager.Instance.sliceManager.piececolorToSprite[sliceColor];
        ////            matArray[1].SetTexture("_BaseMap", GameManager.Instance.sliceManager.piececolorToSprite[sliceColor]);
        ////            //sr.color = GameManager.Instance.sliceManager.pieceColorToColor[sliceColor];
        ////            //sr.sprite = GameManager.Instance.sliceManager.lootSliceColorDict[sliceColor];
        ////            break;
        ////        default:
        ////            break;
        ////    }
        ////}

        ////public void InstantiateLootLockSlice(int pieceSymbolEnumCount, int pieceColorEnumCount)
        ////{
        ////    //GameObject go = Instantiate(GameManager.Instance.sliceManager.lootLockSlicePrefab, transform);
        ////    GameObject go = Instantiate(GameManager.Instance.sliceManager.slicePrefab, transform);
        ////    Renderer rend = go.GetComponent<Renderer>();
        ////    Material[] matArray = rend.materials;
        ////    rend.materials = matArray;

        ////    //SpriteRenderer sr = go.transform.GetChild(0).GetComponent<SpriteRenderer>();
        ////    child = go;

        ////    switch (sliceCatagory)
        ////    {
        ////        case SliceCatagory.Shape:
        ////            sliceSymbol = PieceSymbol.None;
        ////            sr.sprite = GameManager.Instance.sliceManager.pieceSymbolToSprite[PieceSymbol.None];
        ////            //sr.sprite = GameManager.Instance.sliceManager.lootLockSliceSymbolSpritesDict[sliceSymbol];
        ////            break;
        ////        case SliceCatagory.Color:
        ////            sliceColor = PieceColor.None;
        ////            sr.sprite = GameManager.Instance.sliceManager.piececolorToSprite[sliceColor];
        ////            //sr.color = GameManager.Instance.sliceManager.pieceColorToColor[PieceColor.None];
        ////            //sr.sprite = GameManager.Instance.sliceManager.lootLockSliceColorDict[sliceColor];

        ////            break;
        ////        case SliceCatagory.SpecificShape:
        ////            sliceSymbol = (PieceSymbol)Random.Range(0, pieceSymbolEnumCount - 2);
        ////            sr.sprite = GameManager.Instance.sliceManager.pieceSymbolToSprite[sliceSymbol];
        ////            //sr.sprite = GameManager.Instance.sliceManager.lootLockSliceSymbolSpritesDict[sliceSymbol];
        ////            break;
        ////        case SliceCatagory.SpecificColor:
        ////            sliceColor = (PieceColor)Random.Range(0, pieceColorEnumCount - 2);
        ////            sr.sprite = GameManager.Instance.sliceManager.piececolorToSprite[sliceColor];
        ////            //sr.color = GameManager.Instance.sliceManager.pieceColorToColor[sliceColor];
        ////            //sr.sprite = GameManager.Instance.sliceManager.lootLockSliceColorDict[sliceColor];
        ////            break;
        ////        default:
        ////            break;
        ////    }
        ////}

        ////public void InstantiateLootLockLimiterSlice(int pieceSymbolEnumCount, int pieceColorEnumCount)
        ////{
        ////    //GameObject go = Instantiate(GameManager.Instance.sliceManager.lootLockLimiterSlicePrefab, transform);
        ////    GameObject go = Instantiate(GameManager.Instance.sliceManager.slicePrefab, transform);
        ////    Renderer rend = go.GetComponent<Renderer>();
        ////    Material[] matArray = rend.materials;
        ////    rend.materials = matArray;

        ////    //SpriteRenderer sr = go.transform.GetChild(0).GetComponent<SpriteRenderer>();
        ////    child = go;

        ////    switch (sliceCatagory)
        ////    {
        ////        case SliceCatagory.Shape:
        ////            sliceSymbol = PieceSymbol.None;
        ////            sr.sprite = GameManager.Instance.sliceManager.pieceSymbolToSprite[PieceSymbol.None];
        ////            //sr.sprite = GameManager.Instance.sliceManager.lootLockSliceSymbolSpritesDict[sliceSymbol];
        ////            break;
        ////        case SliceCatagory.Color:
        ////            sliceColor = PieceColor.None;
        ////            sr.sprite = GameManager.Instance.sliceManager.piececolorToSprite[sliceColor];
        ////            //sr.color = GameManager.Instance.sliceManager.pieceColorToColor[PieceColor.None];
        ////            //sr.sprite = GameManager.Instance.sliceManager.lootLockSliceColorDict[sliceColor];

        ////            break;
        ////        case SliceCatagory.SpecificShape:
        ////            sliceSymbol = (PieceSymbol)Random.Range(0, pieceSymbolEnumCount - 2);
        ////            sr.sprite = GameManager.Instance.sliceManager.pieceSymbolToSprite[sliceSymbol];
        ////            //sr.sprite = GameManager.Instance.sliceManager.lootLockSliceSymbolSpritesDict[sliceSymbol];
        ////            break;
        ////        case SliceCatagory.SpecificColor:
        ////            sliceColor = (PieceColor)Random.Range(0, pieceColorEnumCount - 2);
        ////            sr.sprite = GameManager.Instance.sliceManager.piececolorToSprite[sliceColor];
        ////            //sr.color = GameManager.Instance.sliceManager.pieceColorToColor[sliceColor];
        ////            //sr.sprite = GameManager.Instance.sliceManager.lootLockSliceColorDict[sliceColor];
        ////            break;
        ////        default:
        ////            break;
        ////    }
        ////}

        //public void ResetDate()
        //{
        //    sliceColor = PieceColor.None;
        //    sliceSymbol = PieceSymbol.None;
        //    sliceCatagory = SliceCatagory.None;
        //    isLock = false;
        //    isLoot = false;
        //    isLimiter = false;
        //    fulfilledCondition = false;
        //    hasSlice = false;
        //}

        ////public Cell CheckIntRangeSliceCells(int num)
        ////{
        ////    if (num < 0)
        ////    {
        ////        return ConnectionManager.Instance.cells[ConnectionManager.Instance.cells.Count - 1];
        ////    }
        ////    else if (num > ConnectionManager.Instance.cells.Count)
        ////    {
        ////        return ConnectionManager.Instance.cells[0];
        ////    }
        ////    else
        ////    {
        ////        return ConnectionManager.Instance.cells[num];
        ////    }
        ////}

        //public void SetEmpty()
        //{
        //    Renderer rend = child.transform.GetChild(0).GetComponent<Renderer>();
        //    Material[] matArray = rend.materials;
        //    matArray[0].SetTexture("_BaseMap", GameManager.Instance.sliceManager.emptyScrollTexture);
        //}

        //private bool CheckRepeatingSlices(Slice current)
        //{
        //    bool sameSlice = false;

        //    for (int i = 0; i < GameManager.Instance.sliceManager.boardSlices.Length; i++)
        //    {
        //        Slice compareTo = GameManager.Instance.sliceManager.boardSlices[i].GetComponent<Slice>();

        //        if (current != compareTo)
        //        {
        //            sameSlice = CompareSlices(current, compareTo);

        //            if (sameSlice)
        //            {
        //                Debug.Log("SAME SLICES! " + current.name + " " + compareTo.name);
        //                return sameSlice;
        //            }
        //        }
        //    }

        //    return false;
        //}

        //private bool CompareSlices(Slice currentSlice, Slice compareTo)
        //{
        //    bool sameSlice = false;

        //    if (currentSlice.sliceSymbol != PieceSymbol.None)
        //    {
        //        if (currentSlice.sliceSymbol == compareTo.sliceSymbol)
        //        {
        //            sameSlice = true;
        //        }
        //    }

        //    if (currentSlice.sliceColor != PieceColor.None)
        //    {
        //        if (currentSlice.sliceColor == compareTo.sliceColor)
        //        {
        //            sameSlice = true;
        //        }
        //    }

        //    return sameSlice;
        //}
    }
}
