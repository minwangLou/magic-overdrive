using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpSelectionButton : MonoBehaviour
{

    public TMP_Text nameLevelText, upgradeText;
    public Image objectIcon;
    private PoolObject objectSelect;




    public void UpdateButtonDisplay(PoolObject objectSelect)
    {
        if (objectSelect.bonus != null){ //seleccionado el objecto de tipo bonus
            BonusData bonus = objectSelect.bonus;

            nameLevelText.text = bonus.bonusName + " - Lvl " + (bonus.inGameCurrentLevel + 1);

            upgradeText.text = bonus.bonusInGameDescription;
            //添加图片后取消comentar
            objectIcon.sprite = Resources.Load<Sprite>(bonus.bonusIcon);




        }
        else //seleccionado el objeto de tipo weapon
        {
            WeaponData weapon = objectSelect.weapon;

            nameLevelText.text = weapon.name + " - Lvl " + (weapon.currentLevel + 1);

            //添加图片后取消comentar
            //objectIcon.sprite = Resources.Load<Sprite>(weapon.weaponIcon_location);

            if (weapon.currentLevel > 0)
            {
                upgradeText.text = weapon.weaponAttribute[weapon.currentLevel].upgrateText;
            }
            else
            {
                upgradeText.text = "Unlock " + weapon.name;
            }
        }

        this.objectSelect = objectSelect;
    }


    public void SelectUpgrateOption()
    {
        if (objectSelect != null)
        {
            if (objectSelect.weapon != null)
            {
                WeaponManager.instance.SelectWeaponToUpgrate(objectSelect.idObject);
            }
            else
            {
                AttributeManager.instance.SelectBonusToUpgrate(objectSelect.idObject);
            }
            /*
            UIController.instance.levelUpPanel.SetActive(false);
            Time.timeScale = 1f;
            */
            SwitchPanelInGame.instance.DisableLevelUpPanel();

            ExperienceLevelController.instance.upgrateObjectSelect = true;

        }

    }

    //老方法，不使用
    //老方法，不使用
    /*
    public void UpdateButtonDisplay(Weapon theWeapon)
    {
        if (theWeapon.gameObject.activeSelf == true)
        {

            upgradeText.text = theWeapon.stats[theWeapon.weaponLevel].upgradeText;
            objectIcon.sprite = theWeapon.icon;

            nameLevelText.text = theWeapon.name + " - Lvl " + (theWeapon.weaponLevel + 1);


        }
        else
        {
            upgradeText.text = "Unlock " + theWeapon.name;
            objectIcon.sprite = theWeapon.icon;
            nameLevelText.text = theWeapon.name;

        }

        weaponS = theWeapon;
    }

    public void SelectUpgrade()
    {

        if (weaponS != null)
        {
            if (weaponS.gameObject.activeSelf == true)
            {
                weaponS.LevelUpWeapon();
            }
            else
            {
                PlayerController.instance.AddWeapon(weaponS);
            }
    

            UIController.instance.levelUpPanel.SetActive(false);
            Time.timeScale = 1f;

        }
    }
    */





}
