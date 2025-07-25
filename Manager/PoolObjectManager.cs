﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObjectManager : MonoBehaviour
{
    public static PoolObjectManager instance;

    //Lista empieza en el index 0, [0]!=null
    //Recopila primero el objeto de tipo bonus y luego el objeto de tipo weapon
    public List<PoolObject> assignObjectPool = new List<PoolObject>();
    public List<PoolObject> unassignObjectPool = new List<PoolObject>();
    public List<PoolObject> objectSelectList = new List<PoolObject>(); //object select to mostrate in levelUP panel and ready to upgrate

    public int maxNumberWeapon;
    [HideInInspector]public int currentNumberWeapon;

    public int maxNumberBonus;
    [HideInInspector]public int currentNumberBonus;


    [HideInInspector] public bool isReady = false;



    private void Awake()
    {
        instance = this;
    }



    public void SetUpObjectPool()
    {
        SetUpPoolFromBonus();
        SetUpPoolFromWeapons();
        isReady = true;
    }

    public void ClearPool()
    {
        assignObjectPool.Clear();
        unassignObjectPool.Clear();
        objectSelectList.Clear();
    }

    //Construir Pool extrayendo el objeto de tipo Bonus
    private void SetUpPoolFromBonus()
    {
        List<BonusData> bonusDatas = SaveManager.instance.bonusDatas;
        PoolObject poolObject;

        for (int i = 1; i< bonusDatas.Count; i++)
        {
            if (bonusDatas[i].unlock == 1 && bonusDatas[i].inGameCurrentLevel < bonusDatas[i].inGameMaxLevel)
            {

                if (bonusDatas[i].inGameCurrentLevel == 0 && currentNumberBonus < maxNumberBonus)
                {


                    poolObject = new PoolObject(unassignObjectPool.Count, bonusDatas[i].bonusName, bonusDatas[i].id, bonusDatas[i].rarityWeight, bonusDatas[i]);

                    unassignObjectPool.Add(poolObject);
                    
                }
                else if (bonusDatas[i].inGameCurrentLevel > 0)
                {
                    poolObject = new PoolObject(assignObjectPool.Count, bonusDatas[i].bonusName, bonusDatas[i].id, bonusDatas[i].rarityWeight, bonusDatas[i]);
                    assignObjectPool.Add(poolObject);
                }
                


            }
        }
    }

    //Construir Pool extrayendo el objeto de tipo Weapon
    private void SetUpPoolFromWeapons()
    {
        List<WeaponData> weaponDatas = WeaponManager.instance.weaponDatas;
        PoolObject poolObject;

        for (int i = 1; i<weaponDatas.Count; i++)
        {//重新检查
            if (weaponDatas[i].unloke == 1 && weaponDatas[i].currentLevel < weaponDatas[i].maxLevel)
            {
                

                if (weaponDatas[i].currentLevel == 0 && currentNumberWeapon < maxNumberWeapon)
                {
                    poolObject = new PoolObject(unassignObjectPool.Count, weaponDatas[i].name, weaponDatas[i].id, weaponDatas[i].rarityWeight, weaponDatas[i]);

                    unassignObjectPool.Add(poolObject);
                    
                }
                else if (weaponDatas[i].currentLevel > 0)
                {
                    poolObject = new PoolObject(assignObjectPool.Count, weaponDatas[i].name, weaponDatas[i].id, weaponDatas[i].rarityWeight, weaponDatas[i]);

                    assignObjectPool.Add(poolObject);
                }
                
            }

        }

    }

    //Extraer objeto aleatorio desde object pool
    public PoolObject ExtractRandomObjectFromPool()
    {
        int randomValue = Random.Range(0, 2);
        // 0 = Extraer el objeto de assignPool, 1 = Extraer el objeto de unassignPool
        PoolObject objectReturn = null;


        if (assignObjectPool.Count == 0 && unassignObjectPool.Count == 0)
            return null;

        if (unassignObjectPool.Count == 0 || 
            (assignObjectPool.Count != 0 && unassignObjectPool.Count != 0 && randomValue == 0))
            //Extraer el objecto de assign List
        {
            objectReturn = ExtractObject(assignObjectPool);
            assignObjectPool.Remove(objectReturn);
            

        }
        else if (assignObjectPool.Count == 0 ||
            (assignObjectPool.Count != 0 && unassignObjectPool.Count != 0 && randomValue == 1))
            //Extraer el objecto de unassign List
        {
            objectReturn = ExtractObject(unassignObjectPool);
            unassignObjectPool.Remove(objectReturn);

        }

        if (objectReturn != null)
        {
            objectSelectList.Add(objectReturn);
        }

        return objectReturn;//si devuelve null, pues lo sustituye por vida o dinero
    }

    //Extraer objeto de una lista concreta
    private PoolObject ExtractObject(List<PoolObject> objectPool)
    {
        int totalRarityWeight = 0;
        int randomValue;
        int sumRarityWeight = 0;

        PoolObject objectReturn = null;


        foreach (PoolObject objectInPool in objectPool)
        {
            totalRarityWeight += objectInPool.rarityWeight;
        }
        randomValue = Random.Range(0,totalRarityWeight);

        foreach (PoolObject objectInPool in objectPool)
        {
            sumRarityWeight += objectInPool.rarityWeight;
            if (sumRarityWeight > randomValue)
            {
                objectReturn = objectInPool;
                break;
            }

        }

        return objectReturn; 
    }




}
