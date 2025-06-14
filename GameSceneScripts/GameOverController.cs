using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverController : MonoBehaviour
{
    public static GameOverController instance;

    private void Awake()
    {
        instance = this;
    }

    [HideInInspector]public int totalKills;
    [HideInInspector] public int bossDefeated;
    [HideInInspector] public string survivalTimeSeconds;
    [HideInInspector] public int level;
    [HideInInspector] public int coinObtain;

    public TMP_Text staticText;
    public TMP_Text variableText;

    private void ExtratcVariable()
    {
        survivalTimeSeconds = UIController.instance.timeSurvivalMostrate.text;
        level = ExperienceLevelController.instance.currentLevel;
        coinObtain = CoinController.instance.currentCoins;
    }

    public void UpdateTextDisplay()
    {
        ExtratcVariable();

        staticText.text = "Total kills:" + "\n" +
                      "Boss Defeated:" + "\n\n\n\n" +
                      "Survival Time:" + "\n" +
                      "Level:" + "\n\n\n\n" +
                      "Coin Obtain:";

        variableText.text = totalKills + "\n" +
                       bossDefeated + "\n\n\n\n" +
                       survivalTimeSeconds + "\n" +
                       level + "\n\n\n\n" +
                       coinObtain;

    }

    public void EndGame()
    {
        GameManager.instance.EndGame();
    }



}
