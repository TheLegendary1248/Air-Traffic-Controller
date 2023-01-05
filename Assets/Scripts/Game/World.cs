using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    //Chunks
    public float GetTerrainHeight(Vector2 vec) => GetTerrainHeight((Vector3)vec);
    public float GetTerrainHeight(Vector3 vec) => 0;
}
