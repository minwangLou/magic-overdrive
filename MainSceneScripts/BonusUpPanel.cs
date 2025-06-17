using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using TMPro;
using UnityEngine.UI;

public class BonusUpPanel : MonoBehaviour
{
    public static BonusUpPanel instance;

    public List<BonusData> bonusDatas;

    public Transform _bonusList;
    public GameObject bonusPrefab;

    public Image _bonusIcon;
    public TextMeshProUGUI _bonusName;
    public TextMeshProUGUI _bonusDescription;
    public TextMeshProUGUI _bonusCostText;
    public GameObject _bonusCostIcon;
    [HideInInspector] public BonusUI bonusSelect;
    [HideInInspector] public List<BonusUI> listBonusUI;

    public CanvasGroup _canvasGroup;
    

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        bonusDatas = SaveManager.instance.bonusDatas;

        foreach (BonusData bonus in bonusDatas)
        {
            if (bonus != null)
            {
                BonusUI bonusUI = Instantiate(bonusPrefab, _bonusList).GetComponent<BonusUI>();
                listBonusUI.Add(bonusUI);

                bonusUI.SetMapData(bonus);

                if (bonus.id == 1)
                {
                    bonusUI.bonusPanel = instance;
                    bonusUI.RenewUI(bonus);
                }

            }



        }
    }

    public void Buybutton()
    {
        if (bonusSelect != null)
        {
            bonusSelect.LevelUpBonus();
        }
    }

    public void RefundBonusUps()
    {
        CoinManager.instance.RefundBonusCost();

        foreach(BonusUI bonusUI in listBonusUI)
        {
            bonusUI.ResetBonusLevel();
        }
        if (bonusSelect != null)
        {
            bonusSelect.RenewUI(bonusSelect.bonusData);
        }
    }



}
