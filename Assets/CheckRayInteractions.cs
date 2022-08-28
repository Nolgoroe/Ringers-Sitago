using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckRayInteractions : MonoBehaviour
{
    public LayerMask LayerToHit;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit2D hit2D = Physics2D.GetRayIntersection(ray, Mathf.Infinity, LayerToHit);

        if (hit2D)
        {
            Debug.LogWarning("hit! :" + hit2D.transform.name);
        }


        if(Input.GetMouseButtonUp(0))
        {
            Debug.LogWarning("UP");

            if (hit2D)
            {
                PowerUpManager.instance.ObjectToUsePowerUpOn = hit2D.transform;

                PowerUpManager.HasUsedPowerUp = true;

                Debug.Log("Used power");
                return;
            }
            else
            {
                PowerUpManager.instance.FinishedUsingPowerup(false, PowerUpManager.instance.currentlyInUse);
            }
        }
    }
}
