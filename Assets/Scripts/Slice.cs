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
    }
}
