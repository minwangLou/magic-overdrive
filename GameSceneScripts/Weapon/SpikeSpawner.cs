using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSpawner : Weapon
{
    public GameObject spikePrefab;

    [Header("Spawn Settings")]
    public int layers = 3;     // �ܹ����ɼ���
    public int countPerLayer = 8;     // ÿ��������
    public float layerSpacing = 1.5f;  // ÿ��֮��İ뾶����
    public float randomOffset;
    public float spawnInterval = 0f;     // ÿ��������ɼ����0 ��ʾÿ֡һ��

    private float cooldownTimer;
    


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
            randomOffset = Random.Range(0f, 360f);

            float radius = layerSpacing * (layer + 1);

            for (int i = 0; i < countPerLayer; i++)
            {
                float angle = (360f / countPerLayer) * i + randomOffset;
                float rad = angle * Mathf.Deg2Rad;
                Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * radius;

                // ʵ����������λ�ã�����
                GameObject spike = Instantiate(spikePrefab, spawnCenter + offset, Quaternion.identity);
                //spike.transform.position = spawnCenter + offset;

                // ���ü������
                var weapon = spike.GetComponent<SpikeWeapon>();
                weapon.Initialize(damage, damage*0.3f, duration, knockBackForce);


            }

            // �ȴ���һ������
            if (spawnInterval > 0f)
                yield return new WaitForSeconds(spawnInterval);
            else
                yield return null;
        }
    }

}
