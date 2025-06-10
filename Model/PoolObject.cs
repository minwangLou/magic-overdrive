using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoolObject
{
    public int id;
    public string name;
    public int idObject;
    public int rarityWeight;
    public BonusData bonus;
    public WeaponData weapon;

    public PoolObject(int id, string name, int idObject, int rarityWeight, BonusData bonus)
    {
        this.id = id;
        this.name = name;
        this.idObject = idObject;
        this.rarityWeight = Mathf.RoundToInt(rarityWeight * 0.35f);
        this.bonus = bonus;
        
    }

    public PoolObject(int id, string name, int idObject, int rarityWeight, WeaponData weapon)
    {
        this.id = id;
        this.name = name;
        this.idObject = idObject;
        this.rarityWeight = Mathf.RoundToInt(rarityWeight * 0.65f);
        this.weapon = weapon;

    }

}
