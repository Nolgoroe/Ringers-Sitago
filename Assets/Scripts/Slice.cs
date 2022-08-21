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


    public void SetSliceData(Transform parent, SliceCatagory sc)
    {
        sliceCatagory = sc;

        InstantiateSlice();
    }

    public void InstantiateSlice()
    {
        GameObject go = Instantiate(SliceManager.instance.slicePrefab, transform);

        SpriteRenderer renderer = go.transform.GetComponent<SpriteRenderer>();

        childSlice = go;

        SetRendereData(renderer);
    }

    public void SetRendereData(SpriteRenderer renderer)
    {
        switch (sliceCatagory)
        {
            case SliceCatagory.Shape:
                sliceSymbol = PieceSymbol.General;

                renderer.sprite = SliceManager.instance.limiterSliceSymbolToSprite[sliceSymbol];

                break;
            case SliceCatagory.Color:
                sliceColor = PieceColor.General;

                renderer.sprite = SliceManager.instance.limiterSlicecolorToSprite[sliceColor];

                break;
            default:
                break;
        }
    }
}
