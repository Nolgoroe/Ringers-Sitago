using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CancelPoweUsage : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    GraphicRaycaster m_Raycaster;
    EventSystem m_EventSystem;
    public Canvas refCanvas;
    private void Start()
    {
        m_Raycaster = refCanvas.GetComponent<GraphicRaycaster>();
        m_EventSystem = GetComponent<EventSystem>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);

        RaycastHit2D hit2D = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
        //Debug.LogWarning(hit2D.transform.name);


        if (PowerUpManager.IsUsingPowerUp && hit2D.transform.name == transform.name)
        {
            Debug.LogError("Deactivated power");

            PowerUpManager.instance.FinishedUsingPowerup(false, PowerUpManager.instance.currentlyInUse);
        }
    }
}
