using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pausePanel;

    private void Start()
    {
        if (pausePanel.activeSelf == true)
        {
            pausePanel.SetActive(false);
        }
    }


    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            pausePanel.SetActive(true);

            PausePanelController.instance.UpdateLevelAllObject();
        }
        else
        {
            Time.timeScale = 1f;
            pausePanel.SetActive(false);
        }
    }

    // ¿ÉÑ¡£ºESC¼ü´¥·¢ÔÝÍ£
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
}
