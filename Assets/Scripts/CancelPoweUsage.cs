using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CancelPoweUsage : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (PowerUpManager.IsUsingPowerUp)
        {
            PowerUpManager.instance.FinishedUsingPowerup(false, PowerUpManager.instance.currentlyInUse);
        }
    }
}
