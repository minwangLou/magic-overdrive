using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumberController : MonoBehaviour
{
    public static DamageNumberController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }


    }

    public DamageNumber numberToSpawn;
    public Transform positionCanva;
    public List<DamageNumber> numberPool = new List<DamageNumber>();


    public void SpawnDamage (float damageAmount, Vector3 location)
    {
        int damageAmountInt = Mathf.RoundToInt(damageAmount);

        //DamageNumber newDamage = Instantiate(numberToSpawn, location, Quaternion.identity, positionCanva);
        DamageNumber newDamage = GetFromPool();

        newDamage.SetUp(damageAmountInt);

        newDamage.gameObject.SetActive(true);

        newDamage.transform.position = location;
    }

    //Devolver un texto de daño que está en el recicle pool
    //Si recicle pool está vacío, pues crea una nuevo texto de daño y devolver dicha nueva instancia creada
    public DamageNumber GetFromPool()
    {
        DamageNumber numberFromPool = null;

        if (numberPool.Count == 0)
        {
            numberFromPool = Instantiate(numberToSpawn, positionCanva);
        }
        else
        {
            numberFromPool = numberPool[0];
            numberPool.RemoveAt(0);
        }

        return numberFromPool;
    }

    //Meter los textos de daños al recicle pool para reutilizarlas
    public void RecicleNumberToPool(DamageNumber numberToRecicle)
    {
        numberToRecicle.gameObject.SetActive(false);

        numberPool.Add(numberToRecicle);
    }
}
