using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Derivative of World that uses a singular terrain chunk as it's entirety
/// </summary>
public class MonoWorld : World
{
    public TerrainChunk terrain;
    public new Vector2 scale
    {
        get => terrain.scale;
        set => terrain.scale = value;
    }
    public new Vector2 origin
    {
        get => terrain.origin;
        set => terrain.origin = value;
    }
    public new Vector3 offset
    {
        get => terrain.offset;
        set => terrain.offset = value;
    }
    public override void Awake() => base.Awake();
    public override void Start()
    {
        //Set values from inspector
        scale = _scale;
        origin = _origin;
        offset = _offset;
    }
    private void FixedUpdate()
    {
        //Gradually move along z to transform world noise
        offset = new Vector3(0, 0, Time.fixedTime / 50f);
    }
    public override bool GetTerrainHeight(Vector3 pos, out float result)
    {
        
        pos = AlignToWorld(pos);
        result = 0;
        if (WithinBorderRaw(pos))
        {
            result = terrain.GetTerrainHeight(pos / 100f);
            return true;
        }
        else return false;
    }
    public override bool WithinBorder(Vector2 vec) => base.WithinBorder(vec);
}
