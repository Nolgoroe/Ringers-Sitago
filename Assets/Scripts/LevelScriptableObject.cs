using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using UnityEditor;

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
    //public bool isLock;
    //public bool isLoot;
    //public bool isLimiter;

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

    //[ContextMenu("THIS")]
    //public void actionhere()
    //{
    //    string n = name;
    //    n = n.Replace("Level ", "");

    //    int num = Convert.ToInt32(n);

    //    levelNum = num;
    //    levelIndexInZone = num;

    //    Debug.Log(num);
    //}
    //[ContextMenu("THIS Leaderboard")]
    //public void actionhere2()
    //{
    //    numIndexForLeaderBoard = 105 + levelNum;
    //}
}
