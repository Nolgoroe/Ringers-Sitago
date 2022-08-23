using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System;
using System.Text.RegularExpressions;

public enum BottomUIToShow
{
    None,
    OnlyDeal,
    All
}

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    //[Header("Fade into Level")]
    //public GameObject fadeIntoLevelScreen;
    //public float fadeIntoLevelSpeed;
    //public float fadeIntoLevelDelay;

    [Header("Gameplay")]
    //public GameObject gameplayCanvas;
    public GameObject destroyAllParent;
    //public TMP_Text timerText;
    //public TMP_Text scoreText;

    [Header("System Messages")]
    public GameObject systemMessages;
    public TMP_Text headerText;
    public float speedFadeOutHeadText;

    [Header("System Data")]
    public TMP_Text gameVersionText;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        systemMessages.SetActive(false);

        //gameVersionText.text = Application.version;
    }

    public void RestartCurrentLevel()
    {
        if (!GameManager.instance.gameDone)
        {
            GameManager.instance.ResetCurrentLevel();
        }
    }

    public void HeaderFadeInText(string toSay, float fadeSpeed)
    {
        LeanTween.cancel(headerText.gameObject);

        systemMessages.SetActive(true);

        headerText.color = new Color(headerText.color.r, headerText.color.g, headerText.color.b, 1);

        headerText.text = toSay;

        LeanTween.value(headerText.gameObject, 1, 0, fadeSpeed).setOnComplete(() => systemMessages.SetActive(false)).setOnUpdate((float val) =>
        {
            TMP_Text sr = headerText.GetComponent<TMP_Text>();
            Color newColor = sr.color;
            newColor.a = val;
            sr.color = newColor;
        });
    }
    //public void HeaderAppearText(string toSay)
    //{
    //    LeanTween.cancel(headerText.gameObject);

    //    systemMessages.SetActive(true);
    //    headerText.gameObject.SetActive(true);

    //    headerText.color = new Color(headerText.color.r, headerText.color.g, headerText.color.b, 1);

    //    headerText.text = toSay;
    //}

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
