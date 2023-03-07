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
    /// <summary>
    /// Placeholder for quickly setting terrain dimensions for reasons
    /// </summary>
    public new Vector2 dimensions
    {
        set
        {
            terrain.scale = value;
            float HARDCODE = 1f; // 10f
            transform.localScale = new Vector3(value.x * scale.x, transform.localScale.y, value.y * scale.y);
            worldBorder = new Rect(Vector2.zero, new Vector2(value.x * scale.x, value.y * scale.y));
        }
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
        
        //HARDCODED
        //Gradually move along z to transform world noise
        if(!GameManager.gameOver) offset = new Vector3(0, 0, Time.fixedTime / 50f);
    }
    /// <summary>
    /// Gets terrain height of the world
    /// </summary>
    /// <param name="pos">The point to sample</param>
    /// <param name="result">The height of the terrain at 'pos'</param>
    /// <returns></returns>
    public override bool GetTerrainHeight(Vector3 pos, out float result)
    {
        float HARDCODE = 1f;//100f;
        //Fix up scaling
        Vector3 parentScale = Main.transform.localScale;
        //
        pos = AlignToWorld(pos);
        result = 0;
        if (WithinBorderRaw(pos))
        {
            pos.Scale(new Vector3(1 / scale.x, 1 / scale.y, 1f));
            result = terrain.GetTerrainHeight(pos / HARDCODE) * Main.transform.localScale.y;
            return true;
        }
        else return false;
    }
    public override bool WithinBorder(Vector2 vec) => base.WithinBorder(vec);
}
