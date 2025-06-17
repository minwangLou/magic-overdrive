using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class RoleUI : MonoBehaviour, IPointerDownHandler
{
    public Image _backImage;
    public Image _avatar;


    private RoleData roleData;

    private RoleSelectPanel rolePanel;



    private void Awake()
    {
        rolePanel = RoleSelectPanel.instance;
    }

    private void Start()
    {

        _backImage = GetComponent<Image>();
        _avatar = transform.GetChild(0).GetComponent<Image>();
    }

    public void SetRoleData(RoleData roleData)
    {
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
            rolePanel._roleName.text = roleData.name;
            rolePanel._avatar.sprite = Resources.Load<Sprite>(roleData.avatar);
            rolePanel._roleDescription.text = roleData.description;
            rolePanel._record.text = RecordText(roleData.record);

            //Register role data in button
            Rolebutton.instance.roleSelcted = roleData;
        }
        else //rol lock
        {

            rolePanel._roleName.text = "???";
            rolePanel._avatar.sprite = Resources.Load<Sprite>("Image/Lock");
            rolePanel._roleDescription.text = roleData.unlockCondition;
            rolePanel._record.text = "No record";

            Rolebutton.instance.roleSelcted = null;
        }
    }

    private string RecordText (int record)
    {
        if (record == -1)
        {
            return "No record";
        }
        else
        {
            return "Pass Level: " + roleData.record.ToString();
        }
    }


    
    public void OnPointerDown(PointerEventData eventData)//µã»÷
    {

        RenewUI(roleData);

        if (Rolebutton.instance.roleConfirmed == true)
        {
            Rolebutton.instance.SelectAnotherRole();
        }


    }



}
