using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusData
{
    public int id;
    public string bonusName;
    public string bonusIcon;
    public int outGameMaxLevel;
    public int outGameCurrentLevel;
    public int cost;
    public string bonusOutGameDescription;
    public string bonusInGameDescription;
    public int unlock;
    public float valuePerLevelInGame;
    public float valuePerLevelOutGame;

    public int idAttribute;
    public int inGameCurrentLevel;
    public int inGameMaxLevel;
    public AttributeType attributeType;
    public BonusType bonusType;
    public float totalValue;
    public int inGameUnlock;

    public float roleBonusValue;//Bonus del obtenido de role seleccionado
    public float itemBonusValue; //funcionamiento Para futuro, incrementar o disminuir valor de bonus obtenido de item

    public int rarityWeight;


    public void Info()
    {
        Debug.Log("BonusDatas:" +
                "\nidAttribute:" + idAttribute +
                "\nbonusName: "+ bonusName + 
                "\ntotalValue: " + totalValue + 
                "\nroleBonusValue: " + roleBonusValue);
    }


}
