using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SwitchPanelInGame : MonoBehaviour
{
    public static SwitchPanelInGame instance;
    public List<GameObject> panelInGame;

    public CanvasGroup levelUpPanel;
    public CanvasGroup pausePanel;
    public CanvasGroup gameOverPanel;

    private bool permitPause;

    private void Awake()
    {
        instance = this;

        foreach (GameObject panel in panelInGame)
        {
            panel.SetActive(true);
        }
    }


    private void Start()
    {
        IniciateGameScene();
        permitPause = true;
    }

    private void Update()
    {
        PressEscToPause();
    }

    private void PressEscToPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (permitPause == true)
            {
                ShowPausePanel();
            }
            else if (permitPause == false && pausePanel.alpha == 1)
            {
                DisablePausePanel();
            }
            
        }
    }

    private void IniciateGameScene()
    {
        ChangeVisibilityCanva(levelUpPanel, false);
        ChangeVisibilityCanva(pausePanel, false);
        ChangeVisibilityCanva(gameOverPanel, false);

    }

    public void ShowLevelUpPanel()
    {
        permitPause = false;
        Time.timeScale = 0f;
        ChangeVisibilityCanva(levelUpPanel, true);
    }

    public void DisableLevelUpPanel()
    {
        permitPause = true;
        Time.timeScale = 1f;
        ChangeVisibilityCanva(levelUpPanel, false);
    }

    public void ShowPausePanel()
    {
        permitPause = false;
        Time.timeScale = 0f;
        ChangeVisibilityCanva(pausePanel, true);
        PausePanelController.instance.UpdateLevelAllObject();
    }

    public void DisablePausePanel()
    {
        permitPause = true;
        Time.timeScale = 1f;
        ChangeVisibilityCanva(pausePanel, false);
    }

    public void ShowGameOverPanel()
    {
        if (permitPause == false) //Pasar de pause panel al gameOver Panel
        {
            ChangeVisibilityCanva(pausePanel, false);
        }
        else //Player died, Pasar al GameOver Panel
        {
            //添加角色死亡动画，使用startcoruntine，播放结束再继续运行
            permitPause = false;
            Time.timeScale = 0f;
        }

        GameOverController.instance.UpdateTextDisplay();

        ChangeVisibilityCanva(gameOverPanel, true);

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
}
