using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceLevelController : MonoBehaviour
{
    public static ExperienceLevelController instance;

    private void Awake()
    {
        instance = this;
    }

    public int currentExperience;

    public ExperiencePickUp expPickUp;

    //el valor inicial de la lista es 1, que corresponde con el nivel de experiencia
    //La longitud maxima de la lista es maxLevel+1, para poder encajar la vista
    //cada nivel de lista corresponde dicho nivel, la expriencia requirida
    public List<int> expLevels;
    public int currentLevel = 1, maxLevel;

    //public List<Weapon> weaponsToUpgrade;

    public List<PoolObject> objectToLevelUp;

    private UIController uiController;

    private PoolObjectManager poolManager;

    [HideInInspector]public float expIncrease; //Value of bonus growth

    public bool upgrateObjectSelect;

    public ExtraUpgrateType alwayExtraUpgrate;



    // Start is called before the first frame update
    void Start()
    {
        uiController = UIController.instance;

        poolManager = PoolObjectManager.instance;

        SetUpExpValue();

    }

    private void SetUpExpValue()
    {

        //Realizar la lista de los numeros de experiencia que requiere para cada nivel
        //Puede cambiar más tarde para la mejora de la experiencia del juego
        while (expLevels.Count <= maxLevel)
        {
            expLevels.Add(Mathf.CeilToInt(expLevels[expLevels.Count - 1] * 1.1f));
        }

        if (maxLevel < expLevels.Count)
        {
            maxLevel = expLevels.Count - 1;
        }
    }


    public void PlayerGetExp(int amountToGet)
    {
        currentExperience += Mathf.RoundToInt(amountToGet * expIncrease);

        StartCoroutine(LevelUp());

        

    }

    public void SpawnExp (Vector3 positionSpawn, int expValue)
    {
        ExperiencePickUp experience = Instantiate(expPickUp, positionSpawn, Quaternion.identity);
        experience.setExpValue(expValue);

    }



    public IEnumerator LevelUp()
    {
        int expNeedToLevelUp = CalculateExpToLevelUp();

        while (currentExperience >= expNeedToLevelUp)
        {
            
            currentExperience -= expNeedToLevelUp;
            currentLevel++;

            UIController.instance.UpdateExperience(currentExperience, CalculateExpToLevelUp(), currentLevel);

            //打入断点
            if (alwayExtraUpgrate != ExtraUpgrateType.Null)
            {
                ApplyExtraUpgrate(alwayExtraUpgrate);
            }
            else
            {

                AudioManager.instance.PlaySound(SoundType.LevelUp);

                // 弹出面板并等待玩家选择
                yield return ShowLevelUpPanelCoroutine();
            }


            expNeedToLevelUp = CalculateExpToLevelUp();
        }

        UIController.instance.UpdateExperience(currentExperience, CalculateExpToLevelUp(), currentLevel);
    }



    private IEnumerator ShowLevelUpPanelCoroutine()
    {

        SwitchPanelInGame.instance.ShowLevelUpPanel();

        ShowLevelUpSelection();

        upgrateObjectSelect = false; // 重置选择状态

        yield return new WaitUntil(() => upgrateObjectSelect);


    }


    private int CalculateExpToLevelUp()
    {
        int expNeedToLevelUp;

        if (currentLevel <= maxLevel)
        {
            expNeedToLevelUp = expLevels[currentLevel];
        }
        else
        {
            expNeedToLevelUp = expLevels[maxLevel];
        }
        return expNeedToLevelUp;
    }


    public void ShowLevelUpSelection()
    {
        poolManager.ClearPool();

        poolManager.SetUpObjectPool();


        for (int i = 0; i < uiController.selectionNumber; i++){

            PoolObject objectExtract = poolManager.ExtractRandomObjectFromPool();

            if (objectExtract != null)
            {
                uiController.levelUpSelection[i].gameObject.SetActive(true);
                uiController.levelUpSelection[i].UpdateButtonDisplay(objectExtract);
            }
        }

        if (poolManager.objectSelectList.Count == 0)
        {
            uiController.levelUpSelection[0].gameObject.SetActive(true);
            uiController.levelUpSelection[1].gameObject.SetActive(true);

            uiController.levelUpSelection[0].UpdatebuttonWithCoin();
            uiController.levelUpSelection[1].UpdatebuttonWithHealth();
        }

        poolManager.isReady = false;
    }

    public void ApplyExtraUpgrate(ExtraUpgrateType upgrateType)
    {
        if (upgrateType == ExtraUpgrateType.Coin)
        {
            CoinController.instance.AddCoins(50);

        }else if (upgrateType == ExtraUpgrateType.Health)
        {
            PlayerHealthController.instance.RecoverRoleHealth(50f);
        }
    }


}
