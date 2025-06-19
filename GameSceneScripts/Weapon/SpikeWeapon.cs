using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeWeapon : MonoBehaviour
{
    public float attack = 10f;
    public float tickDamage = 3f;
    public float duration = 3f;
    public float tickInterval = 1f;

    private List<EnemyController> enemiesInRange = new List<EnemyController>();

    void Start()
    {
        // 初始一次伤害
        foreach (EnemyController enemy in enemiesInRange)
        {
            if (enemy != null)
                enemy.EnemyTakeDamage(attack, false);
        }

        StartCoroutine(ApplyTickDamage());
        StartCoroutine(DestroyAfterDuration());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null && !enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Add(enemy);
                enemy.EnemyTakeDamage(attack, false); // 再补一次初始伤害（保险）
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
            foreach (EnemyController enemy in enemiesInRange)
            {
                if (enemy != null)
                    enemy.EnemyTakeDamage(tickDamage, false);
            }
        }
    }

    private IEnumerator DestroyAfterDuration()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
