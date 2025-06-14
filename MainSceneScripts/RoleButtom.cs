using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class RoleButtom : MonoBehaviour, IPointerDownHandler
{
    public static RoleButtom instance;
    public RoleData roleSelcted;
    public GameObject _arrowImage;

    public bool roleConfirmed;

    public TextMeshProUGUI _buttomText;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _arrowImage.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (roleSelcted!=null)
        {
            //Not have role confirmed yet
            if (roleConfirmed == false)
            {
                _buttomText.text = "START";
                _arrowImage.SetActive(true);
                roleConfirmed = true;

                RoleSelectPanel.instance.ChangeColorRolPanel(true);
            }
            else
            {
                //Once Player confirmed role, register those role data in game manager.
                GameManager.instance.roleSelected = roleSelcted;

                //Disable role select panel from scene and Enable map selection panel
                SwitchPanelOutGame.instance.PassRolToMap();
                SelectAnotherRole();
            }
        }
    }

    public void SelectAnotherRole()
    {
        _buttomText.text = "CONFIRM";
        _arrowImage.SetActive(false);
        roleConfirmed = false;

        RoleSelectPanel.instance.ChangeColorRolPanel(false);
    }

}
