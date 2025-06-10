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
    public TextMeshProUGUI _record;// role record got
    public Image _backGroundRole;
    public Image _backGroundRecord;
    public Color normalColor;
    public Color pressColor;


    public TextMeshProUGUI _buttomText;

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
                if (role.id == 1)
                {
                    roleUI.RenewUI(role);
                }
                roleUI.SetRoleData(role);
            }
         }
    }

    public void ChangeColorRolPanel(bool rolConfirm)
    {
        if (rolConfirm)
        {
            _backGroundRole.color = pressColor;
            _backGroundRecord.color = pressColor;
        }
        else
        {
            _backGroundRole.color = normalColor;
            _backGroundRecord.color = normalColor;
        }

    }
}
