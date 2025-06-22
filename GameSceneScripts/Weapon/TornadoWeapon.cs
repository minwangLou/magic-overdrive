using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoWeapon : MonoBehaviour
{
    private float damage;
    private Vector2 moveDirection;
    private float speed;
    private float duration;
    private float knockBackForce;

    private Animator anim;

    private Rigidbody2D rb;


    public void Initialize(float damage, Vector2 moveDirection, float speed, float duration, float knockBackForce )
    {
        this.damage = damage;
        this.moveDirection = moveDirection;
        this.speed = speed;
        this.duration = duration;
        this.knockBackForce = knockBackForce;
    }


    private void Start()
    {
        anim = GetComponent<Animator>();

        rb = GetComponent<Rigidbody2D>();
        rb.velocity = moveDirection * speed;

        StartCoroutine(Utils.DestroyAfterDuration(anim, duration, gameObject));
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyController>()?.EnemyTakeDamage(damage, knockBackForce);
        }
    }

}
