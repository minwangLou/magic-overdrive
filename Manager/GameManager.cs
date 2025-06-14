using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System.Reflection;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public RoleData roleSelected;

    public MapData mapData;

    public int coinObtainInGame;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log(Application.persistentDataPath);
        
    }

    /*
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "2-GameScene")
        {
            StartGame();
        }
    }
    */
    
    public void MapSelection()
    {
        mapData = MapSelectPanel.instance.mapDataSelect;
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        /*
        if (sceneName.Equals("2-GameScene"))
        {
            StartGame();
        }
        */
    }
    

    public void RegisterGameManager()
    {
        StartGame();
    }

    //Se ejecuta despúes de cambiar el escenario al GameScene
    public void StartGame()
    {
        //añadir la generación de mapa

        //Según el role seleccionado, calcular sus atributos que va a usar en el juego
        AttributeManager.instance.SetUpRoleData(roleSelected);

        //Instanciar el jugador y su arma en el partido
        PlayerController.instance.IniciateRoleData();

        MapManager.instance.defaultChunkPrefab = Resources.Load<GameObject>(mapData.mapChunk_location);

    }

    public void EndGame()
    {
        coinObtainInGame = CoinController.instance.currentCoins;
        SceneManager.LoadScene("1-MainScene");

    }

    public void UpdateCoinAmount()
    {
        CoinManager.instance.AddCoin(coinObtainInGame);
    }





}
