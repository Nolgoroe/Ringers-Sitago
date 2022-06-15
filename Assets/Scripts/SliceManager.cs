using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SliceCatagory
{
    Shape,
    Color,
}

public class SliceManager : MonoBehaviour
{
    public static SliceManager instance;

    [Header("Prefabs")]
    public GameObject slicePrefab;

    [Header("Board Parts")]
    public Cell[] boardCells;  
    public Slice[] boardSlices;
    public List<Slice> populatedSlices;

    [Header("Dictionairy datas")]
    public Sprite[] limiterSliceColors;
    public Dictionary<PieceColor, Sprite> limiterSlicecolorToSprite;

    public Sprite[] limiterSliceSymbolsSprites;
    public Dictionary<PieceSymbol, Sprite> limiterSliceSymbolToSprite;

    private List<int> possibleSlotsTemp;
    private void Awake()
    {
        instance = this;
    }

    public void InitLevel()
    {
        limiterSliceSymbolToSprite = new Dictionary<PieceSymbol, Sprite>();
        limiterSlicecolorToSprite = new Dictionary<PieceColor, Sprite>();

        for (int i = 0; i < limiterSliceSymbolsSprites.Length; i++)
        {
            limiterSliceSymbolToSprite.Add((PieceSymbol)i, limiterSliceSymbolsSprites[i]);
        }

        for (int i = 0; i < limiterSliceColors.Length; i++)
        {
            limiterSlicecolorToSprite.Add((PieceColor)i, limiterSliceColors[i]);
        }
    }


    public void SpawnSlices(int numOfSlices)
    {
        if (numOfSlices > 0)
        {
            populatedSlices = new List<Slice>();

            possibleSlotsTemp = new List<int>();

            for (int i = 0; i < boardSlices.Length; i++)
            {
                possibleSlotsTemp.Add(i);
            }

            if (GameManager.instance.currentMap.is12PieceRing)
            {
                //SpawnTwelveRingSlices(numOfSlices);
            }
            else
            {
                SpawnEightRingSlices(numOfSlices);
            }
        }
    }

    public void SpawnEightRingSlices(int numOfSlices)
    {
        if (GameManager.instance.currentMap.RandomSlicePositions)
        {
            int randomPos = Random.Range(0, boardSlices.Length);

            populatedSlices.Add(boardSlices[randomPos].transform.GetComponent<Slice>());

            if (numOfSlices == 2)
            {
                for (int i = 1; i < numOfSlices; i++)
                {
                    randomPos += 4;

                    if (randomPos >= boardSlices.Length)
                    {
                        randomPos -= boardSlices.Length;
                    }

                    populatedSlices.Add(boardSlices[randomPos].transform.GetComponent<Slice>());

                }

            }
            else if (numOfSlices == 3)
            {
                for (int i = 1; i < numOfSlices; i++)
                {
                    randomPos += 3;

                    if (randomPos >= boardSlices.Length)
                    {
                        randomPos -= boardSlices.Length;
                    }

                    populatedSlices.Add(boardSlices[randomPos].transform.GetComponent<Slice>());

                }
            }
            else if (numOfSlices == 4)
            {
                for (int i = 1; i < numOfSlices; i++)
                {
                    randomPos += 2;

                    if (randomPos >= boardSlices.Length)
                    {
                        randomPos -= boardSlices.Length;
                    }
                    populatedSlices.Add(boardSlices[randomPos].transform.GetComponent<Slice>());

                }
            }
            else if (numOfSlices > 4)
            {
                possibleSlotsTemp.Remove(randomPos);

                for (int i = 0; i < 3; i++)
                {
                    randomPos += 2;

                    if (randomPos >= boardSlices.Length)
                    {
                        randomPos -= boardSlices.Length;
                    }

                    populatedSlices.Add(boardSlices[randomPos].transform.GetComponent<Slice>());

                    possibleSlotsTemp.Remove(randomPos);
                }

                int slicesLeft = numOfSlices - populatedSlices.Count;

                for (int i = 0; i < slicesLeft; i++)
                {
                    randomPos = Random.Range(0, possibleSlotsTemp.Count);

                    populatedSlices.Add(boardSlices[possibleSlotsTemp[randomPos]].transform.GetComponent<Slice>());

                    possibleSlotsTemp.Remove(possibleSlotsTemp[randomPos]);
                }
            }

            for (int i = 0; i < populatedSlices.Count; i++)
            {
                populatedSlices[i].SetSliceData(populatedSlices[i].transform, GameManager.instance.currentMap.slicesToSpawn[i].sliceToSpawn);
            }
        }
        else
        {

            for (int i = 0; i < GameManager.instance.currentMap.slicesToSpawn.Length; i++)
            {
                populatedSlices.Add(boardSlices[GameManager.instance.currentMap.specificSliceSpots[i]].transform.GetComponent<Slice>());
            }


            for (int i = 0; i < populatedSlices.Count; i++)
            {
                populatedSlices[i].SetSliceData(boardSlices[GameManager.instance.currentMap.specificSliceSpots[i]].transform, GameManager.instance.currentMap.slicesToSpawn[i].sliceToSpawn);
            }
        }
    }



    public void SpawnTwelveRingSlices(int numOfSlices)
    {
        if (GameManager.instance.currentMap.RandomSlicePositions)
        {
            int randomPos = Random.Range(0, boardSlices.Length);

            populatedSlices.Add(boardSlices[randomPos].transform.GetComponent<Slice>());

            if (numOfSlices == 2)
            {
                for (int i = 1; i < numOfSlices; i++)
                {
                    randomPos += 6;

                    if (randomPos >= boardSlices.Length)
                    {
                        randomPos -= boardSlices.Length;
                    }

                    populatedSlices.Add(boardSlices[randomPos].transform.GetComponent<Slice>());

                }

            }
            else if (numOfSlices == 3)
            {
                for (int i = 1; i < numOfSlices; i++)
                {
                    randomPos += 4;

                    if (randomPos >= boardSlices.Length)
                    {
                        randomPos -= boardSlices.Length;
                    }

                    populatedSlices.Add(boardSlices[randomPos].transform.GetComponent<Slice>());

                }
            }
            else if (numOfSlices == 4)
            {
                for (int i = 1; i < numOfSlices; i++)
                {
                    randomPos += 3;

                    if (randomPos >= boardSlices.Length)
                    {
                        randomPos -= boardSlices.Length;
                    }
                    populatedSlices.Add(boardSlices[randomPos].transform.GetComponent<Slice>());

                }
            }
            else if (numOfSlices == 5)
            {
                possibleSlotsTemp.Remove(randomPos);

                for (int i = 0; i < 2; i++) // 3 spaces
                {
                    randomPos += 3;

                    if (randomPos >= boardSlices.Length)
                    {
                        randomPos -= boardSlices.Length;
                    }

                    populatedSlices.Add(boardSlices[randomPos].transform.GetComponent<Slice>());

                    possibleSlotsTemp.Remove(randomPos);
                }

                for (int i = 0; i < 2; i++) // 2 spaces
                {
                    randomPos += 2;

                    if (randomPos >= boardSlices.Length)
                    {
                        randomPos -= boardSlices.Length;
                    }

                    populatedSlices.Add(boardSlices[randomPos].transform.GetComponent<Slice>());

                    possibleSlotsTemp.Remove(randomPos);
                }
            }
            else if (numOfSlices > 5)
            {
                possibleSlotsTemp.Remove(randomPos);

                for (int i = 0; i < 5; i++)
                {
                    randomPos += 2;

                    if (randomPos >= boardSlices.Length)
                    {
                        randomPos -= boardSlices.Length;
                    }

                    populatedSlices.Add(boardSlices[randomPos].transform.GetComponent<Slice>());

                    possibleSlotsTemp.Remove(randomPos);
                }

                int slicesLeft = numOfSlices - populatedSlices.Count;

                for (int i = 0; i < slicesLeft; i++)
                {
                    randomPos = Random.Range(0, possibleSlotsTemp.Count);

                    populatedSlices.Add(boardSlices[possibleSlotsTemp[randomPos]].transform.GetComponent<Slice>());

                    possibleSlotsTemp.Remove(possibleSlotsTemp[randomPos]);
                }
            }

            for (int i = 0; i < populatedSlices.Count; i++)
            {
                populatedSlices[i].SetSliceData(populatedSlices[i].transform, GameManager.instance.currentMap.slicesToSpawn[i].sliceToSpawn);
            }
        }
        else
        {

            for (int i = 0; i < GameManager.instance.currentMap.slicesToSpawn.Length; i++)
            {
                populatedSlices.Add(boardSlices[GameManager.instance.currentMap.specificSliceSpots[i]].transform.GetComponent<Slice>());
            }


            for (int i = 0; i < populatedSlices.Count; i++)
            {
                populatedSlices[i].SetSliceData(boardSlices[GameManager.instance.currentMap.specificSliceSpots[i]].transform, GameManager.instance.currentMap.slicesToSpawn[i].sliceToSpawn);
            }
        }
    }
}
