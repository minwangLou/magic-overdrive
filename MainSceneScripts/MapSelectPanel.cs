using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;

public class MapSelectPanel : MonoBehaviour
{
    public static MapSelectPanel instance;

    public CanvasGroup _canvasGroup;

    public List<MapData> mapDatas; //Map data info

    public Transform _mapList;
    public GameObject mapPrefab;

    public TextMeshProUGUI _mapDetailName;
    public TextMeshProUGUI _mapDetailContent;

    public CanvasGroup _scrobalCanva;

    [HideInInspector] public MapUI mapUISelect;

    public MapData mapDataSelect;


    private void Awake()
    {
        instance = this;

    }


    private void Start()
    {
        mapDatas = SaveManager.instance.mapDatas;

        //Disable Scrobar Vertical when number of maps are lower than 3.
        if (mapDatas.Count <= 3)
        {
            _scrobalCanva.alpha = 0;
        }
        else
        {
            _scrobalCanva.alpha = 1;
        }
        
        foreach (MapData mapData in mapDatas)
        {
            if (mapData != null)
            {
                MapUI mapUI = Instantiate(mapPrefab, _mapList).GetComponent<MapUI>();


                mapUI.setMapData(mapData);

                if (mapData.id == 1)
                {
                    mapUI.RenewUi(mapData);
                }
            }


        }

        
    }






}
