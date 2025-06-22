using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;

    public int maxNumberSpawn;
    private int currentEnemyNumber;//para test, se puede borrar luego


    public float spawnDistance = 10f;
    public float spawnCounter = 0.1f;
    //private float currentSpawnCounter;

    private List<GameObject> enemyList = new List<GameObject>();
    private List<GameObject> bossList = new List<GameObject>();

    [HideInInspector]public List<GameObject> enemyInstantiate;

    public int enemyCheckPerFrame;
    private int enemyToCheck;
    public float despawnDistance;

    private GameObject player;

    public WaveList waveList;
    private List<WaveInfo> waves;

    private int currentWave;
    private float waveCounter;//Contador tiempo del wave acutual

    public Transform _enemyList;

    private List<EnemyData> enemyDataList;
    private List<EnemyData> bossDataList;
    public TextAsset enemyDataTextAsset;
    public TextAsset bossDataTextAsset;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        player = PlayerHealthController.instance.gameObject;

        SetUpEnemyAttributeData();

        waves = waveList.waves;
    }


    // Start is called before the first frame update
    void Start()
    {

        currentWave = -1;
        GoToNextWave();
    }

    private void SetUpEnemyAttributeData()
    {
        enemyDataList = JsonConvert.DeserializeObject<List<EnemyData>>(enemyDataTextAsset.text);
        if (enemyDataList[0] != null)
        {
            enemyDataList.Insert(0,null);
        }

        bossDataList = JsonConvert.DeserializeObject<List<EnemyData>>(bossDataTextAsset.text);
        if (bossDataList[0] != null)
        {
            bossDataList.Insert(0,null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        SpawnEnemy();

        DespawnEnemyFar();
        
        if (bossList.Count > 0)
        {
            UpdateBossLocation();
        }
        
        currentEnemyNumber = enemyList.Count; // solo para ver los numeros de enemies actuales, puede borrar
    }


    private void SpawnEnemy()
    {
        if (PlayerHealthController.instance.gameObject.activeSelf)//comprobar si jugador está vivo o no
        {
            if (currentWave < waves.Count)
            {

                waveCounter -= Time.deltaTime;
                if (waveCounter <= 0)
                {
                    GoToNextWave();
                }

                spawnCounter -= Time.deltaTime;
                if (spawnCounter <= 0  && enemyList.Count < maxNumberSpawn)
                {
                    spawnCounter = waves[currentWave].timeBetweenSpawns;

                    Vector2 spawnPosition = GetRandomSpawnPosition();

                    
                    GameObject newEnemy = Instantiate(selectEnemieToSpawn(), _enemyList);// = Instantiate(waves[currentWave].enemyToSpawn, _enemyList);
                    enemyInstantiate.Add(newEnemy);

                    newEnemy.transform.position = spawnPosition;

                    EnemyController aux = newEnemy.GetComponent<EnemyController>();


                    
                    if (aux.enemyBoss == false)
                    {
                        aux.SetEnemyAttribute(enemyDataList[aux.id]);

                        enemyList.Add(newEnemy);
                    }
                    else
                    {
                        aux.SetEnemyAttribute(bossDataList[aux.id]);


                        bossList.Add(newEnemy);
                        GoToNextWave();
                    }
                    
                    
                }

            }
        }
    }


    private GameObject selectEnemieToSpawn()
    {
        int listLenght = waves[currentWave].enemyListToSpawn.Length;

        if (listLenght > 1)
        {
            int randomValue = Random.Range(0, listLenght);
            return waves[currentWave].enemyListToSpawn[randomValue];
        }
        else
        {
            return waves[currentWave].enemyListToSpawn[0];
        }

    }

    private void UpdateBossLocation()
    {
        

        foreach (GameObject boss in bossList)
        {

            
            if (boss != null && CheckEnemyFarToPlayer(boss))
            {
                ChangePositionBoss(boss);
            }
        }
    }

    private void ChangePositionBoss(GameObject boss)
    {
        if (PlayerHealthController.instance.gameObject.activeSelf)
        {
            Vector2 toPosition = GetRandomSpawnPosition();
            boss.transform.position = toPosition;
        }
    }


    //Metodo principal para eliminar los enemigos que está lejos del jugador
    private void DespawnEnemyFar()
    {   

        for (int i = enemyToCheck; i < enemyToCheck + enemyCheckPerFrame; i++)
        {
            if (i < enemyList.Count)
            {
                if (enemyList[i] != null)
                {
                    if (CheckEnemyFarToPlayer(enemyList[i]))
                    {
                        Destroy(enemyList[i].gameObject);
                        enemyList.RemoveAt(i);
                        i--;
                    }

                }
                else
                {
                    enemyList.RemoveAt(i);
                    i--;
                }

            }
            else
            {
                enemyToCheck = 0;
            }
        }
    }

    //Destruir el enemigo si está muy lejos del jugador, fuera del pantalla
    //Parámetro entrante, posición del dicho enemigo
    //Devolver true si ha destruido enemigo para borrarlo de la lista
    //Devolver false si no ha destruido el enemigo
    private bool CheckEnemyFarToPlayer(GameObject enemy)
    {

        if (player != null)
        {
            Vector3 positionPlayer = player.transform.position;
            Vector3 positionEnemy = enemy.transform.position;

            if (Vector3.Distance(positionPlayer, positionEnemy) > despawnDistance)
            {
                //Destroy(enemy);
                return true;
            }
        }

        return false;



    }


    private Vector2 GetRandomSpawnPosition()
    {
        Camera cam = Camera.main;
        float camHeight = 2f * cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        Vector3 screenCenter = cam.transform.position;
        Vector3 spawnPosition;

        int side = Random.Range(0, 4);
        float offsetX = camWidth / 2 + spawnDistance;  // 屏幕外的X方向偏移
        float offsetY = camHeight / 2 + spawnDistance; // 屏幕外的Y方向偏移

        switch (side)
        {
            case 0: // 上
                spawnPosition = new Vector3(Random.Range(screenCenter.x - camWidth / 2, screenCenter.x + camWidth / 2), screenCenter.y + offsetY, 0);
                break;
            case 1: // 下
                spawnPosition = new Vector3(Random.Range(screenCenter.x - camWidth / 2, screenCenter.x + camWidth / 2), screenCenter.y - offsetY, 0);
                break;
            case 2: // 左
                spawnPosition = new Vector3(screenCenter.x - offsetX, Random.Range(screenCenter.y - camHeight / 2, screenCenter.y + camHeight / 2), 0);
                break;
            default: // 右
                spawnPosition = new Vector3(screenCenter.x + offsetX, Random.Range(screenCenter.y - camHeight / 2, screenCenter.y + camHeight / 2), 0);
                break;
        }

        return spawnPosition;
    }


    private void GoToNextWave()
    {
        currentWave++;

        if (currentWave >= waves.Count)//Ha llegado al final de la lista de wave
        {
            currentWave = waves.Count - 1;
        }

        waveCounter = waves[currentWave].waveLength;
        spawnCounter = waves[currentWave].timeBetweenSpawns;
        maxNumberSpawn = waves[currentWave].maxNumberSpawn;
    }

}






