using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //Lista de estados que tiene la arma en cada de su nivel
    //public List<WeaponStatus> stats;
    //[HideInInspector] public int weaponLevel;

    //[HideInInspector]public bool statsUpdated;

    public Sprite icon;


    //new attribute
    public int weaponID;

    public float might, coolDown, area, speed, duration;
    public int amount;

    /*
    public void LevelUpWeapon()
    {
        if (weaponLevel < stats.Count - 1)
        {
            weaponLevel++;

            statsUpdated = true;


            //Al alcanzar el m¨¢ximo nivel del arma, lo elimina
            //de la lista de assignedweapon y lo reemplaza al fullylevelledWeapon list
            if (weaponLevel >= stats.Count - 1)
            {
                PlayerController.instance.fullyLevelledWeapons.Add(this);
                PlayerController.instance.assignedWeapons.Remove(this);
            }
        }

    }
    */
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

/*
[System.Serializable]
public class WeaponStatus
{
    public float speed, damage, range, timeBetweenAttacks, duration;
    public int amount;
    public string upgradeText;
}
*/


