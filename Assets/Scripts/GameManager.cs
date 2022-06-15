using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("In game Data")]
    public LevelScriptableObject currentMap;
    public Transform instantiateUnder;
    public int totalPlacedPieces;
    public int unsuccessfullConnectionsCount;
    public int currentMapIndex;

    [Header("game setup data")]
    public LevelScriptableObject[] allLevels;

    private void Awake()
    {
        instance = this;
    }

    public bool CheckEndLevel()
    {
        Debug.LogError("END LEVEL HERE");

        if (unsuccessfullConnectionsCount > 0)
        {
            return false;
        }

        return true;
    }

    public void StartLevel(bool DoFade)
    {
        UIManager.instance.mainMenu.SetActive(false);

        if (DoFade)
        {
            StartCoroutine(UIManager.instance.FadeIntoLevelAction());
        }
        else
        {
            ResetDataStartLevelStartNormal();
        }

    }

    public void ResetDataStartLevelStartNormal()
    {
        for (int i = 0; i < instantiateUnder.childCount; i++)
        {
            Destroy(instantiateUnder.GetChild(i).gameObject);
        }

        ResetAllLevelData();

        if (currentMapIndex > allLevels.Length - 1)
        {
            currentMap = allLevels[allLevels.Length - 1];
        }
        else
        {
            currentMap = allLevels[currentMapIndex];
        }

        UIManager.instance.gameplayCanvas.SetActive(true);

        Instantiate(currentMap.boardPrefab, instantiateUnder);

        Instantiate(currentMap.clipPrefab, instantiateUnder);

        InitAllSystems();

        SliceManager.instance.SpawnSlices(currentMap.slicesToSpawn.Length);

        if(currentMap.powerupsForMap.Length > 0)
        {
            foreach (PowerUp power in currentMap.powerupsForMap)
            {
                PowerupProperties prop = PowerUpManager.instance.powerupButtons.Where(p => p.powerupType == power).SingleOrDefault();

                if (prop)
                {
                    prop.numOfUses += 3;
                    prop.canBeSelected = true;
                }
            }
        }
    }

    public void InitAllSystems()
    {
        ClipManager.instance.InitLevel();
        SliceManager.instance.InitLevel();
        GameplayController.instance.InitLeve();
    }

    public void ResetCurrentLevel()
    {
        ResetDataStartLevelMidGame();
    }

    private void ResetDataStartLevelMidGame()
    {
        for (int i = 0; i < instantiateUnder.childCount; i++)
        {
            Destroy(instantiateUnder.GetChild(i).gameObject);
        }

        ResetAllLevelData();

        if (currentMapIndex > allLevels.Length - 1)
        {
            currentMap = allLevels[allLevels.Length - 1];
        }
        else
        {
            currentMap = allLevels[currentMapIndex];
        }

        UIManager.instance.gameplayCanvas.SetActive(true);

        Instantiate(currentMap.boardPrefab, instantiateUnder);

        Instantiate(currentMap.clipPrefab, instantiateUnder);

        InitAllSystems();

        SliceManager.instance.SpawnSlices(currentMap.slicesToSpawn.Length);
    }

    private void ResetAllLevelData()
    {
        totalPlacedPieces = 0;
        unsuccessfullConnectionsCount = 0;
    }
}
