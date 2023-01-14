using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Derivative of World that functions by tiling world pieces
/// </summary>
public class TiledWorld : World
{
    public Vector2Int resolution;
    public GameObject defaultTile;
    public Dictionary<Vector2Int, GameObject> refs;
    public TerrainChunk[] terrain;
    [Tooltip("Whether to keep all chunks in sync with the world as a continous whole")]
    public bool keepSync;

    
    public override bool GetTerrainHeight(Vector3 vec, out float result)
    {
        if (keepSync)
        {
            result = 0;
            return true;
        }
        else throw new System.NotImplementedException();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
