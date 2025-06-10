using System.Collections;
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

    //Eliminar todo el objeto de tipo bonus o weapon de pool unassign por llegar número de objeto máximo.
    private bool bonusDelete = false;
    private bool weaponDelete = false;



    private void Awake()
    {
        instance = this;
    }



    public void SetUpObjectPool()
    {
        SetUpPoolFromBonus();
        SetUpPoolFromWeapons();
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
        {
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

        Debug.Log("assign pool number: " + assignObjectPool.Count);
        Debug.Log("unassign pool number: " + unassignObjectPool.Count);

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



    //下面的metodo不会被用到
    //下面的metodo不会被用到

    /*
    //Sacar un objeto de la pool unassign y meter a pool assign
    //Desbloquear el objeto
    public void AssignObject(int idObject, ObjectType objectType)
    {
        PoolObject obj = null;

        foreach (PoolObject objectUnasign in unassignObjectPool)
        {
            if (objectUnasign.idObject == idObject && objectUnasign.objectType == objectType)
            {
                obj = objectUnasign;
                break;
            }
        }

        if (obj != null) {
            unassignObjectPool.Remove(obj);
            assignObjectPool.Add(obj);

        }
    }

    //Sacar un objeto de la pool assign por llegar al nivel máximo
    public void ObjectArriveMaximunLevel(int idObject, ObjectType objectType)
    {
        PoolObject obj = null;

        foreach (PoolObject objectAssign in assignObjectPool)
        {
            if (objectAssign.idObject == idObject && objectAssign.objectType == objectType)
            {
                obj = objectAssign;
            }
        }

        if (obj != null)
        {
            assignObjectPool.Remove(obj);
            Debug.Log("Eliminar weapon éxito de assignPool por llegar al nivel Máximo");

            //opción de anadir objeto a otra lista para evolucionar en otra forma
        }
    }


    //Se realiza despues de seleccionar un objeto de assign pool
    //comprueba si es necesario eliminar de unassign pool los objetos de tipo bonus weapon debido alcanzar numero máximo permitido.
    //En cuanto detecta si es necesario, procesar la eliminación
    public void DeteleObjectFromPool()
    {
        if (bonusDelete == false)
        {
            DeleteBonus();
        } else if (weaponDelete == false)
        {
            DeleteWeapon();
        }
    }

    //Eliminar todos los objetos de tipo bonus debido ha alcanzado número máximo permitido
    private void DeleteBonus()
    {
        List<PoolObject> toRemovePool = new List<PoolObject>();
        if (currentNumberBonus == maxNumberBonus)
        {
            foreach (PoolObject objectUnassign in unassignObjectPool)
            {
                if (objectUnassign.objectType == ObjectType.Bonus)
                {
                    toRemovePool.Add(objectUnassign);
                }
            }

            foreach (PoolObject objetRemove in toRemovePool)
            {
                unassignObjectPool.Remove(objetRemove);
            }

            bonusDelete = true;

        }
    }

    private void DeleteWeapon()
    {
        List<PoolObject> toRemovePool = new List<PoolObject>();

        if (currentNumberWeapon == maxNumberWeapon)
        {
            foreach (PoolObject objectUnassign in unassignObjectPool)
            {
                if (objectUnassign.objectType == ObjectType.Weapon)
                {
                    toRemovePool.Add(objectUnassign);
                }
            }

            foreach (PoolObject objetRemove in toRemovePool)
            {
                unassignObjectPool.Remove(objetRemove);
            }

            weaponDelete = true;

        }
    }
    */



}
