using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Derivative of World that uses a singula terrain chunk as it's entirety
/// </summary>
public class MonoWorld : World
{
    public TerrainChunk terrain;
    public override Vector2 scale => terrain.scale;
    public override Vector2 origin => terrain.origin;
    public override Vector3 offset => terrain.offset;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        //Set values from inspector
        this.scale = _scale;
        Debug.Log($"Scale of thing {scale.x}, {scale.y}");
        this.origin = _origin;
        offset = _offset;
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        offset = new Vector3(0, 0, Time.fixedTime / 50f);
    }
    public override bool GetTerrainHeight(Vector2 vec, out float result)
    {
        result = 0;
        if (WithinBorder(vec))
        {
            result = terrain.GetTerrainHeight(vec);
            return true;
        }
        else return false;
    }
    public override bool WithinBorder(Vector2 vec) => base.WithinBorder(vec);
}
