using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBullet : MonoBehaviour
{
    private Vector2 moveDirection;
    private float speed;
    private float damage;
    private float knockBackForce;
    private float duration;

    private Animator anim;

    public void Initialize(Vector2 moveDirection, float speed, float damage, float knockBackForce, float duration)
    {
        this.moveDirection = moveDirection;
        this.speed = speed;
        this.damage = damage;
        this.knockBackForce = knockBackForce;
        this.duration = duration;

        transform.right = moveDirection;

    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(Utils.DestroyAfterDuration(anim, duration, gameObject));
    }

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.EnemyTakeDamage(damage, knockBackForce);
            }

            speed = 0;
            StartCoroutine(Utils.DestroyAfterDuration(anim, 0, gameObject));
        }
    }
}
