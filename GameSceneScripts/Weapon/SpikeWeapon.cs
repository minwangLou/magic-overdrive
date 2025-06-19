using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeWeapon : MonoBehaviour
{
    public float attack = 10f;
    public float tickDamage = 3f;
    public float duration = 3f;
    public float tickInterval = 1f;

    private Animator anim;

    private List<EnemyController> enemiesInRange = new List<EnemyController>();

    void Start()
    {
        anim = GetComponent<Animator>();

        // ��ʼһ���˺�
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
                enemy.EnemyTakeDamage(attack, false); // �ٲ�һ�γ�ʼ�˺������գ�
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

        anim.SetTrigger("Despawn");
        yield return new WaitForEndOfFrame(); // ��һ֡�� Animator ������״̬
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        float animLength = state.length;

        // �ȴ���������
        yield return new WaitForSeconds(animLength);

        Destroy(gameObject);
    }
}
