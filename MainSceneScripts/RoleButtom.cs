using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Rolebutton : MonoBehaviour, IPointerDownHandler
{
    public static Rolebutton instance;
    public RoleData roleSelcted;
    public GameObject _arrowImage;

    public bool roleConfirmed;

    public TextMeshProUGUI _buttonText;

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
                _buttonText.text = "START";
                _arrowImage.SetActive(true);
                roleConfirmed = true;

                RoleSelectPanel.instance.ChangeColorRolPanel(true);
            }
            else
            {
                AudioManager.instance.PlaySound(SoundType.ButtonClick);
                //Once Player confirmed role, register those role data in game manager.
                GameManager.instance.roleSelected = roleSelcted;

                //Disable role select panel from scene and Enable map selection panel
                SwitchPanelOutGame.instance.PassRolToMap();
                SelectAnotherRole(null);
            }
        }
    }

    public void SelectAnotherRole(RoleData anotherRoleData)
    {
        if (anotherRoleData == null || anotherRoleData != roleSelcted)
        {
            _buttonText.text = "CONFIRM";
            _arrowImage.SetActive(false);
            roleConfirmed = false;

            RoleSelectPanel.instance.ChangeColorRolPanel(false);
        }


    }

    public void ReturnMainPanel()
    {
        SelectAnotherRole(null);
    }
}
