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

    public GameObject mapChunk;

    public int coinObtainInGame;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        //Debug.Log(Application.persistentDataPath);

    }

    private void Start()
    {
        AudioManager.instance.PlayBGM(BGMType.MainMenu);
    }


    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

    }
    

    public void RegisterGameManager()
    {
       StartGame();
    }

    //Se ejecuta despúes de cambiar el escenario al GameScene
    public void StartGame()
    {
        //añadir la generación de mapa
        MapManager.instance.defaultChunkPrefab = mapChunk;

        //Según el role seleccionado, calcular sus atributos que va a usar en el juego
        AttributeManager.instance.SetUpRoleData(roleSelected);

        //Instanciar el jugador y su arma en el partido
        PlayerController.instance.IniciateRoleData();

        

    }

    public void EndGame()
    {
        coinObtainInGame = CoinController.instance.currentCoins;
        SceneManager.LoadScene("1-MainScene");
        Time.timeScale = 1f;
        Destroy(gameObject);

    }

    public void UpdateCoinAmount()
    {
        CoinManager.instance.AddCoin(coinObtainInGame);
    }





}
