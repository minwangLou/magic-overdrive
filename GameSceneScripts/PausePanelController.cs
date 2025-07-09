using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanelController : MonoBehaviour
{
    public static PausePanelController instance;

    private void Awake()
    {
        instance = this;
    }


    private List<ObjectPanelInfo> weaponPanelList = new List<ObjectPanelInfo>();
    private List<ObjectPanelInfo> bonusPanelList = new List<ObjectPanelInfo>();

    
    private List<WeaponData> weaponDatas;
    private List<BonusData> bonusDatas;

    public GameObject weaponPanelPrefab;
    public GameObject bonusPanelPrefab;
    public GameObject levelPrefab;

    public Transform _bonusPanelList;
    public Transform _weaponPanelList;

    private void OnEnable()
    {
        bonusDatas = SaveManager.instance.bonusDatas;
        weaponDatas = WeaponManager.instance.weaponDatas;
    }

    public void AddBonusToList (int bonusID)
    {
        ObjectPanelInfo newObject = new ObjectPanelInfo
        {
            objectID = bonusID,
            objectInPanel = Instantiate(bonusPanelPrefab, _bonusPanelList),
            LevelBackGroundList = new List<GameObject>(),
            objectType = ObjectType.Bonus

        };

        SetObjectIcon(newObject.objectInPanel, bonusDatas[bonusID].bonusIcon);


        Transform _levelList = newObject.objectInPanel.transform.GetChild(1);

        for (int i = 0; i< bonusDatas[bonusID].inGameMaxLevel; i++)
        {
            GameObject LevelFrame = Instantiate(levelPrefab, _levelList);
            //Extraer el backGround, color relleno del cuadro de nivel
            GameObject _LevelBackGround = LevelFrame.transform.GetChild(0).gameObject;

            _LevelBackGround.SetActive(false);

            newObject.LevelBackGroundList.Add(_LevelBackGround);

        }

        bonusPanelList.Add(newObject);

    }

    public void AddWeaponToList (int weaponID)
    {
        ObjectPanelInfo newObject = new ObjectPanelInfo 
        {
            objectID = weaponID,
            objectInPanel = Instantiate(weaponPanelPrefab, _weaponPanelList),
            LevelBackGroundList = new List<GameObject>(),
            objectType = ObjectType.Weapon

         };

        SetObjectIcon(newObject.objectInPanel, weaponDatas[weaponID].weaponIcon_location);

        Transform _levelList = newObject.objectInPanel.transform.GetChild(1);

        for (int i = 0; i < weaponDatas[weaponID].maxLevel; i++)
        {
            GameObject LevelFrame = Instantiate(levelPrefab, _levelList);
            //Extraer el backGround, color relleno del cuadro de nivel
            GameObject _LevelBackGround = LevelFrame.transform.GetChild(0).gameObject;

            _LevelBackGround.SetActive(false);

            newObject.LevelBackGroundList.Add(_LevelBackGround);

        }

        weaponPanelList.Add(newObject);
    }

    private void SetObjectIcon(GameObject panel, string iconPath)
    {
        Image iconImage = panel.transform.GetChild(0).GetComponent<Image>();
        iconImage.sprite = Resources.Load<Sprite>(iconPath);
    }

    public void UpdateLevelAllObject()
    {
        foreach (ObjectPanelInfo bonustToUpdate in bonusPanelList)
        {
            UpdateObjectLevel(bonustToUpdate);
        }

        foreach (ObjectPanelInfo weaponToUpdate in weaponPanelList)
        {
            UpdateObjectLevel(weaponToUpdate);
        }
    }
    

    private void UpdateObjectLevel(ObjectPanelInfo objectInPanel)
    {
        if (objectInPanel.objectType == ObjectType.Bonus)
        {
            for (int i = 0; i< bonusDatas[objectInPanel.objectID].inGameCurrentLevel; i++)
            {
                objectInPanel.LevelBackGroundList[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < weaponDatas[objectInPanel.objectID].currentLevel; i++)
            {
                objectInPanel.LevelBackGroundList[i].SetActive(true);
            }
        }
    }

}

public class ObjectPanelInfo
{
    public int objectID;
    public GameObject objectInPanel;
    public List<GameObject> LevelBackGroundList;
    public ObjectType objectType;


}
