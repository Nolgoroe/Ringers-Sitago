using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupData : MonoBehaviour
{
    public string name;
    //public slotType slot;
    public PowerUp power;
    public PieceSymbol specificSymbol;
    public PieceColor specificColor;
    public int numOfUses;
    public int scopeOfUses;
    public int timeForCooldown;
    public string mats;
    public string spritePath;
    public string nextTimeAvailable /*= DateTime.Now.AddMinutes(15).ToString()*/;

    public string Description = "This a temporaty description for all potions";

    public bool isTutorialPower = false;

    public PowerupData(string equipName, PowerUp equippower, PieceSymbol equipSpecificSymbol, PieceColor equipSpecificColor, int equipnumUses,
                        int equipScopeUses, int equipTimeCooldown, string equipNextTimeAvailable, string equipDescription, bool equipIsTutorialPower, string equipmatList, string path)
    {
        name = equipName;

        //slot = (slotType)equiptype;
        power = equippower;
        specificSymbol = equipSpecificSymbol;
        specificColor = equipSpecificColor;

        numOfUses = equipnumUses;
        scopeOfUses = equipScopeUses;
        timeForCooldown = equipTimeCooldown;

        nextTimeAvailable = equipNextTimeAvailable;
        Description = equipDescription;

        isTutorialPower = equipIsTutorialPower;

        mats = equipmatList;
        spritePath = path;
    }
    public PowerupData() //// Override for an empty constructor
    {

    }
}
