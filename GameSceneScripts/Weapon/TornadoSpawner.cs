using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoSpawner : Weapon
{
    public GameObject TornadoPrefab;

    private float cooldownTimer = 0f;


    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0f)
        {
            SpawnWhirlwinds();
            cooldownTimer = coolDown;
        }
    }

    private void SpawnWhirlwinds()
    {
        for (int i = 0; i < amount; i++)
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;

            GameObject tornadoInstantiate = Instantiate(TornadoPrefab, transform.position, Quaternion.identity);
            tornadoInstantiate.transform.localScale = Vector3.one * area;

            // 自动销毁
            Destroy(tornadoInstantiate, duration);

            // 传入攻击和移动参数
            var damager = tornadoInstantiate.GetComponent<TornadoWeapon>();
            damager.weaponDamage = might;
            damager.moveDirection = randomDirection;
            damager.moveSpeed = speed;
        }
    }
}

