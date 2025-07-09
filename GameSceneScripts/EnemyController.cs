using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{


    private Rigidbody2D rbEnemy;
    private Transform target;

    public int id;

    private float damage;
    private float moveSpeed;
    private float maxHealth;

    private float knockBackTime = 1f;
    private float knockBackCounter;

    private float hitWaitTime = 1f;
    private bool canHit = true;


    //experience
    private int expDrop;

    //coin
    private int coinDrop;
    private float coinDropRate;

    public bool enemyBoss = false;

    private float currentMoveSpeed;
    private float knockBackForce;

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerController.instance.gameObject.transform;
        rbEnemy = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        updateKnockBackCounter();
        if (target != null)
        {
            rbEnemy.velocity = (target.position - transform.position).normalized * currentMoveSpeed;
        }

    }

    public void SetEnemyAttribute(EnemyData enemyData)
    {
        damage = enemyData.damage;
        moveSpeed = enemyData.moveSpeed;
        currentMoveSpeed = moveSpeed;

        maxHealth = enemyData.maxHealth;

        knockBackTime = enemyData.knockBackTime;
        hitWaitTime = enemyData.hitWaitTime;

        expDrop = enemyData.experienceDrop;
        coinDrop = enemyData.coinDrop;
        coinDropRate = enemyData.coinDropRate;
    }

    //Cuando se choca con el jugar el jugador, hacerle daño
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && canHit)
        {
            StartCoroutine(DealDamageOverTime());
        }
    }

    //Reducir vida del jugador mediante el damage que tiene por el enemigo
    private IEnumerator DealDamageOverTime()
    {
        canHit = false;
        PlayerHealthController.instance.TakeDamage(damage);
        yield return new WaitForSeconds(hitWaitTime);
        canHit = true;
    }


    //Sufrir ataque por el enemigo cuando choca con el arma del jugador.
    public void ApplyDamage(float damageToTake)
    {
        maxHealth -= damageToTake;

        DamageNumberController.instance.SpawnDamage(damageToTake, transform.position);

        if (maxHealth <= 0)
        {

            DropExperience();

            DropCoin();

            EnemySpawner.instance.enemyInstantiate.Remove(gameObject);
            Destroy(gameObject);


            GameOverController.instance.totalKills++;
            if (enemyBoss == true)
            {
                GameOverController.instance.bossDefeated++;
            }
            
        }

        
    }
   
    //Reducir vida de enemigo
    //y llamar al método para realizar el efecto de knock back cuando la herramienta le coincide
    public void EnemyTakeDamage(float damageToTake, float knockBackForce)
    {
        if (damageToTake > 0)
        {
            AudioManager.instance.PlaySound(SoundType.HitEnemy);
            ApplyDamage(damageToTake);
        }

        if ( knockBackCounter <= 0)
        {
            currentMoveSpeed = -knockBackForce;
            knockBackCounter = knockBackTime;
        }


    }
    //Realizar el efecto de Knock Back cuando la herramienta le choca
    private void updateKnockBackCounter()
    {
        if (knockBackCounter > 0)
        {
            knockBackCounter -= Time.deltaTime;

            if (knockBackCounter <= 0)
            {
                currentMoveSpeed = moveSpeed;
            }
        }
    }
    

    private void DropExperience()
    {

        ExperienceLevelController.instance.SpawnExp(transform.position, expDrop);
    }

    private void DropCoin()
    {

        if (Random.value < coinDropRate)
        {
            CoinController.instance.DropCoin(transform.position, coinDrop);
        }
    }

}





