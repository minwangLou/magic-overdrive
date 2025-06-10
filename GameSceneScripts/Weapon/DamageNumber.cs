using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    public TMP_Text damageText;

    public float numberSpawnTime;
    private float numberSpawnCounter;

    private float moveSpeed = 0.5f;


    // Update is called once per frame
    void Update()
    {
        UpdateDamageNumber();
    }

    private void UpdateDamageNumber()
    {
        if (numberSpawnCounter > 0)
        {
            numberSpawnCounter -= Time.deltaTime;

            if (numberSpawnCounter <= 0)
            {
                DamageNumberController.instance.RecicleNumberToPool(this);
            }

        }

        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

    }

    public void SetUp (int damageDisplay)
    {
        numberSpawnCounter = numberSpawnTime;

        damageText.text = damageDisplay.ToString();
    }
}
