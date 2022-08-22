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

    public SpriteRenderer subPieceSpriteRenderer;

    private void Awake()
    {
        subPieceSpriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void SetPiece()
    {
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

    public void SetPieceSpecific(PieceSymbol symbol, PieceColor color)
    {
        int indexcColor = (int)color;
        int indexcSymbol = (int)symbol;

        if (color == PieceColor.Joker)
        {
            indexcColor = Random.Range(0, GameManager.instance.currentMap.levelAvailableColors.Length);
            indexcSymbol = Random.Range(0, GameManager.instance.currentMap.levelAvailableSymbols.Length);
        }
        else
        {
            indexcColor = (int)color;
            indexcSymbol = (int)symbol;
        }

        colorOfPiece = color;
        symbolOfPiece = symbol;


        indexcColor--;
        if(indexcColor < 0) // we do -1 since first is general
        {
            indexcColor = 0;
        }

        indexcSymbol--;
        if(indexcSymbol < 0) // we do -1 since first is general
        {
            indexcSymbol = 0;
        }

        subPieceSpriteRenderer.sprite = ClipManager.instance.colorsToSprites[indexcColor].symbolSprites[indexcSymbol]; 

    }


    public void SetPieceAsJoker()
    {
        //subPieceSpriteRenderer.enabled = false;
        Debug.LogError("Joker piece display!");
    }

    void SetSubPieceDisplay()
    {
        int colorIndex = System.Array.IndexOf(ClipManager.instance.colorsToSprites, ClipManager.instance.colorsToSprites.Where(p => p.matColor == colorOfPiece).SingleOrDefault());
        int symbolIndex = (int)symbolOfPiece - 1; // we do -1 since first index is "GENERAL"

        subPieceSpriteRenderer.sprite = ClipManager.instance.colorsToSprites[colorIndex].symbolSprites[symbolIndex];
    }
}
