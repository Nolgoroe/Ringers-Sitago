using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class stonePieceDataStruct
{
    public int cellIndex;
    public bool randomValues;
    public bool isNeutral;

    [Header("ONLY IF NOT NEUTRAL")]
    public PieceColor colorOfPieceRight;
    public PieceSymbol symbolOfPieceRight;
    public PieceColor colorOfPieceLeft;
    public PieceSymbol symbolOfPieceLeft;
}

[System.Serializable]
public class sliceToSpawnDataStruct
{
    public SliceCatagory sliceToSpawn;
}

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Create Level")]
public class LevelScriptableObject : ScriptableObject
{
    public bool is12PieceRing;

    public GameObject boardPrefab;
    public GameObject clipPrefab;

    public int cellsCountInLevel;

    public bool RandomSlicePositions;
    public bool allowRepeatSlices;

    public PowerUp[] powerupsForMap;

    public stonePieceDataStruct[] stoneTiles;
    public sliceToSpawnDataStruct[] slicesToSpawn;

    public PieceColor[] levelAvailableColors;
    public PieceSymbol[] levelAvailableSymbols;

    public int[] specificSliceSpots;
}
