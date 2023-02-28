using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class QuickGameSetup : MonoBehaviour
{
    public MonoWorld world;
    public Spawner spawner;
    public NumberSliderSync widthInput;
    public NumberSliderSync lengthInput;
    public NumberSliderSync zOffsetInput;
    public void Awake()
    {
        widthInput.onValueChange += SetTerrainWidth;
        lengthInput.onValueChange += SetTerrainLength;
    }
    public void SetTerrainWidth(float val)
    {
        world.scale = new Vector2(world.scale.x, val);
    }
    public void SetTerrainLength(float val)
    {
        world.scale = new Vector2(val, world.scale.y);
    }
    public void SetTerrainScale(float val)
    {
        world.scale = new Vector2(val, val);
    }
    
    
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
