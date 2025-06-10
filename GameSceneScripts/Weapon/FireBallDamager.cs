using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallDamager : MonoBehaviour
{

    [HideInInspector]public float weaponDamage;
    public bool knockBackEnemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<EnemyController>().EnemyTakeDamage(weaponDamage, knockBackEnemy);
        }
    }

}
