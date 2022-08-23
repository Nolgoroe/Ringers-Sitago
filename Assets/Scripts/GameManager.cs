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
    public bool timeCounting;
    public bool gameStarted;
    public bool gameDone;

    [Header("game setup data")]
    public LevelScriptableObject[] allLevels;
    public float timerTime;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        timeCounting = true;

        StartTheGame(false);
    }

    private void Update()
    {
        if (gameStarted)
        {
            timerTime += Time.deltaTime;
        }
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

    public void StartTheGame(bool DoFade)
    {
        timerTime = 0;

        SoundManager.instance.PlayMusic();
        gameStarted = true;

        ResetDataStartLevelStartNormal();

    }

    public void ResetDataStartLevelStartNormal()
    {
        timerTime = 0;

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

        GameObject board =  Instantiate(currentMap.boardPrefab, instantiateUnder);
        ConnectionManager.instance.GrabCellList(board.transform);

        Instantiate(currentMap.clipPrefab, instantiateUnder);

        InitAllSystems();

        SliceManager.instance.SpawnSlices(currentMap.slicesToSpawn.Length);


        if (currentMap.powerupsForMap.Length > 0)
        {
            foreach (PowerUp power in currentMap.powerupsForMap)
            {
                PowerupProperties prop = PowerUpManager.instance.powerupButtons.Where(p => p.powerupType == power).SingleOrDefault();

                if (prop)
                {
                    prop.canBeSelected = true;
                    prop.GetComponent<SpriteChanger>().ChangeToCanUse();
                }
            }
        }
        else
        {
            foreach (PowerupProperties prop in PowerUpManager.instance.powerupButtons)
            {
                if (prop)
                {
                    prop.canBeSelected = false;
                    prop.GetComponent<SpriteChanger>().ChangeToCantUse();

                    //prop.GetComponent<Button>().interactable = false;
                }
            }
        }

        SliceManager.instance.GetComponent<Animator>().SetTrigger("Limiter Appear");
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

        //UIManager.instance.gameplayCanvas.SetActive(true);

        GameObject board = Instantiate(currentMap.boardPrefab, instantiateUnder);
        ConnectionManager.instance.GrabCellList(board.transform);

        Instantiate(currentMap.clipPrefab, instantiateUnder);

        InitAllSystems();

        SliceManager.instance.SpawnSlices(currentMap.slicesToSpawn.Length);

        SliceManager.instance.GetComponent<Animator>().SetTrigger("Limiter Appear");
    }

    private void ResetAllLevelData()
    {
        totalPlacedPieces = 0;
        unsuccessfullConnectionsCount = 0;
        ScoreManager.instance.hasClickedDeal = false;
        PowerUpManager.instance.timesClickedDeal = 0;
    }
}
