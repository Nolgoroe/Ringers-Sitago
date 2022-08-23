using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    public GameObject canUseObject;
    public GameObject cantUseObject;

    public void ChangeToCanUse()
    {
        cantUseObject.SetActive(false);
        canUseObject.SetActive(true);
    }

    public void ChangeToCantUse()
    {
        cantUseObject.SetActive(true);
        canUseObject.SetActive(false);

        Debug.LogError("Changed");
    }
}
