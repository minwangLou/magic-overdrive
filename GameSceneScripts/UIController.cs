using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    private void Awake()
    {
        instance = this;
    }

    //Experience
    public Slider expLevelSlider;
    public TMP_Text expLevelText;

    //level up buttons
    public LevelUpSelectionButton[] levelUpButtons;
    public GameObject levelUpPanel;


    //Coin
    public TMP_Text coinText;



    void Start()
    {
        if (levelUpPanel.activeSelf == true)
        {
            levelUpPanel.SetActive(false);
        }
    }


    public void UpdateExperience(int currentExp, int levelExp, int currentLevel)
    {
        expLevelSlider.maxValue = levelExp;
        expLevelSlider.value = currentExp;

        expLevelText.text = "Level: " + currentLevel;
    }

    public void SkipLevelUp()
    {
        levelUpPanel.SetActive(false);
        Time.timeScale = 1f;

        ExperienceLevelController.instance.upgrateObjectSelect = true;
    }

    public void UpdateCoinText()
    {
        coinText.text = CoinController.instance.currentCoins.ToString();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }
}
