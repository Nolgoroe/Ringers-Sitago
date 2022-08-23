using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public enum PowerUp
{
    Joker,
    Deal,
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
    public PowerupProperties dealButton;
    public SpriteRenderer dealButtonDark;
    public float dealCooldown;
    public Vector3[] piecesDealPositionsOut;
    public float delayClipMove;
    public float timeToAnimateMove;
    public float WaitTimeBeforeIn;
    public float timesClickedDeal;

    [Header("Power up")]
    public PowerupProperties currentlyInUse;
    public Piece ObjectToUsePowerUpOn;
    public Transform potionFollowObject;

    //public SpriteChanger potionOriginal;

    [Header("Lists and arrays")]
    public List<PowerupProperties> powerupButtons;


    Dictionary<PowerUp, PowerupProperties> PowerupTypeToProperties;

    private void Awake()
    {
        instance = this;
        PowerupTypeToProperties = new Dictionary<PowerUp, PowerupProperties>();
    }

    private void Start()
    {
        dealButtonDark.gameObject.SetActive(false);
        potionFollowObject.gameObject.SetActive(false);

        foreach (var powerup in powerupButtons)
        {
            PowerupTypeToProperties.Add(powerup.powerupType, powerup);
        }

        for (int i = 0; i < System.Enum.GetNames(typeof(PowerUp)).Length; i++)
        {
            PowerupProperties props = null;
            if (PowerupTypeToProperties.TryGetValue((PowerUp)i, out props))
            {
                props = PowerupTypeToProperties[(PowerUp)i];
            }


            if (props)
            {
                AssignPowerUp(props.powerupType, props);
            }
        }
    }

    private void Update()
    {
        if (IsUsingPowerUp)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10;

            Vector3 pos = Camera.main.ScreenToWorldPoint(mousePosition);

            potionFollowObject.position = pos;
        }
    }

    public IEnumerator DealCooldown(float time)
    {
        dealButton.canBeSelected = false;

        float startDeal = dealButtonDark.material.GetFloat("_Arc1");
        float endDeal = 360;

        dealButtonDark.gameObject.SetActive(true);

        LeanTween.value(dealButtonDark.gameObject, startDeal, endDeal, time).setOnComplete(() => AfterDarkDeal()).setOnUpdate((float val) =>
        {
            dealButtonDark.material.SetFloat("_Arc1", val);
        });

        for (int i = 0; i < ClipManager.instance.slots.Length; i++)
        {
            GameObject toMove = ClipManager.instance.slots[i].transform.GetChild(0).gameObject;

            LeanTween.moveLocal(toMove, piecesDealPositionsOut[i], timeToAnimateMove).setEase(LeanTweenType.easeInOutQuad); // animate

            yield return new WaitForSeconds(delayClipMove);
        }


        yield return new WaitForSeconds(WaitTimeBeforeIn);
        ClipManager.instance.DealAnimClipLogic();


        if (GameManager.instance.totalPlacedPieces == 7)
        {
            ConnectionManager.instance.StartLastClipAlgoritm();
            yield return new WaitUntil(() => ConnectionManager.instance.hasFinishedAlgorithm == true);

            int randomNum = UnityEngine.Random.Range(0, 4);
            Debug.LogError(randomNum);

            if (ConnectionManager.instance.decidedAlgoritmPath != null)
            {
                ClipManager.instance.RefreshSpecificSlot(randomNum, ConnectionManager.instance.decidedAlgoritmPath);
            }
        }




        for (int i = ClipManager.instance.slots.Length - 1; i > -1; i--)
        {
            GameObject toMove = ClipManager.instance.slots[i].transform.GetChild(0).gameObject;


            LeanTween.move(toMove, Vector3.zero, timeToAnimateMove).setEase(LeanTweenType.easeInOutQuad).setMoveLocal(); // animate

            //Invoke("playReturnPiecePlaceSound", ClipManager.instance.timeToAnimateMove - 0.25f);

            yield return new WaitForSeconds(delayClipMove);

        }

    }

    public void Deal()
    {
        if (!dealButton.canBeSelected)
        {
            dealButton.GetComponent<Animator>().SetTrigger("Not Ready");
            return;
        }

        if (!GameManager.instance.gameDone)
        {
            ScoreManager.instance.hasClickedDeal = true;
            timesClickedDeal++;

            StartCoroutine(DealCooldown(dealCooldown));
            StartCoroutine(ToggleBoolAfterSeconds(dealButton, dealCooldown, true));
        }
    }

    IEnumerator ToggleBoolAfterSeconds(PowerupProperties dealButton, float seconds, bool isTrue)
    {
        yield return new WaitForSeconds(seconds);
        dealButton.canBeSelected = isTrue;
    }

    void AfterDarkDeal()
    {
        dealButtonDark.material.SetFloat("_Arc1", 0);
        dealButtonDark.gameObject.SetActive(false);

        dealButton.GetComponent<Animator>().SetTrigger("Ready");
    }
    public void AssignPowerUp(PowerUp ThePower, PowerupProperties theButton)
    {
        PowerupProperties prop = theButton.gameObject.GetComponent<PowerupProperties>();

        switch (ThePower)
        {
            case PowerUp.Joker:
                theButton.interactEvent.AddListener(() => CallJokerCoroutine(prop));
                theButton.interactEvent.AddListener(() => UsingPowerup(theButton));
                break;
            default:
                break;

            case PowerUp.Deal:
                theButton.interactEvent.AddListener(() => Deal());
                break;
        }
    }

    public void UsingPowerup(PowerupProperties prop)
    {
        if (prop.canBeSelected)
        {
            currentlyInUse = prop.gameObject.GetComponent<PowerupProperties>();

            // we set the setactibe to true and then false in order to reset the animation to start
            // where scale is 1 and alpha is 1
            potionFollowObject.gameObject.SetActive(false);
            potionFollowObject.gameObject.SetActive(true);

            potionFollowObject.GetComponent<Animator>().SetBool("Blink", true);
            prop.canBeSelected = false;

            IsUsingPowerUp = true;
        }
    }
    public void CallJokerCoroutine(PowerupProperties prop)
    {
        if(prop.canBeSelected)
        {
            potionFollowObject.gameObject.SetActive(true);
            potionFollowObject.localScale = Vector3.one;
            prop.transform.GetComponent<SpriteChanger>().ChangeToCantUse();

            StartCoroutine(JokerPower(prop));
        }
        else
        {
            prop.GetComponent<Animator>().SetTrigger("Not Ready");
        }
    }

    public IEnumerator JokerPower(PowerupProperties prop)
    {
        yield return new WaitUntil(() => HasUsedPowerUp == true);

        bool successfulUse = false;

        if (ObjectToUsePowerUpOn.leftChild.symbolOfPiece != PieceSymbol.Joker) ///// If 1 of the sub pieces is a joker - so is the other. If the symbol is a joker then the color is awell
        {
            ObjectToUsePowerUpOn.GetComponent<Animator>().SetTrigger("Joker Transform");

            ObjectToUsePowerUpOn.leftChild.symbolOfPiece = PieceSymbol.Joker;
            ObjectToUsePowerUpOn.leftChild.colorOfPiece = PieceColor.Joker;

            ObjectToUsePowerUpOn.rightChild.symbolOfPiece = PieceSymbol.Joker;
            ObjectToUsePowerUpOn.rightChild.colorOfPiece = PieceColor.Joker;

            ObjectToUsePowerUpOn.SetPieceAsJoker();
            ObjectToUsePowerUpOn.leftChild.SetPieceAsJoker();
            ObjectToUsePowerUpOn.rightChild.SetPieceAsJoker();

            successfulUse = true;

            if (ObjectToUsePowerUpOn.partOfBoard)
            {
                Cell c = ObjectToUsePowerUpOn.transform.parent.GetComponent<Cell>();
                int cellIndex = System.Array.IndexOf(SliceManager.instance.boardCells, c);

                ConnectionManager.instance.CheckConnectionsOnPickup(c, cellIndex);

                ConnectionManager.instance.CheckConnection(c, cellIndex);
            }

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
        HasUsedPowerUp = false;

        if (successfull)
        {
            potionFollowObject.GetComponent<Animator>().SetTrigger("Deselect");
            potionFollowObject.GetComponent<Animator>().SetBool("Blink", false);
            prop.canBeSelected = false;
            prop.GetComponent<SpriteChanger>().ChangeToCantUse();

            currentlyInUse = null;
        }
        else
        {
            potionFollowObject.GetComponent<Animator>().SetTrigger("Deselect");
            potionFollowObject.GetComponent<Animator>().SetBool("Blink", false);

            prop.canBeSelected = true;
            prop.GetComponent<SpriteChanger>().ChangeToCanUse();

        }


        //ReactivatePowerButtons();
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
