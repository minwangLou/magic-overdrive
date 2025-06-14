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


    //Coin
    public TMP_Text coinText;

    //End Game Interface
    public GameObject gameOverInterface;

    public TMP_Text timeSurvivalMostrate;
    [HideInInspector]public float timeSurvivalInSeconds;

    private void Update()
    {
        timeSurvivalInSeconds += Time.deltaTime;
        timeSurvivalMostrate.text = Utils.FormatTime(timeSurvivalInSeconds);
    }


    public void UpdateExperience(int currentExp, int levelExp, int currentLevel)
    {
        expLevelSlider.maxValue = levelExp;
        expLevelSlider.value = currentExp;

        expLevelText.text = "Level: " + currentLevel;
    }

    public void SkipLevelUp()
    {
        SwitchPanelInGame.instance.DisableLevelUpPanel();

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
