using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoWeapon : MonoBehaviour
{
    [HideInInspector] public float weaponDamage;
    [HideInInspector] public Vector2 moveDirection;
    [HideInInspector] public float moveSpeed;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = moveDirection * moveSpeed;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyController>()?.EnemyTakeDamage(weaponDamage, false);
        }
    }
}
