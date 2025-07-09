using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapButtom : MonoBehaviour, IPointerDownHandler
{
    public static MapButtom instance;

    [HideInInspector]public MapData mapSelected;

    public GameObject _arrowImage;
    [HideInInspector]public bool mapConfirmed;

    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        _arrowImage.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        if (mapSelected != null)
        {
            AudioManager.instance.PlaySound(SoundType.ButtonClick);

            if (mapConfirmed == false)
            {
                mapConfirmed = true;
                _arrowImage.SetActive(true);
            }
            else
            {
                Debug.Log("MapBGM: " + mapSelected.bgmType.ToString());
                AudioManager.instance.PlayBGM(mapSelected.bgmType);

                GameManager.instance.mapChunk = Resources.Load<GameObject>(mapSelected.mapChunk_location);
                GameManager.instance.ChangeScene("2-GameScene");
            }
        }
    }

    public void SelectAnotherMap(MapData selectMapData)
    {
        if (mapSelected != selectMapData)
        {
            _arrowImage.SetActive(false);
            mapConfirmed = false;
        }
    }

    public void RetunToRolePanel()
    {
        SelectAnotherMap(null);
    }

}
