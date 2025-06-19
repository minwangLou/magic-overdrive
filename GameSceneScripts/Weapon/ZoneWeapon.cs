using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneWeapon : Weapon
{

    private float currentTimer;

    public bool knockBackEnemy;

    private List<EnemyController> enemiesInRange = new List<EnemyController>();


    // Update is called once per frame
    void Update()
    {
        MakingDamageOverTime();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            enemiesInRange.Add(collision.GetComponent<EnemyController>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            enemiesInRange.Remove(collision.GetComponent<EnemyController>());
        }
    }

    private void MakingDamageOverTime()
    {
        currentTimer -= Time.deltaTime;

        if (currentTimer <= 0)
        {
            currentTimer = coolDown;

            for (int i = 0; i < enemiesInRange.Count; i++)
            {
                if (enemiesInRange[i] != null)
                {
                    enemiesInRange[i].EnemyTakeDamage(might, knockBackEnemy);
                }
                else
                {
                    enemiesInRange.RemoveAt(i);
                    i--;
                }
            }
        }
    }
    
    
}
