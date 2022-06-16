using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SubPiece : MonoBehaviour
{
    public PieceSymbol symbolOfPiece;
    public PieceColor colorOfPiece;
    public bool isGoodConnected;

    int randomColor;
    int randomSymbol;

    public Image subPieceImage;
    public void SetPiece()
    {
        //if (symbolOfPiece == PieceSymbol.Joker)
        //{
        //    SetPieceAsNormal();
        //}

        if (GameManager.instance.currentMap.levelAvailableColors.Length > 0 || GameManager.instance.currentMap.levelAvailableSymbols.Length > 0)
        {

            if (GameManager.instance.currentMap.levelAvailableColors.Length > 0)
            {
                randomColor = Random.Range(0, GameManager.instance.currentMap.levelAvailableColors.Length);
                colorOfPiece = GameManager.instance.currentMap.levelAvailableColors[randomColor];
            }

            if (GameManager.instance.currentMap.levelAvailableSymbols.Length > 0)
            {
                randomSymbol = Random.Range(0, GameManager.instance.currentMap.levelAvailableSymbols.Length);
                symbolOfPiece = GameManager.instance.currentMap.levelAvailableSymbols[randomSymbol];
            }
        }
        else
        {
            randomColor = Random.Range(0, GameManager.instance.currentMap.levelAvailableColors.Length);
            colorOfPiece = (PieceColor)randomColor;

            randomSymbol = Random.Range(0, GameManager.instance.currentMap.levelAvailableSymbols.Length);
            symbolOfPiece = (PieceSymbol)randomSymbol;
        }


        SetSubPieceDisplay();
    }



    public void SetPieceAsJoker()
    {
        subPieceImage.sprite = ClipManager.instance.jokerSprite;

        Debug.LogError("Joker piece display!");
    }

    //public void SetPieceAsNormal()
    //{
    //    Debug.LogError("Normal piece display!");
    //}

    void SetSubPieceDisplay()
    {
        int colorIndex = System.Array.IndexOf(ClipManager.instance.colorsToSprites, ClipManager.instance.colorsToSprites.Where(p => p.matColor == colorOfPiece).SingleOrDefault());
        int symbolIndex = (int)symbolOfPiece - 1; // we do -1 since first index is "GENERAL"

        subPieceImage.sprite = ClipManager.instance.colorsToSprites[colorIndex].symbolSprites[symbolIndex];
    }
}
