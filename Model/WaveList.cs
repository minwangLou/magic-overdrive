using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Wave")]
public class WaveList : ScriptableObject
{
    public List<WaveInfo> waves;
} 

[System.Serializable]
public class WaveInfo
{
    public GameObject[] enemyListToSpawn;//El tipo de enemigo que aparece en un wave
    public float waveLength; // Tiempo total de un wave
    public float timeBetweenSpawns; //Tiempo que separa cada enemigos que aparecen
    public int maxNumberSpawn;
}
