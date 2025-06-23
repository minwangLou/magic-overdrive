using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    private Transform player;
    public GameObject defaultChunkPrefab;
    public int chunkSize;
    public int viewDistance;


    public int removeDistance; // 只有距离玩家 > removeDistance 的 Chunk 会被销毁
    public float checkInterval = 5f; // 每几秒检查一次
    private float checkTimer;

    private Dictionary<Vector2Int, GameObject> loadedChunks = new Dictionary<Vector2Int, GameObject>();
    private Vector2Int currentChunkCoord;


    public List<SpecialChunkEntry> specialChunks = new List<SpecialChunkEntry>();
    private Dictionary<Vector2Int, GameObject> specialChunkDict = new Dictionary<Vector2Int, GameObject>();

    private Transform _mapchunk;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        player = PlayerController.instance.gameObject.transform;
        _mapchunk = gameObject.transform;

        /*
        InicializeSpecialChunk();

        currentChunkCoord = GetChunkCoordFromPosition(player.position);
        UpdateChunksAround(currentChunkCoord);
        */

        // 启动协程，等 prefab 就绪后再初始化
        StartCoroutine(InitWhenPrefabReady());
    }

    private IEnumerator InitWhenPrefabReady()
    {
        // 等待 defaultChunkPrefab 被外部赋值（Inspector 或者异步加载）
        yield return new WaitUntil(() => defaultChunkPrefab != null);

        InicializeSpecialChunk();

        currentChunkCoord = GetChunkCoordFromPosition(player.position);
        UpdateChunksAround(currentChunkCoord);
    }


    void Update()
    {


        if (Time.timeScale == 0f) return;

        if (defaultChunkPrefab != null)
        {

            Vector2Int playerChunkCoord = GetChunkCoordFromPosition(player.position);

            if (playerChunkCoord != currentChunkCoord)
            {
                currentChunkCoord = playerChunkCoord;
                UpdateChunksAround(playerChunkCoord);
            }

            CleanupChunksCoroutine();

        }
    }

    private void InicializeSpecialChunk()
    {
        foreach (var entry in specialChunks)
        {
            if (!specialChunkDict.ContainsKey(entry.position))
                specialChunkDict.Add(entry.position, entry.chunkPrefab);
        }
    }


    Vector2Int GetChunkCoordFromPosition(Vector3 pos)
    {
        return new Vector2Int(
            (int)(pos.x / chunkSize),
            (int)(pos.y / chunkSize)
        );
    }

    void UpdateChunksAround(Vector2Int centerCoord)
    {
        for (int x = -viewDistance; x <= viewDistance; x++)
        {
            for (int y = -viewDistance; y <= viewDistance; y++)
            {
                Vector2Int newCoord = new Vector2Int(centerCoord.x + x, centerCoord.y + y);
                if (!loadedChunks.ContainsKey(newCoord))
                {

                    Vector3 worldPos = new Vector3(newCoord.x * chunkSize, newCoord.y * chunkSize, 0);

                    //GameObject chunk = Instantiate(chunkPrefab, worldPos, Quaternion.identity);
                    GameObject prefab = specialChunkDict.ContainsKey(newCoord) ? specialChunkDict[newCoord] : defaultChunkPrefab;

                    //GameObject chunk = Instantiate(prefab, worldPos, Quaternion.identity);
                    GameObject chunk = Instantiate(prefab, _mapchunk);

                    chunk.transform.position = worldPos;

                    loadedChunks.Add(newCoord, chunk);
                }
            }
        }
    }

    private void CleanupChunksCoroutine()
    {
        checkTimer += Time.deltaTime;
        if (checkTimer >= checkInterval)
        {
            checkTimer = 0f;
            CheckAndRemoveFarChunks();
        }
    }


    void CheckAndRemoveFarChunks()
    {
        Vector2Int playerCoord = GetChunkCoordFromPosition(player.position);
        List<Vector2Int> chunksToRemove = new List<Vector2Int>();

        foreach (var kvp in loadedChunks)
        {
            int distX = Mathf.Abs(kvp.Key.x - playerCoord.x);
            int distY = Mathf.Abs(kvp.Key.y - playerCoord.y);

            if (distX > removeDistance || distY > removeDistance)
            {
                Destroy(kvp.Value);
                chunksToRemove.Add(kvp.Key);
            }
        }

        foreach (var coord in chunksToRemove)
        {
            loadedChunks.Remove(coord);
        }
    }
}


[System.Serializable]
public class SpecialChunkEntry
{
    public Vector2Int position;
    public GameObject chunkPrefab;
}