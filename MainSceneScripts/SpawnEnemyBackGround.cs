using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyBackGround : MonoBehaviour
{
    public float spawnDistance;
    public List<GameObject> objectEnemy;

    public float spawnInterval;
    public float intervalChange;

    public float moveSpeed;
    public float stopDistance = 0.1f;

    public float distanceTarget;


    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnOneEnemy();
            yield return new WaitForSeconds(Random.Range(spawnInterval- intervalChange, spawnInterval + intervalChange));
        }
    }

    private void SpawnOneEnemy()
    {

        // ���ѡ�����Ԥ����
        GameObject prefab = objectEnemy[Random.Range(0, objectEnemy.Count)];

        // �����յ㣨������ͬ�߽磩
        int startSide = Random.Range(0, 4);
        int endSide = GetDestinationSide(startSide);

        Vector3 startPos = GetRandomSpawnPosition(startSide);
        Vector3 endPos = startPos;


        while (Vector3.Distance(startPos, endPos) < distanceTarget)
        {
            endPos = GetRandomSpawnPosition(endSide);
        }


        // ʵ��������
        GameObject enemy = Instantiate(prefab, startPos, Quaternion.identity);
        enemy.GetComponent<EnemyController>().enabled = false;
        enemy.transform.SetParent(transform, false); // ��֯����
        enemy.GetComponent<CapsuleCollider2D>().isTrigger = true;

        // �����ƶ�Э��
        StartCoroutine(MoveEnemy(enemy.GetComponent<Rigidbody2D>(), enemy, endPos));
    }

    private IEnumerator MoveEnemy(Rigidbody2D rbEnemy, GameObject enemy, Vector3 targetPos)
    {

        while (Vector3.Distance(enemy.transform.position, targetPos) > stopDistance)
        {
            Vector3 dir = (targetPos - enemy.transform.position).normalized;
            rbEnemy.velocity = dir * moveSpeed;
            yield return null;
        }

        rbEnemy.velocity = Vector2.zero; // ֹͣ�ƶ�
        Destroy(enemy);
    }



    private Vector3 GetRandomSpawnPosition(int startSide)
    {
        Camera cam = Camera.main;
        float camHeight = 2f * cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        Vector3 screenCenter = cam.transform.position;
        Vector3 spawnPosition;
        
        int side = GetDestinationSide(startSide);

        float offsetX = camWidth / 2 + spawnDistance;  // ��Ļ���X����ƫ��
        float offsetY = camHeight / 2 + spawnDistance; // ��Ļ���Y����ƫ��

        switch (side)
        {
            case 0: // ��
                spawnPosition = new Vector3(Random.Range(screenCenter.x - camWidth / 2, screenCenter.x + camWidth / 2), screenCenter.y + offsetY, 0);
                break;
            case 1: // ��
                spawnPosition = new Vector3(Random.Range(screenCenter.x - camWidth / 2, screenCenter.x + camWidth / 2), screenCenter.y - offsetY, 0);
                break;
            case 2: // ��
                spawnPosition = new Vector3(screenCenter.x - offsetX, Random.Range(screenCenter.y - camHeight / 2, screenCenter.y + camHeight / 2), 0);
                break;
            default: // ��
                spawnPosition = new Vector3(screenCenter.x + offsetX, Random.Range(screenCenter.y - camHeight / 2, screenCenter.y + camHeight / 2), 0);
                break;
        }

        return spawnPosition;
    }



    private int GetDestinationSide(int startSide)
    {
        int[] possibleSides = { 0, 1, 2, 3 };
        List<int> sidesList = new List<int>(possibleSides);

        if (startSide != -1)
        {
            sidesList.Remove(startSide);
        }

        return sidesList[Random.Range(0, sidesList.Count)];
    }
}
