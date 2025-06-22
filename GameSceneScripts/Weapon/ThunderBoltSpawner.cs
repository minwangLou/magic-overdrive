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

    // 随机选取场景中的敌人，取不超过 Amount 个，依次在它们当前位置生成雷击。
    private void SpawnRandomThunderBolts()
    {
        // 1. 从全局 EnemySpawner 列表复制一份，避免修改原始数据
        List<GameObject> allEnemyGameObjects =
            new List<GameObject>(EnemySpawner.instance.enemyInstantiate);

        // 2. 清理已被销毁或为 null 的引用
        allEnemyGameObjects.RemoveAll(
            gameObjectCandidate => gameObjectCandidate == null);

        int totalEnemies = allEnemyGameObjects.Count;
        if (totalEnemies == 0)
        {
            return;
        }

        // 3.决定本次实际落雷次数（不超过可用敌人数量）
        int strikeCount = amount <= totalEnemies ? amount : totalEnemies;

        // 4. 随机选取 strikeCount 个敌人位置生成雷击
        for (int index = 0; index < strikeCount; index++)
        {
            int randomIndex = Random.Range(0, allEnemyGameObjects.Count);
            Vector3 chosenEnemyPosition = allEnemyGameObjects[randomIndex].transform.position;
            Vector3 chosenPosition = new Vector3(chosenEnemyPosition.x, chosenEnemyPosition.y + offsetY, 0);

            // 在目标位置 Instantiate 雷击 Prefab
            GameObject thunderBoltInstance = Instantiate(
                ThunderBoltPrefab,
                chosenPosition,
                Quaternion.identity
            );

            // 初始化雷击参数
            ThunderBolt damager = thunderBoltInstance.GetComponent<ThunderBolt>();
            damager.Initialize(damage, area, knockBackForce);


            // 从候选列表中移除，避免同一循环内重复选中
            allEnemyGameObjects.RemoveAt(randomIndex);
        }
    }
}
