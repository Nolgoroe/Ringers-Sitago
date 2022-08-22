﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AnimationEvents : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AfterClearRing()
    {
        UIManager.instance.RestartCurrentLevel();
    }
    void AfterGoodComplete()
    {
        GameManager.instance.ResetDataStartLevelStartNormal();
    }

    void AfterPlaceTile()
    {
        SortingGroup sorting = GetComponent<SortingGroup>();
        sorting.sortingOrder = 14;
    }
}