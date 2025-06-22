using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [HideInInspector]
    public int weaponID;

     public float damage, coolDown, area, speed, duration;
     public int amount;

    public float knockBackForce;


    public void SetAttributes(List<Attribute> attibutes)
    {/*
        might = attibutes[1].value;
        coolDown = attibutes[2].value; 
        area = attibutes[3].value;
        speed = attibutes[4].value;
        duration = attibutes[5].value;
        amount = (int)attibutes[6].value;
        */
        foreach (var attr in attibutes)
        {
            if (attr != null)
            {
                switch (attr.name)
                {
                    case "Might":
                        damage = attr.value;
                        break;
                    case "Cooldown":
                        coolDown = attr.value;
                        break;
                    case "Area":
                        area = attr.value;
                        break;
                    case "Speed":
                        speed = attr.value;
                        break;
                    case "Duration":
                        duration = attr.value;
                        break;
                    case "Amount":
                        amount = (int)attr.value;
                        break;
                    default:

                        break;
                } 
            }
        }

        UpdateWeaponStats();
    }

    public virtual void UpdateWeaponStats()
    {

    }


}


