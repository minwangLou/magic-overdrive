using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoWeapon : MonoBehaviour
{
    [HideInInspector] public float weaponDamage;
    [HideInInspector] public Vector2 moveDirection;
    [HideInInspector] public float moveSpeed;
    public float duration;

    private Animator anim;

    private Rigidbody2D rb;

    private void Start()
    {
        anim = GetComponent<Animator>();

        rb = GetComponent<Rigidbody2D>();
        rb.velocity = moveDirection * moveSpeed;

        StartCoroutine(DestroyAfterDuration());
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyController>()?.EnemyTakeDamage(weaponDamage, false);
        }
    }

    private IEnumerator DestroyAfterDuration()
    {
        yield return new WaitForSeconds(duration);

        anim.SetTrigger("Despawn");
        yield return new WaitForEndOfFrame(); // 等一帧让 Animator 切入新状态
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        float animLength = state.length;

        // 等待动画播完
        yield return new WaitForSeconds(animLength);

        Destroy(gameObject);
    }
}
