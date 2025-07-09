using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSpawner : Weapon
{
    private List<EnemyController> enemiesInRange = new List<EnemyController>();
    private int currentShieldPoint;
    private float coolDownTimer;


    public GameObject shieldPrefab;
    private List<GameObject> shieldInstantiateList = new List<GameObject>();

    public float intervalShield;
    public float initScale;
    private float invincibleTimer;

    [HideInInspector]public bool playerInvincible;

    private void Start()
    {

        currentShieldPoint = amount;
        coolDownTimer = coolDown;
        invincibleTimer = duration;

        GenerateInitialShieldRings();

        PlayerHealthController.instance.shield = this;
    }

    private void GenerateInitialShieldRings()
    {
        for (int ringIndex = 0; ringIndex < currentShieldPoint; ringIndex++)
        {
            UpdateShiled(ringIndex);
        }
    }

    private void Update()
    {
        RecoverShieldByTime();

        UpdateInvicibleTime();
    }

    private void UpdateInvicibleTime()
    {
        if (playerInvincible == true)
        {

            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                playerInvincible = false;
                invincibleTimer = duration;
            }
        }

    }

    private void RecoverShieldByTime()
    {
        if (currentShieldPoint < amount)
        {
            coolDownTimer -= Time.deltaTime;
            if (coolDownTimer < 0f)
            {
                AddShieldRing();

            }

        }
    } 



    private void AddShieldRing()
    {
        currentShieldPoint += 1;

        coolDownTimer = coolDown;

        int ringIndex = shieldInstantiateList.Count;
        UpdateShiled(ringIndex);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemiesInRange.Add(collision.GetComponent<EnemyController>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(collision.GetComponent<EnemyController>());
        }
    }


    public bool ProtectPlayer()
    {
        if (currentShieldPoint == 0)
        {
            return false;
        }

        enemiesInRange.RemoveAll(enemy => enemy == null);

        // 对范围内的敌人造成 0 伤害但带 knockback
        foreach (EnemyController enemy in enemiesInRange)
        {
            if (enemy != null)
            {
                enemy.EnemyTakeDamage(0f, knockBackForce);
            }
        }

        // 消耗一个护盾点
        RemoveShield();

        return true;
    }



    private void RemoveShield()
    {
        AudioManager.instance.PlaySound(SoundType.Shield);

        currentShieldPoint -= 1;

        int lastIndex = shieldInstantiateList.Count - 1;

        GameObject ringToRemove = shieldInstantiateList[lastIndex];
        shieldInstantiateList.RemoveAt(lastIndex);
        Destroy(ringToRemove);

        playerInvincible = true;
    }

    private void UpdateShiled(int ringIndex)
    {
        float scale = initScale + intervalShield * ringIndex;

        GameObject shieldInstantiate = Instantiate(
            shieldPrefab,
            transform.position,
            Quaternion.identity,
            transform   // 设为玩家子物体，跟随移动
        );
        shieldInstantiate.transform.localScale = Vector3.one * scale;

        this.shieldInstantiateList.Add(shieldInstantiate);
    }


}
