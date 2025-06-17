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

    public Sprite coin, health;
    private bool coinbutton;
    private bool healthbutton;

    private ExtraUpgrateType upgrateType;
    public GameObject _alwaysButton;



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


    public void UpdatebuttonWithCoin()
    {
        nameLevelText.text = "COIN";
        upgradeText.text = "Claim coin";
        objectIcon.sprite = coin;

        upgrateType = ExtraUpgrateType.Coin;

        activeAlwaysButton();
    }

    public void UpdatebuttonWithHealth()
    {
        nameLevelText.text = "Health";
        upgradeText.text = "Recover Health";
        objectIcon.sprite = health;

        upgrateType = ExtraUpgrateType.Health;

        activeAlwaysButton();


    }

    private void activeAlwaysButton()
    {
        if (_alwaysButton.activeSelf == false)
        {
            _alwaysButton.SetActive(true);
        }
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



        }
        else//生命值和金币
        {
            Debug.Log("coinbutton: "+ coinbutton);
            Debug.Log("healthbutton: " + healthbutton);

            ExperienceLevelController.instance.ApplyExtraUpgrate(upgrateType);
        }

        objectSelect = null;

        EndSelection();
    }

    private void EndSelection()
    {
        UIController.instance.DisableSelectionList();

        SwitchPanelInGame.instance.DisableLevelUpPanel();

        ExperienceLevelController.instance.upgrateObjectSelect = true;
    }

    public void SelectAlwaysUpgrate()
    {
        ExperienceLevelController.instance.alwayExtraUpgrate = upgrateType;
        EndSelection();
    } 



}
