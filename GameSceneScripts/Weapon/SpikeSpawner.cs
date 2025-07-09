using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSpawner : Weapon
{
    public GameObject spikePrefab;

    [Header("Spawn Settings")]
    public int layers = 3;     // 总共生成几层
    public float layerSpacing = 1.5f;  // 每层之间的半径增量
    public float spawnInterval = 0f;     // 每条尖刺生成间隔，0 表示每帧一条

    private float cooldownTimer;

    private void Start()
    {
        cooldownTimer = coolDown;
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0f)
        {
            Vector3 spawnCenter = transform.position;
            StartCoroutine(SpawnSpikes(spawnCenter));
            cooldownTimer = coolDown;
        }
    }

    private IEnumerator SpawnSpikes(Vector3 spawnCenter)
    {
        float randomOffset;

        for (int layer = 0; layer < layers; layer++)
        {
            AudioManager.instance.PlaySound(SoundType.Spike);

            randomOffset = Random.Range(0f, 360f);

            float radius = (area*2f) * (layer + 1);

            for (int i = 0; i < amount; i++)
            {
                float angle = (360f / amount) * i + randomOffset;
                float rad = angle * Mathf.Deg2Rad;
                Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * radius;

                // 实例化并设置位置
                GameObject spike = Instantiate(spikePrefab, spawnCenter + offset, Quaternion.identity);

                // 配置尖刺属性
                var weapon = spike.GetComponent<SpikeWeapon>();
                weapon.Initialize(damage, damage*0.3f, duration, knockBackForce);


            }

            // 等待下一次生成
            if (spawnInterval > 0f)
                yield return new WaitForSeconds(spawnInterval);
            else
                yield return null;
        }
    }

    public override void UpdateWeaponStats()
    {
        layers = Mathf.Max(1, Mathf.CeilToInt(amount / 3f));
        if (layers > 3)
        {
            layers = 3;
        }
    }

}
