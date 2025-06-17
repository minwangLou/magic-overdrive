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
    public List<LevelUpSelectionButton> levelUpSelection;
    public Transform _LevelUpSelectionList;
    public GameObject selectionPrefab;
    public int selectionNumber = 3;



    //Coin
    public TMP_Text coinText;

    public TMP_Text timeSurvivalMostrate;
    [HideInInspector]public float timeSurvivalInSeconds;

    [HideInInspector]public int skipCount;
    [HideInInspector]public bool skipCountUpdate = false;
    public CanvasGroup skipbutton;
    public TMP_Text _counter;

    private void Start()
    {
        InstantiateSelectionbutton();
    }

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
        if (skipCount > 0)
        {
            SwitchPanelInGame.instance.DisableLevelUpPanel();

            ExperienceLevelController.instance.upgrateObjectSelect = true;
            
            skipCount--;
            CheckSkipCount();

        }
    }

    public void UpdateCoinText()
    {
        coinText.text = CoinController.instance.currentCoins.ToString();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void CheckSkipCount()
    {
        if (skipCount == 0)
        {
            skipbutton.alpha = 0.7f;
            skipbutton.interactable = false;
        }
        _counter.text = skipCount.ToString();

    }


    public void InstantiateSelectionbutton()
    {
        for (int i = 0; i< selectionNumber; i++)
        {
            GameObject buttonInstantiate = Instantiate(selectionPrefab, _LevelUpSelectionList);
            LevelUpSelectionButton button = buttonInstantiate.GetComponent<LevelUpSelectionButton>();

            button._alwaysButton.SetActive(false);

            levelUpSelection.Add(button);
            buttonInstantiate.SetActive(false);
        }

       

    }


    public void DisableSelectionList()
    {
        for (int i = 0; i< levelUpSelection.Count; i++)
        {
            levelUpSelection[i].gameObject.SetActive(false);
        }
    }
}
