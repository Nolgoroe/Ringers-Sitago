using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class PowerupProperties : MonoBehaviour, IPointerClickHandler
{
    public PowerUp powerupType;

    public int numOfUses;

    public PieceColor transformColor;
    public PieceSymbol transformSymbol;

    public bool canBeSelected = false;

    public UnityEvent interactEvent;

    public void SetProperties(PowerUp type)
    {
        powerupType = type;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (numOfUses > 0 && canBeSelected)
        {
            interactEvent.Invoke();
        }
        //Debug.Log("Shooting event powerup");
    }
}
