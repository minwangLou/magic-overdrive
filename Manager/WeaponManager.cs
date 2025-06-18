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

    private AttributeManager attributeManager;

    public Transform _weaponList;

    private void Awake()
    {
        instance = this;
        ReadWeaponJsonDatas();
    }

    private void Start()
    {
        attributeManager = AttributeManager.instance;
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

        weaponsInstantiate.Add(weaponInstantiate);

        List<Attribute> totalWeaponAttribute = AttributeManager.instance.TotalAttributeCalculation(weaponSelect.weaponAttribute[1].currentLevelAttribute);

        weaponInstantiate.SetAttributes(totalWeaponAttribute);

    }


    public void UpdateAllWeaponAttributes()
    {
        //attributeManager.TotalBonusValueCalculation();

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

            ObjectUIManager.instance.AddWeaponIcon(weaponDatas[weaponID]);
            PoolObjectManager.instance.currentNumberWeapon++;

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
        /*

    //Se ejecuta cuando tiene en level up, cogió el objeto de tipo Bonus
    public void UpdateAllWeaponAttributes()
    {
        attributeManager.TotalBonusValueCalculation();

        for (int i = 1; i < weaponDatas.Count; i++) //empieza apartir index 1 de la lista, index 0 = null
        {
            UpdateWeaponAttribute(i);
        }
    }


    //Contiene currentLevel ++1, no tiene que escribir en el externo.
    public void LevelUpWeaponAttribute(int weaponID)
    {
        weaponDatas[weaponID].currentLevel++;
        UpdateWeaponAttribute(weaponID);

    }

    //改
    public void UpdateWeaponAttribute(int weaponID)
    {
        List<Attribute> weaponCurrentLevelAttribute = weaponDatas[weaponID].weaponAttribute[weaponDatas[weaponID].currentLevel].currentLevelAttribute;

        //Extraer lista de attribute actualizado
        List<Attribute> attributeUpdate = attributeManager.TotalAttributeCalculation(weaponCurrentLevelAttribute);

        //Reemplazar con el viejo
        weaponDatas[weaponID].weaponAttribute[weaponDatas[weaponID].currentLevel].currentLevelAttribute = attributeUpdate;
    }
        */

}
