using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public enum PowerUp
{
    Joker,

    //Switch,
    //PieceBomb,
    //SliceBomb,
    None
}


public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager instance;

    public static bool IsUsingPowerUp;
    public static bool HasUsedPowerUp;


    [Header("Deal Related")]
    public Button dealButton;
    public float dealCooldown;
    public Vector3[] piecesDealPositionsOut;
    public float delayClipMove;
    public float timeToAnimateMove;
    public float WaitTimeBeforeIn;

    [Header("Power up")]
    public PowerupProperties currentlyInUse;
    public Piece ObjectToUsePowerUpOn;

    [Header("Lists and arrays")]
    public List<PowerupProperties> powerupButtons;

    private void Awake()
    {
        instance = this;
    }


    public IEnumerator DealCooldown(float time)
    {
        dealButton.interactable = false;

        Image dealButtonImage = dealButton.GetComponent<Image>();
        dealButtonImage.fillAmount = 0;

        LeanTween.value(dealButtonImage.gameObject, dealButtonImage.fillAmount, 1, time).setOnComplete(() => dealButton.interactable = true).setOnUpdate((float val) =>
        {
            dealButtonImage.fillAmount = val;
        });

        for (int i = 0; i < ClipManager.instance.slots.Length; i++)
        {
            RectTransform toMove = ClipManager.instance.slots[i].transform.GetChild(0).GetComponent<RectTransform>();

            LeanTween.move(toMove, piecesDealPositionsOut[i], timeToAnimateMove).setEase(LeanTweenType.easeInOutQuad); // animate

            yield return new WaitForSeconds(delayClipMove);
        }


        yield return new WaitForSeconds(WaitTimeBeforeIn);
        ClipManager.instance.DealAnimClipLogic();

        for (int i = ClipManager.instance.slots.Length - 1; i > -1; i--)
        {
            RectTransform toMove = ClipManager.instance.slots[i].transform.GetChild(0).GetComponent<RectTransform>();

            
            LeanTween.move(toMove, Vector3.zero, timeToAnimateMove).setEase(LeanTweenType.easeInOutQuad); // animate

            //Invoke("playReturnPiecePlaceSound", ClipManager.instance.timeToAnimateMove - 0.25f);

            yield return new WaitForSeconds(delayClipMove);

        }
    }

    public void Deal()
    {
        if (!GameManager.instance.gameDone)
        {
            ScoreManager.instance.hasClickedDeal = true;

            StartCoroutine(DealCooldown(dealCooldown));
        }
    }


    private void Start()
    {
        for (int i = 0; i < powerupButtons.Count; i++)
        {
            AssignPowerUp((PowerUp)i, powerupButtons[i]);
        }
    }

    public void AssignPowerUp(PowerUp ThePower, PowerupProperties theButton)
    {
        theButton.interactEvent.AddListener(() => UsingPowerup(theButton));

        PowerupProperties prop = theButton.gameObject.GetComponent<PowerupProperties>();
        switch (ThePower)
        {
            case PowerUp.Joker:
                theButton.interactEvent.AddListener(() => CallJokerCoroutine(prop));
                break;
            default:
                break;
        }
    }

    public void UsingPowerup(PowerupProperties prop)
    {
        if (prop.numOfUses > 0 && prop.canBeSelected)
        {
            currentlyInUse = prop.gameObject.GetComponent<PowerupProperties>();

            prop.canBeSelected = false;

            IsUsingPowerUp = true;
        }
    }
    public void CallJokerCoroutine(PowerupProperties prop)
    {
        if(prop.numOfUses > 0)
        {
            StartCoroutine(JokerPower(prop));
        }
    }

    public IEnumerator JokerPower(PowerupProperties prop)
    {
        yield return new WaitUntil(() => HasUsedPowerUp == true);

        bool successfulUse = false;

        if (ObjectToUsePowerUpOn.leftChild.symbolOfPiece != PieceSymbol.Joker) ///// If 1 of the sub pieces is a joker - so is the other. If the symbol is a joker then the color is awell
        {

            ObjectToUsePowerUpOn.leftChild.symbolOfPiece = PieceSymbol.Joker;
            ObjectToUsePowerUpOn.leftChild.colorOfPiece = PieceColor.Joker;

            ObjectToUsePowerUpOn.rightChild.symbolOfPiece = PieceSymbol.Joker;
            ObjectToUsePowerUpOn.rightChild.colorOfPiece = PieceColor.Joker;

            ObjectToUsePowerUpOn.leftChild.SetPieceAsJoker();
            ObjectToUsePowerUpOn.rightChild.SetPieceAsJoker();

            ObjectToUsePowerUpOn.SetPieceAsJoker();
            successfulUse = true;


            FinishedUsingPowerup(successfulUse, prop);

            Debug.Log("Joker");
        }
        else
        {
            FinishedUsingPowerup(false, prop);
        }
    }

    public void FinishedUsingPowerup(bool successfull, PowerupProperties prop)
    {
        StopAllCoroutines();

        ObjectToUsePowerUpOn = null;

        IsUsingPowerUp = false;
        currentlyInUse = null;
        HasUsedPowerUp = false;

        if (successfull)
        {
            prop.numOfUses--;
        }

        if(prop.numOfUses <= 0)
        {
            prop.GetComponent<Button>().interactable = false;
        }

        ReactivatePowerButtons();
    }

    public void ReactivatePowerButtons()
    {
        Debug.LogError("reactivating");

        foreach (PowerupProperties but in powerupButtons)
        {
            but.canBeSelected = true;
        }
    }
}
