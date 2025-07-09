using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SwitchPanelOutGame : MonoBehaviour
{
    public static SwitchPanelOutGame instance;

    public GameObject mainPanel;
    public GameObject settingPanel;
    public GameObject bonusUpPanel;
    public GameObject roleSelectPanel;
    public GameObject mapSelectPanel;

    public CanvasGroup mainPanelCanva;
    

    private void Awake()
    {
        instance = this;

        mainPanel.SetActive(true);
        settingPanel.SetActive(true);
        roleSelectPanel.SetActive(true);
        mapSelectPanel.SetActive(true);
        bonusUpPanel.SetActive(true);
    }
    private void Start()
    {
        IniciateGame();

    }

    private void IniciateGame()
    {
        ChangeVisibilityCanva(mainPanelCanva,true);
        ChangeVisibilityCanva(SettingPanel.instance.canvasGroup, false);
        ChangeVisibilityCanva(RoleSelectPanel.instance._canvasGroup, false);
        ChangeVisibilityCanva(MapSelectPanel.instance._canvasGroup, false);
        ChangeVisibilityCanva(BonusUpPanel.instance._canvasGroup, false);
    }

    public void PassMainToSetting()
    {
        ChangeVisibilityCanva(SettingPanel.instance.canvasGroup, true);
    }

    public void PassSettingToMain()
    {
        ChangeVisibilityCanva(SettingPanel.instance.canvasGroup, false);
        AudioManager.instance.SaveVolumeValue();
    }

    public void PassMainToBonusUp()
    {
        ChangeVisibilityCanva(mainPanelCanva, false);
        ChangeVisibilityCanva(BonusUpPanel.instance._canvasGroup, true);
    }

    public void ReturnBonusUpToMain()
    {
        ChangeVisibilityCanva(BonusUpPanel.instance._canvasGroup, false);
        ChangeVisibilityCanva(mainPanelCanva, true);
    }

    public void PassMainToRol()
    {
        ChangeVisibilityCanva(mainPanelCanva, false);
        ChangeVisibilityCanva(RoleSelectPanel.instance._canvasGroup, true);
    }

    public void ReturnRolToMain()
    {
        ChangeVisibilityCanva(RoleSelectPanel.instance._canvasGroup, false);
        ChangeVisibilityCanva(mainPanelCanva, true);
    }

    public void QuitGame()
    {
        //Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void PassRolToMap()
    {
        ChangeVisibilityCanva(RoleSelectPanel.instance._canvasGroup, false);
        ChangeVisibilityCanva(MapSelectPanel.instance._canvasGroup, true);
    }

    public void ReturnMapToRol()
    {
        ChangeVisibilityCanva(RoleSelectPanel.instance._canvasGroup, true);
        ChangeVisibilityCanva(MapSelectPanel.instance._canvasGroup, false);
        GameManager.instance.roleSelected = null;
    }

    private void ChangeVisibilityCanva(CanvasGroup canvasGroup, bool enable)
    {
        if (enable)
        {
            canvasGroup.alpha = 1;
        }
        else
        {
            canvasGroup.alpha = 0;
        }
        canvasGroup.interactable = enable;
        canvasGroup.blocksRaycasts = enable;

    }


    public void DeleteDataFolder()
    {
        string folderPath = Path.Combine(Application.persistentDataPath, "Data");

        if (Directory.Exists(folderPath))
        {
            Directory.Delete(folderPath, true); // 第二个参数为 true 表示递归删除所有内容
            Debug.Log("已删除文件夹：" + folderPath);
        }
        else
        {
            Debug.LogWarning("文件夹不存在：" + folderPath);
        }
    }




}
