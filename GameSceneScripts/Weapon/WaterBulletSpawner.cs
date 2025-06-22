using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBulletSpawner : Weapon
{
    [Header("Spawner Settings")]
    public GameObject waterBulletPrefab;

    private float cooldownTimer = 0f;
    private List<EnemyController> trackedEnemies = new List<EnemyController>();

    void Update()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0f)
        {
            FireBullets();
            cooldownTimer = coolDown;
        }
    }

    private void FireBullets()
    {
        // 1. 清理已被销毁的引用
        trackedEnemies.RemoveAll(e => e == null);

        if (trackedEnemies.Count == 0)
            return;


        // 2. 按与发射点距离排序，取前 amount 个
        Vector3 spawnOrigin = transform.position;
        var sorted = new List<EnemyController>(trackedEnemies);
        sorted.Sort((firstEnemy, secondEnemy) =>
        {
            float firstDistanceSquared = Vector3.SqrMagnitude(firstEnemy.transform.position - spawnOrigin);
            float secondDistanceSquared = Vector3.SqrMagnitude(secondEnemy.transform.position - spawnOrigin);
            return firstDistanceSquared.CompareTo(secondDistanceSquared);
        });

        for (int i = 0; i < amount; i++)
        {
            // 目标敌人索引（0..sorted.Count-1）
            int index = i < sorted.Count ? i : i % sorted.Count;
            Vector3 targetPosition = sorted[index].transform.position;
            SpawnBullet(spawnOrigin, targetPosition);
        }

    }

    private void SpawnBullet(Vector3 spawnOrigin, Vector3 targetPosition)
    {
        Vector2 direction = (targetPosition - spawnOrigin).normalized;
        GameObject bulletInstance = Instantiate(
            waterBulletPrefab,
            spawnOrigin,
            Quaternion.identity
        );
        WaterBullet damager = bulletInstance.GetComponent<WaterBullet>();
        damager.Initialize(direction, speed, damage, knockBackForce, duration);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null && !trackedEnemies.Contains(enemy))
                trackedEnemies.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponent<EnemyController>();
            trackedEnemies.Remove(enemy);
        }
    }

}
