using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class RoleUI : MonoBehaviour, IPointerDownHandler
{
    public Image _avatar;



    private RoleData roleData;

    private WeaponData initWeapon;

    public List<BonusData> bonusDatas;

    public RoleSelectPanel rolePanel;





    public void SetRoleData(RoleData roleData)
    {
        initWeapon = WeaponManager.instance.weaponDatas[roleData.initWeaponID];
        bonusDatas = SaveManager.instance.bonusDatas;
        this.roleData = roleData;
        if (roleData.unlock != 0)
        {
            _avatar.sprite = Resources.Load<Sprite>(roleData.avatar);
        }
        else
        {
            _avatar.sprite = Resources.Load<Sprite>("Image/Lock");
        }
    }


    public void RenewUI(RoleData roleData)
    {


        if (roleData.unlock != 0) //rol unlock
        {
            rolePanel._roleName.text = roleData.name.Replace("\\n", "\n");

            rolePanel._avatar.sprite = Resources.Load<Sprite>(roleData.avatar);
            rolePanel._roleDescription.text = roleData.description;


            rolePanel.InitWeaponIcon.sprite = Resources.Load<Sprite>(initWeapon.weaponIcon_location);
            rolePanel.InitWeaponName.text = initWeapon.name;

            //roleAttribute
            rolePanel._AttributeContent.text = RoleAttributeContext();

            //Register role data in button
            Rolebutton.instance.roleSelcted = roleData;
        }
        else //rol lock
        {
            
            rolePanel._roleName.text = "???";
            rolePanel._avatar.sprite = Resources.Load<Sprite>("Image/Lock");
            rolePanel._roleDescription.text = roleData.unlockCondition;
            //rolePanel._record.text = "No record";

            
            rolePanel.InitWeaponIcon.sprite = Resources.Load<Sprite>("Image/Lock"); ;
            rolePanel.InitWeaponName.text = "???";

            rolePanel._AttributeContent.text = "Need to Unlock the Character";

            Rolebutton.instance.roleSelcted = null;
            
        }
    }

    private string RoleAttributeContext ()
    {
        string result = null;
        for (int i = 0; i< roleData.roleBonusDatas.Count; i++)
        {
            RoleBonus currentRoleBonus = roleData.roleBonusDatas[i];
            result += bonusDatas[currentRoleBonus.idBonus].bonusName
                + " "
                + Utils.ValueBonusToString(currentRoleBonus.value, bonusDatas[currentRoleBonus.idBonus].bonusType)
                + "\n\n";
            
        }

        Debug.Log(result);
        return result;
    }

    
    public void OnPointerDown(PointerEventData eventData)//µã»÷
    {
        if (Rolebutton.instance.roleConfirmed == true)
        {
            Rolebutton.instance.SelectAnotherRole(roleData);
        }

        AudioManager.instance.PlaySound(SoundType.ButtonClick);
        RenewUI(roleData);

    }



}
