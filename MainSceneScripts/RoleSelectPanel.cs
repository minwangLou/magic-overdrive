using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using TMPro;
using UnityEngine.UI;

public class RoleSelectPanel : MonoBehaviour
{

    public static RoleSelectPanel instance;



    public List<RoleData> roleDatas; //Role data info

    public Transform _roleList; //Role UI list
    public GameObject rolePrefab; //Role prefab

    public TextMeshProUGUI _roleName; //role Name
    public Image _avatar; //role avatar
    public TextMeshProUGUI _roleDescription; //role description mostrate in scene

    public Image InitWeaponIcon;
    public TextMeshProUGUI InitWeaponName;

    public TextMeshProUGUI _AttributeContent;

    //Cambiar el color de panel una vez selecciona el role para partido
    public CustomButtonColor _roleDetailPanelColor;
    public CustomButtonColor _recordPanelColor;

     

    public CanvasGroup _canvasGroup; //canvas group


    private void Awake()
    {
        instance = this; 
    }

    



    // Start is called before the first frame update
    void Start()
    {

        roleDatas = SaveManager.instance.roleDatas;

        foreach (RoleData role in roleDatas)
        {
            if (role != null)
            {
                RoleUI roleUI = Instantiate(rolePrefab, _roleList).GetComponent<RoleUI>();
                roleUI.rolePanel = this;

                roleUI.SetRoleData(role);

                if (role.id == 1)
                {
                    roleUI.RenewUI(role);
                }
                
            }
         }
    }

    public void ChangeColorRolPanel(bool rolConfirm)
    {
        if (rolConfirm)
        {
            _roleDetailPanelColor.ChangeToPressColor();
            _recordPanelColor.ChangeToPressColor();
            /*
            _backGroundRole.color = pressColor;
            _backGroundRecord.color = pressColor;
            */
        }
        else
        {
            _roleDetailPanelColor.ChangeToNormalColor();
            _recordPanelColor.ChangeToNormalColor();
            /*
            _backGroundRole.color = normalColor;
            _backGroundRecord.color = normalColor;
            */
        }

    }
}
