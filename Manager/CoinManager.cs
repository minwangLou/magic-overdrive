using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public CoinData coinData;

    public static CoinManager instance;

    public TextMeshProUGUI coinAmountText;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        this.coinData = SaveManager.instance.coinData;

        if (GameManager.instance.coinObtainInGame > 0)
        {
            GameManager.instance.UpdateCoinAmount();
            SaveManager.instance.SaveCoinData();
        }
    }

    private void Update()
    {
        coinAmountText.text = coinData.coin.ToString();
    }


    public void AddCoin(int amount)
    {
        coinData.coin += amount;
    }

    //Reset all Bonus level to cero and return the coin costed in it
    public void RefundBonusCost()
    {
        coinData.coin += coinData.totalBonusCost;
        coinData.totalBonusCost = 0;
    }


}
