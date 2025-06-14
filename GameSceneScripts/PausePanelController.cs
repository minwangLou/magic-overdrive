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


    private List<ObjectPanelInfo> weaponList = new List<ObjectPanelInfo>();
    private List<ObjectPanelInfo> bonusList = new List<ObjectPanelInfo>();

    
    private List<WeaponData> weaponDatas;
    private List<BonusData> bonusDatas;

    public GameObject bonusPrefab;
    public GameObject levelPrefab;

    public Transform _bonusList;

    private void Start()
    {
        bonusDatas = SaveManager.instance.bonusDatas;
        weaponDatas = WeaponManager.instance.weaponDatas;
    }

    public void AddBonusToList (int bonusID)
    {
        ObjectPanelInfo newObject = new ObjectPanelInfo();
        newObject.objectID = bonusID;
        newObject.objectInPanel = Instantiate(bonusPrefab, _bonusList);

        SetObjectIcon(newObject.objectInPanel, bonusDatas[bonusID].bonusIcon);


        Transform _levelList = newObject.objectInPanel.transform.GetChild(1);


        newObject.LevelBackGroundList = new List<GameObject>();
        newObject.objectType = ObjectType.Bonus;
        for (int i = 0; i< bonusDatas[bonusID].inGameMaxLevel; i++)
        {
            GameObject LevelFrame = Instantiate(levelPrefab, _levelList);
            //Extraer el backGround, color relleno del cuadro de nivel
            GameObject _LevelBackGround = LevelFrame.transform.GetChild(0).gameObject;

            _LevelBackGround.SetActive(false);

            newObject.LevelBackGroundList.Add(_LevelBackGround);

        }

        bonusList.Add(newObject);

    }

    private void SetObjectIcon(GameObject panel, string iconPath)
    {
        Image iconImage = panel.transform.GetChild(0).GetComponent<Image>();
        iconImage.sprite = Resources.Load<Sprite>(iconPath);
    }

    public void UpdateLevelAllObject()
    {
        foreach (ObjectPanelInfo objectToUpdate in bonusList)
        {
            UpdateObjectLevel(objectToUpdate);
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
    }

}

public class ObjectPanelInfo
{
    public int objectID;
    public GameObject objectInPanel;
    public List<GameObject> LevelBackGroundList;
    public ObjectType objectType;


}
