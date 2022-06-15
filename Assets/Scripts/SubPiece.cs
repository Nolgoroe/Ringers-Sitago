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

    public void SetPiece()
    {
        if (symbolOfPiece == PieceSymbol.Joker)
        {
            SetPieceAsNormal();
        }

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
    }

    public void DisableRaycast()
    {
        transform.GetComponent<Image>().raycastTarget = false;
    }
    public void EnableRaycast()
    {
        transform.GetComponent<Image>().raycastTarget = true;
    }


    public void SetPieceAsJoker()
    {
        Debug.LogError("Joker piece display!");
    }

    public void SetPieceAsNormal()
    {
        Debug.LogError("Normal piece display!");
    }
}
