using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [HideInInspector]
    public int weaponID;

     public float might, coolDown, area, speed, duration;
     public int amount;

    public float knowBackForce;


    public void SetAttributes(List<Attribute> attibutes)
    {
        might = attibutes[1].value;
        coolDown = attibutes[5].value; 
        area = attibutes[6].value;
        speed = attibutes[7].value;
        duration = attibutes[8].value;
        amount = (int)attibutes[9].value;

        UpdateWeaponStats();
    }

    public virtual void UpdateWeaponStats()
    {

    }


}


