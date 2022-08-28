using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class PowerupProperties : MonoBehaviour
{
    public PowerUp powerupType;

    public PieceColor transformColor;
    public PieceSymbol transformSymbol;

    public bool canBeSelected = false;

    public UnityEvent interactEvent;

    //public CircleCollider2D powerUpCollider;

    //private void Start()
    //{
    //    if(powerupType != PowerUp.Deal)
    //    {
    //        powerUpCollider = GetComponent<CircleCollider2D>();
    //    }
    //}
    public void SetProperties(PowerUp type)
    {
        powerupType = type;
    }

    private void OnMouseDown()
    {
        //if(powerUpCollider)
        //{
        //    powerUpCollider.enabled = false;
        //}

        interactEvent.Invoke();

        Debug.Log("Shooting event powerup");
    }
}
