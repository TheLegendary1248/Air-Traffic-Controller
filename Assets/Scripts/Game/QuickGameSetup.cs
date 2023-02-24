using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class QuickGameSetup : MonoBehaviour
{
    public static QuickGameSettings setting;
    public MonoWorld world;
    public Spawner spawner;


    public void SetSizeX(float i)
    {

        setting.terrainSize = new Vector2(i, setting.terrainSize.x); 
    }

    public void SetSizeY(float i) => setting.terrainSize = new Vector2(setting.terrainSize.x, i);
    public void SetRate(float s) => setting.SpawnRate = s;
    public void SetRateGrowth(float s) => setting.SpawnRateGrowth = s;
    public void SetRateMax(float s) => setting.MaxSpawnRate = s;
    //Figure out Airport Setup later
    public void SetTerrainChangeScroll(float i) => setting.TerrainZScrollSpeed = i;
}
public struct QuickGameSettings
{
    /// <summary>
    /// Speed at which the terrain scrolls through Z
    /// </summary>
    public float TerrainZScrollSpeed;
    public float SpawnRate;
    public float MaxSpawnRate;
    public float SpawnRateGrowth;
    public float MaxRating;
    public string AirportSetup;
    public Vector2 terrainSize;
}
