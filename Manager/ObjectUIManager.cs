using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectUIManager : MonoBehaviour
{
    public static ObjectUIManager instance;

    private void Awake()
    {
        instance = this;

        HideAllIcons();
    }

    public List<ObjectIconInfo> weaponIconList;
    public List<ObjectIconInfo> bonusIconList;

    private int currentWeaponIconNumber;
    private int currentBonusIconNumber;

    private PausePanelController pausePanel;

    private void Start()
    {
        pausePanel = PausePanelController.instance;
    }

    private void HideAllIcons()
    {
        foreach(ObjectIconInfo info in weaponIconList)
        {
            info.objectIcon.enabled = false;
        }

        foreach (ObjectIconInfo info in bonusIconList)
        {
            info.objectIcon.enabled = false;
        }
    }

    public void AddWeaponIcon(WeaponData weapon)
    {
        /*ÃÌº”Œ‰∆˜Icon∫ÛΩ‚À¯
        if (currentWeaponIconNumber < weaponIconList.Count)
        {

            weaponIconList[currentWeaponIconNumber].objectIcon.enabled = true;

            weaponIconList[currentWeaponIconNumber].objectIcon.sprite 
                = Resources.Load<Sprite>(weapon.weaponIcon_location);
            weaponIconList[currentWeaponIconNumber].objectID = weapon.id;

            currentWeaponIconNumber++;
        }
        */
    }

    public void AddBonusIcon(BonusData bonus)
    {
        if (currentBonusIconNumber < bonusIconList.Count)
        {

            bonusIconList[currentBonusIconNumber].objectIcon.enabled = true;

            bonusIconList[currentBonusIconNumber].objectIcon.sprite 
                = Resources.Load<Sprite>(bonus.bonusIcon);
            bonusIconList[currentBonusIconNumber].objectID = bonus.id;

            currentBonusIconNumber++;

            PausePanelController.instance.AddBonusToList(bonus.id);
        }
    }

    
}

[System.Serializable]
public class ObjectIconInfo
{
    public int objectID;
    public Image objectIcon;
}
