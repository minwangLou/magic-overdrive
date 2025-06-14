using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;


    public List<RoleData> roleDatas;
    public List<MapData> mapDatas;
    public List<BonusData> bonusDatas;
    public CoinData coinData;

    //Json document save in Resource folder
    public TextAsset roleTextAsset;
    public TextAsset mapTextAsset;
    public TextAsset bonusTextAsset;
    public TextAsset coinTextAsset;

    //File Path which save file under Application.persistentDataPath+ "\Data" path
    private string folderPath;
    private string roleFilePath;
    private string mapFilePath;
    private string bonusFilePath;
    private string coinFilePath;

    private void Awake()
    {
        instance = this;

        FileInit();
    }

    private void FileInit()
    {

        folderPath = Path.Combine(Application.persistentDataPath, "Data");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        roleFilePath = Path.Combine(folderPath, roleTextAsset.name + ".json");
        mapFilePath = Path.Combine(folderPath, mapTextAsset.name + ".json");
        bonusFilePath = Path.Combine(folderPath, bonusTextAsset.name + ".json");
        coinFilePath = Path.Combine(folderPath, coinTextAsset.name + ".json");

        InitAndLoad(roleTextAsset, ref roleDatas, roleFilePath);
        if (roleDatas[0] != null)
        {
            roleDatas.Insert(0, null);
        }
        

        InitAndLoad(mapTextAsset, ref mapDatas, mapFilePath);
        if (mapDatas[0] != null)
        {
            mapDatas.Insert(0, null);
        }
        

        InitAndLoad(bonusTextAsset, ref bonusDatas, bonusFilePath);
        if (bonusDatas[0] != null)
        {
            bonusDatas.Insert(0, null);
        }
        

        InitAndLoad(coinTextAsset, ref coinData, coinFilePath);
    }



    //Crear en un directorio de carpeta un documento, leerla y trasformar en datos correspondientes.
    private void InitAndLoad<T>(TextAsset textAsset, ref T dataHolder, string filePath)
    {

        //No encuentra archivo de directorio, crear carpeta de archivo
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, textAsset.text);
            Debug.Log("Folder create in path£º" + filePath);
        }

        //leer el documento y trasformar en datos correspondiente
        string json = File.ReadAllText(filePath);
        dataHolder = JsonConvert.DeserializeObject<T>(json);

        Debug.Log(textAsset.name + " Data load success£º" + filePath);
    }
    
    //Save Role Data
    public void SaveRoleData()
    {
        string json = JsonConvert.SerializeObject(roleDatas, Formatting.Indented);
        File.WriteAllText(roleFilePath, json);
        Debug.Log("Data save success: " + roleFilePath);
    }

   //Save Map data
    public void SaveMapData()
    {
        string json = JsonConvert.SerializeObject(mapDatas, Formatting.Indented);
        File.WriteAllText(mapFilePath, json);
        Debug.Log("Data save success: " + mapFilePath);
    }

    //Save Bonus Data
    public void SaveBonusData()
    {
        string json = JsonConvert.SerializeObject(bonusDatas, Formatting.Indented);
        File.WriteAllText(bonusFilePath, json);
        Debug.Log("Data save success: " + bonusFilePath);
    }

    //Save Coin Data
    public void SaveCoinData()
    {
        string json = JsonConvert.SerializeObject(coinData, Formatting.Indented);
        File.WriteAllText(coinFilePath, json);
        Debug.Log("Data save success: " + coinFilePath);
    }


    //Save all data in persistent path folder
    public void SaveAllData()
    {
        SaveRoleData();
        SaveMapData();
        SaveBonusData();
        SaveCoinData();
    }


}
