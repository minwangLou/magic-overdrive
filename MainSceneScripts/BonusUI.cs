using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BonusUI : MonoBehaviour, IPointerDownHandler
{
    public TextMeshProUGUI _bonusName;
    public Image _bonusIcon;

    public GameObject _bonusLevelPrefab;
    public Transform _levelList;
    [HideInInspector] public List<GameObject> _currentLevel;

    public Image _backGround;
    public Color maxLevelColor;
    public Color normalColor;
    //private bool maxLevel;

    public CustomButtonColor buttomColorNormal;
    public CustomButtonColor buttomColorMaxLevel;

    public BonusData bonusData;

    private CoinData coinData;

    [HideInInspector] public BonusUpPanel bonusPanel;


    private void Start()
    {
        bonusPanel = BonusUpPanel.instance;
        SwitchColorPress();

    }

    public void SetMapData(BonusData bonusData)
    {
        if (bonusData.unlock != 0)
        {
            this.bonusData = bonusData;

            _bonusName.text = bonusData.bonusName;

            //等有了图片再取消comentar
            _bonusIcon.sprite = Resources.Load<Sprite>(bonusData.bonusIcon);

            for (int i = 0; i < bonusData.outGameMaxLevel; i++)
            {
                Transform currentLevel = Instantiate(_bonusLevelPrefab, _levelList).transform;
                _currentLevel.Add(currentLevel.GetChild(0).gameObject);
            }

            //注意检查当前等级是否生成没问题
            RenewLevelMostration();
        }
    }


    public void RenewUI(BonusData bonusData)
    {

        //有图片之后取消comentar
        bonusPanel._bonusIcon.sprite = Resources.Load<Sprite>(bonusData.bonusIcon);

        bonusPanel._bonusName.text = bonusData.bonusName;
        bonusPanel._bonusDescription.text = bonusData.bonusOutGameDescription;
        CostText();

        bonusPanel.bonusSelect = this;

    }

    public void LevelUpBonus()
    {
        if (coinData == null)
        {
            coinData = CoinManager.instance.coinData;
        }


        if (bonusData.outGameCurrentLevel < bonusData.outGameMaxLevel && coinData.coin >= (bonusData.cost*(bonusData.outGameCurrentLevel+1)))
        {
            //purchase bonus, spend correspond amount of coin
            coinData.coin -= (bonusData.cost * (bonusData.outGameCurrentLevel + 1));
            coinData.totalBonusCost += (bonusData.cost * (bonusData.outGameCurrentLevel + 1));

            //Mostrate level up, put tick on frame
            _currentLevel[bonusData.outGameCurrentLevel].SetActive(true);
            bonusData.outGameCurrentLevel += 1;
            CostText();


        }
        
        if (bonusData.outGameCurrentLevel == bonusData.outGameMaxLevel)
        {
            CostText();

            //disable cost icon
            bonusPanel._bonusCostIcon.SetActive(false);

            //change background color to yellow
            SwitchColorPress();

        }
        
    }

    public void OnPointerDown(PointerEventData eventData)//点击
    {

        RenewUI(bonusData);

    }


    private void CostText()
    {
        if (bonusData.outGameCurrentLevel < bonusData.outGameMaxLevel)
        {
            bonusPanel._bonusCostText.text = ((bonusData.outGameCurrentLevel + 1) * bonusData.cost).ToString();
            bonusPanel._bonusCostIcon.SetActive(true);
        }
        else
        {
            bonusPanel._bonusCostText.text = "MAX LEVEL";
            bonusPanel._bonusCostIcon.SetActive(false);
        }
    }

    public void RenewLevelMostration()
    {
        for (int i = 0; i < bonusData.outGameMaxLevel; i++)
        {
            if (i < bonusData.outGameCurrentLevel)
            {
                _currentLevel[i].SetActive(true);
            }
            else
            {
                _currentLevel[i].SetActive(false);
            }

        }
    }

    public void ResetBonusLevel()
    {
        bonusData.outGameCurrentLevel = 0;
        RenewLevelMostration();

        //change color of backGround for not maximum level
        SwitchColorPress();

        //enable cost icon
        bonusPanel._bonusCostIcon.SetActive(true);
    }

    private void SwitchColorPress()
    { 
        if (bonusData.outGameCurrentLevel < bonusData.outGameMaxLevel)
        {
            _backGround.color = normalColor;
            buttomColorNormal.enabled = true;
            buttomColorMaxLevel.enabled = false;
        }
        else
        {
            _backGround.color = maxLevelColor;
            buttomColorNormal.enabled = false;
            buttomColorMaxLevel.enabled = true;
        }
    }

}
