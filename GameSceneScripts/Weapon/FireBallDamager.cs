using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallDamager : MonoBehaviour
{

    [HideInInspector]public float weaponDamage;
    public float knockBackForce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<EnemyController>().EnemyTakeDamage(weaponDamage, knockBackForce);
        }
    }

}
