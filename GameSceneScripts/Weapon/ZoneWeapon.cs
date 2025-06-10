using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneWeapon : Weapon
{
    /*
    private float weaponDamage;
    private float timeBetweenDamage;
    private float damageCounter;

    public bool knockBackEnemy;

    private List<EnemyController> enemiesInRange = new List<EnemyController>();

    // Start is called before the first frame update
    
    void Start()
    {
        SetStats();



    }

    // Update is called once per frame
    void Update()
    {
        MakingDamageOverTime();

        if (statsUpdated == true)
        {
            statsUpdated = false;

            SetStats();
        }
    }

    public void SetStats()
    {
        weaponDamage = stats[weaponLevel].damage;
        transform.localScale = Vector3.one * stats[weaponLevel].range;
        timeBetweenDamage = stats[weaponLevel].speed;

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
        damageCounter -= Time.deltaTime;

        if (damageCounter <= 0)
        {
            damageCounter = timeBetweenDamage;

            for (int i = 0; i < enemiesInRange.Count; i++)
            {
                if (enemiesInRange[i] != null)
                {
                    enemiesInRange[i].EnemyTakeDamage(weaponDamage, knockBackEnemy);
                }
                else
                {
                    enemiesInRange.RemoveAt(i);
                    i--;
                }
            }
        }
    }
    */
    
}
