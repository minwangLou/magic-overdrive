using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderBolt : MonoBehaviour
{
    private float damage;
    private float area;
    private float knockBackForce;

    private Animator anim;

    private CircleCollider2D _circleCollider;

    private void Awake()
    {
        _circleCollider = GetComponent<CircleCollider2D>();
        anim = GetComponent<Animator>();
    }

    public void Initialize(float damage, float area, float knockBackForce)
    {
        this.damage = damage;
        this.area = area;
        this.knockBackForce = knockBackForce;
    }

    private void Start()
    {
        _circleCollider.radius = area;
        ApplyImmediateDamage();

        StartCoroutine(DestroyObject());
    }

    private void ApplyImmediateDamage()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(
            transform.position,
            area
        );

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                EnemyController enemyComponent = hitCollider.GetComponent<EnemyController>();
                if (enemyComponent != null)
                {
                    enemyComponent.EnemyTakeDamage(damage, knockBackForce);
                }
            }
        }
    }


    private IEnumerator DestroyObject()
    {
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        float animLength = state.length;

        yield return new WaitForSeconds(animLength);

        Destroy(gameObject);
    }
}
