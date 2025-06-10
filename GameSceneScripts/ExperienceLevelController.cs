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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerGetExp(int amountToGet)
    {
        currentExperience += Mathf.RoundToInt(amountToGet * expIncrease);

        LevelUp();

        UIController.instance.UpdateExperience(currentExperience, CalculateExpToLevelUp(), currentLevel);

    }

    public void SpawnExp (Vector3 positionSpawn, int expValue)
    {
        ExperiencePickUp experience = Instantiate(expPickUp, positionSpawn, Quaternion.identity);
        experience.setExpValue(expValue);

    }

    private void LevelUp()
    {
        int expNeedToLevelUp = CalculateExpToLevelUp();
            
        //El jugador ha recogido experiencia sufienciete para subir el nuvel
        while (currentExperience >= expNeedToLevelUp)
        {
            //level up
            currentExperience -= expNeedToLevelUp;
            currentLevel++;
            ShowLevelUpPanel();
            expNeedToLevelUp = CalculateExpToLevelUp();
        }

        
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
    private void ShowLevelUpPanel() {

        //Activar el panel de upgrate
        UIController.instance.levelUpPanel.SetActive(true);

        //Parar el tiempo de partido
        Time.timeScale = 0f;

        //ShowUpgradeWeapon();

        ShowLevelUpObject();
    }



    public void ShowLevelUpObject()
    {
        poolManager.ClearPool();

        poolManager.SetUpObjectPool();

        for (int i = 0; i < uiController.levelUpButtons.Length; i++)
        {
            PoolObject objectExtract = poolManager.ExtractRandomObjectFromPool();

            //测试，后续请删除
            //infoObjectExtract(objectExtract);
            if (objectExtract != null)
            {
                uiController.levelUpButtons[i].UpdateButtonDisplay(objectExtract);
            }
            else
            {
                //处理无升级物品，生成金币和血量
                //需添加
            }
            
        }

        //Mostrar solo los botones que tiene herramienta para realizar upgrade, omitir el botón que está vació 
        for (int i = 0; i < uiController.levelUpButtons.Length; i++)
        {
            if (i < poolManager.objectSelectList.Count)
            {
                uiController.levelUpButtons[i].gameObject.SetActive(true);

            }
            else
            {
                uiController.levelUpButtons[i].gameObject.SetActive(false);
            }

        }

    }
    //测试，后续请删除
    private void infoObjectExtract(PoolObject objectExtract)
    {
        Debug.Log(objectExtract.name);
        if (objectExtract.bonus != null)
        {

        }
        else
        {
            WeaponData weapon = objectExtract.weapon;
            Debug.Log(weapon.name + "\nCurrentLevel: " + weapon.currentLevel + "\n");
        }
        
    }

    /*
    //test de weapon pull, se cambiará luego
    private void ShowUpgradeWeapon()
    {

        weaponsToUpgrade.Clear();

        List<Weapon> availableWeapons = new List<Weapon>();
        
        availableWeapons.AddRange(PlayerController.instance.assignedWeapons);
        //primer hueco de upgrate, simpre que sea arma unblocked que tiene por el jugador
        if (availableWeapons.Count > 0)
        {
            int selected = Random.Range(0, availableWeapons.Count);
            weaponsToUpgrade.Add(availableWeapons[selected]);
            availableWeapons.RemoveAt(selected);
        }

        //añadir al weapon poll las armas que no está asignado al jugador
        //bloqueará este funcionamiento si el número de arma que tiene jugador es igual que máximo permitido
        int numberWeaponAssigned = PlayerController.instance.assignedWeapons.Count + PlayerController.instance.fullyLevelledWeapons.Count;
        if (numberWeaponAssigned < PlayerController.instance.maxWeapons)
        {
            availableWeapons.AddRange(PlayerController.instance.unassignedWeapons);
        }

        //restos huecos de upgrate
        for (int i = weaponsToUpgrade.Count; i < 3; i++)
        {
            if (availableWeapons.Count > 0)
            {
                int selected = Random.Range(0, availableWeapons.Count);
                weaponsToUpgrade.Add(availableWeapons[selected]);
                availableWeapons.RemoveAt(selected);
            }
        }

        //Actualizar la información de botón según el weapon recibido para upgrade
        for (int i = 0; i < weaponsToUpgrade.Count; i++)
        {
           UIController.instance.levelUpButtons[i].UpdateButtonDisplay(weaponsToUpgrade[i]);
        }


        //Mostrar solo los botones que tiene herramienta para realizar upgrade, omitir el botón que está vació 
        for (int i = 0; i < UIController.instance.levelUpButtons.Length; i++)
        {
            if (i < weaponsToUpgrade.Count)
            {
                UIController.instance.levelUpButtons[i].gameObject.SetActive(true);

            } else
            {
                UIController.instance.levelUpButtons[i].gameObject.SetActive(false);
            }

        }

    }
    */



}
