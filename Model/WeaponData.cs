using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData
{
    public int id;
    public string name;
    public string weaponIcon_location;
    public int rarityWeight;
    public List<WeaponLevelAttribute> weaponAttribute;
    public int currentLevel;
    public int maxLevel; //0, modify by code
    public string weaponPrefab_location;
    public int unloke; //Initial unlock situation

}
