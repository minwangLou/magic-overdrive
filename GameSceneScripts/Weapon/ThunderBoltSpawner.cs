using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderBoltSpawner : Weapon
{
    public GameObject ThunderBoltPrefab;


    private float _cooldownTimer;
    private List<EnemyController> _trackedEnemyControllers = new List<EnemyController>();

    public float offsetY;

    private void Update()
    {
        _cooldownTimer -= Time.deltaTime;
        if (_cooldownTimer <= 0f)
        {
            SpawnRandomThunderBolts();
            _cooldownTimer = coolDown;
        }
    }

    // ���ѡȡ�����еĵ��ˣ�ȡ������ Amount �������������ǵ�ǰλ�������׻���
    private void SpawnRandomThunderBolts()
    {
        // 1. ��ȫ�� EnemySpawner �б���һ�ݣ������޸�ԭʼ����
        List<GameObject> allEnemyGameObjects =
            new List<GameObject>(EnemySpawner.instance.enemyInstantiate);

        // 2. �����ѱ����ٻ�Ϊ null ������
        allEnemyGameObjects.RemoveAll(
            gameObjectCandidate => gameObjectCandidate == null);

        int totalEnemies = allEnemyGameObjects.Count;
        if (totalEnemies == 0)
        {
            return;
        }

        // 3.��������ʵ�����״��������������õ���������
        int strikeCount = amount <= totalEnemies ? amount : totalEnemies;

        // 4. ���ѡȡ strikeCount ������λ�������׻�
        for (int index = 0; index < strikeCount; index++)
        {
            int randomIndex = Random.Range(0, allEnemyGameObjects.Count);
            Vector3 chosenEnemyPosition = allEnemyGameObjects[randomIndex].transform.position;
            Vector3 chosenPosition = new Vector3(chosenEnemyPosition.x, chosenEnemyPosition.y + offsetY, 0);

            // ��Ŀ��λ�� Instantiate �׻� Prefab
            GameObject thunderBoltInstance = Instantiate(
                ThunderBoltPrefab,
                chosenPosition,
                Quaternion.identity
            );

            // ��ʼ���׻�����
            ThunderBolt damager = thunderBoltInstance.GetComponent<ThunderBolt>();
            damager.Initialize(damage, area, knockBackForce);


            // �Ӻ�ѡ�б����Ƴ�������ͬһѭ�����ظ�ѡ��
            allEnemyGameObjects.RemoveAt(randomIndex);
        }
    }
}
