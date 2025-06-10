using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public static CoinController instance;
    public float coinIncrease; //value of bonus greed

    public void Awake()
    {
        instance = this;
    }

    public int currentCoins;

    public CoinPickUp coin;

    public void AddCoins(int coinsToAdd)
    {
        currentCoins += Mathf.RoundToInt(coinsToAdd * coinIncrease);
        UIController.instance.UpdateCoinText();
    }

    public void DropCoin(Vector3 position, int value)
    {
        CoinPickUp newCoin = Instantiate(coin, position + new Vector3(0.2f, 0.1f, 0f), Quaternion.identity);
        newCoin.coinAmount = value;
        newCoin.gameObject.SetActive(true);
    }
    
}
