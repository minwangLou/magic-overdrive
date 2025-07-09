using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class MapUI : MonoBehaviour, IPointerDownHandler
{
    public MapData mapData;

    [HideInInspector] public MapSelectPanel mapPanel;

    public Image _mapIcon;
    public TextMeshProUGUI _mapName;
    public TextMeshProUGUI _mapDescription;

    public Color normalColor;
    public Color colorSelect;

    private void Awake()
    {
        mapPanel = MapSelectPanel.instance;
    }


    public void setMapData(MapData mapData)
    {
        this.mapData = mapData;

        if (mapData.unlock != 0) //map unlock
        {
            _mapIcon.sprite = Resources.Load<Sprite>(mapData.mapIcon);
            _mapName.text = mapData.mapName;
            _mapDescription.text = mapData.mapDescription;
        }
        else // map locked
        {

        }
    }


    public void RenewUi(MapData mapData)
    {
        if (mapData.unlock != 0) //map unlock
        {
            mapPanel._mapDetailName.text = mapData.mapName;
            mapPanel._mapDetailContent.text = GenerateMapDetail(mapData);

            MapButtom.instance.mapSelected = mapData;

        }
        else //map lock
        {

        }

        MapSelected();
    }

    private string GenerateMapDetail(MapData mapData)
    {
        string result = TimeSpan.FromSeconds(mapData.timeLimitInSeconds).ToString(@"mm\:ss")
                + "\n" + mapData.clockSpeed.ToString()
                + "\n" + formatPercent(mapData.moveSpeed_plus)

                + "\n\n" + formatPercent(mapData.goldBonus_plus)
                + "\n" + formatPercent(mapData.xpBonus_plus)

                + "\n\n" + formatPercent(mapData.enemyHelth_plus);

        return result;
    }

    private string formatPercent(float value)
    {
        int percent = Mathf.RoundToInt(Mathf.Abs(value) * 100);
        return value >= 0 ? $"+{percent}%" : $"-{percent}";
    }


    public void OnPointerDown(PointerEventData eventData)
    {

        if (MapButtom.instance.mapConfirmed == true)
        {
            MapButtom.instance.SelectAnotherMap(mapData);
        }
        AudioManager.instance.PlaySound(SoundType.ButtonClick);
        RenewUi(mapData);

    }

    //Mostrar al jugador qu¨¦ mapa ha seleccionado.
    private void MapSelected()
    {
        if (mapPanel.mapUISelect != null)
        {
            mapPanel.mapUISelect.ReturnNormalColor();
        }
        gameObject.GetComponent<Image>().color = colorSelect;
        mapPanel.mapUISelect = this;
    }

    public void ReturnNormalColor()
    {
        gameObject.GetComponent<Image>().color = normalColor;
    }
}
