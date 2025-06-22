using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeWeapon : MonoBehaviour
{
    private float damage = 10f;
    private float tickDamage = 3f;
    private float duration = 3f;
    public float tickInterval = 1f;

    private Animator anim;
    private BoxCollider2D boxCol;

    private List<EnemyController> enemiesInRange = new List<EnemyController>();
    private LayerMask enemyMask;

    private float knockBackForce;

    void Awake()
    {
        boxCol = GetComponent<BoxCollider2D>();
        enemyMask = LayerMask.GetMask("Enemy"); // 确保敌人都在 Enemy 这个 Layer
    }

    public void Initialize(float damage, float tickDamage, float duration, float knockBackForce)
    {
        this.damage = damage;
        this.tickDamage = tickDamage;
        this.duration = duration;
        this.knockBackForce = knockBackForce;
    }


    void Start()
    {
        anim = GetComponent<Animator>();

        FirstAreaAttack();

        StartCoroutine(ApplyTickDamage());
        StartCoroutine(Utils.DestroyAfterDuration(anim,duration,gameObject));
    }

    private void FirstAreaAttack()
    {
        Vector2 center = boxCol.bounds.center;
        Vector2 size = boxCol.bounds.size;
        Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, transform.eulerAngles.z, enemyMask);

        foreach (var col in hits)
            OnTriggerEnter2D(col);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.EnemyTakeDamage(damage, knockBackForce);
                enemiesInRange.Add(enemy);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        EnemyController enemy = collision.GetComponent<EnemyController>();
        if (enemy != null && enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Remove(enemy);
        }
    }

    private IEnumerator ApplyTickDamage()
    {
        while (true)
        {
            yield return new WaitForSeconds(tickInterval);

            enemiesInRange.RemoveAll(e => e == null);

            var auxList = enemiesInRange.ToArray();

            foreach (EnemyController enemy in auxList)
            {
                if (enemy != null)
                    enemy.EnemyTakeDamage(tickDamage, knockBackForce*0.5f);
            }
        }
    }
}
