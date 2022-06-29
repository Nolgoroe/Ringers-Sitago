using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public float currentScore;

    [Header("Score Add")]
    public float ringCompleteScoreGive;
    public float ringCompleteScoreGiveUnder30;
    public float ringCompleteScoreGiveNoDeal;
    public bool hasClickedDeal;

    [Header("Score Multi")]
    public float ringCompleteScoreGiveMulti;
    public float ringCompleteScoreGiveUnder30Multi;
    public float ringCompleteScoreGiveNoDealMulti;

    private void Awake()
    {
        instance = this;
    }



    public void AddRingCompletionScore()
    {

        int ringIndex = GameManager.instance.currentMap.indexForScore;

        currentScore += ringCompleteScoreGive + (ringIndex * ringCompleteScoreGiveMulti);

        if(GameManager.instance.timerTime > 30)
        {
            currentScore += ringCompleteScoreGiveUnder30 + (ringIndex * ringCompleteScoreGiveUnder30Multi);
        }


        if (!hasClickedDeal)
        {
            currentScore += ringCompleteScoreGiveNoDeal + (ringIndex * ringCompleteScoreGiveNoDealMulti);
        }


        UIManager.instance.scoreText.text = currentScore.ToString();
    }
}
