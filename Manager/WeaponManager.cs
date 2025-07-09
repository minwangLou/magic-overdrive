using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;

    public List<TextAsset> weaponTextAssets;
    public List<WeaponData> weaponDatas = new List<WeaponData>();

    public List<Weapon> weaponsInstantiate = new List<Weapon>(); //arma instanciada en el partido


    [HideInInspector]public Transform _weaponList;

    private void Awake()
    {
        instance = this;
        ReadWeaponJsonDatas();
    }

    private void ReadWeaponJsonDatas()
    {
        for (int i = 0; i <= weaponTextAssets.Count; i++)
        {
            weaponDatas.Add(null);
        }


        foreach (TextAsset weaponTextAsset in weaponTextAssets)
        {
            WeaponData weapon = JsonConvert.DeserializeObject<WeaponData>(weaponTextAsset.text);
            weaponDatas[weapon.id] = weapon; //añadir según id de cada weapon a la posición de la lista de weapon.
        }
    }

    //Instanciar weapon locked en el escenario
    //desbloquear dicha weapon y instanciar su prefab
    public void InstantiateWeapon(int weaponID)
    {
        WeaponData weaponSelect = weaponDatas[weaponID];
        weaponSelect.currentLevel++;


        GameObject weaponPrefab = Resources.Load<GameObject>(weaponSelect.weaponPrefab_location);

        GameObject objectInstantiate = Instantiate(weaponPrefab, _weaponList);

        Weapon weaponInstantiate = objectInstantiate.GetComponent<Weapon>();

        weaponInstantiate.weaponID = weaponID;
        weaponInstantiate.knockBackForce = weaponSelect.knockBackForce;

        weaponsInstantiate.Add(weaponInstantiate);

        List<Attribute> totalWeaponAttribute = AttributeManager.instance.TotalAttributeCalculation(weaponSelect.weaponAttribute[1].currentLevelAttribute);

        weaponInstantiate.SetAttributes(totalWeaponAttribute);

        ObjectUIController.instance.AddWeaponIcon(weaponDatas[weaponID]);

    }


    public void UpdateAllWeaponAttributes()
    {

        foreach (Weapon weapon in weaponsInstantiate)
        {
            UpdateWeaponAttribute(weapon);
        }
    }


    public void UpdateWeaponAttribute(Weapon weapon)
    {
        WeaponData weaponData = weaponDatas[weapon.weaponID];

        List<Attribute> weaponCurrentLevelAttribute = weaponData.weaponAttribute[weaponData.currentLevel].currentLevelAttribute;

        List<Attribute> totalWeaponAttribute = AttributeManager.instance.TotalAttributeCalculation(weaponCurrentLevelAttribute);

        weapon.SetAttributes(totalWeaponAttribute);

    }

    public void SelectWeaponToUpgrate(int weaponID)
    {
        if (weaponDatas[weaponID].currentLevel == 0)
        {
            InstantiateWeapon(weaponID);

            PoolObjectManager.instance.currentNumberWeapon++;
            Debug.Log("weapon: " + PoolObjectManager.instance.currentNumberWeapon);

        }else if (weaponDatas[weaponID].currentLevel > 0 && weaponDatas[weaponID].currentLevel < weaponDatas[weaponID].maxLevel) 
        {
            Weapon weaponInstantiate = null;
            foreach (Weapon weapon in weaponsInstantiate)
            {
                if (weapon.weaponID == weaponID)
                {
                    weaponInstantiate = weapon;
                    break;
                }
            }

            if (weaponInstantiate != null)
            {
                weaponDatas[weaponID].currentLevel++;
                UpdateWeaponAttribute(weaponInstantiate);
            }

        }
    }


}
