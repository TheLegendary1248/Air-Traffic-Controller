using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    //Main instance
    public static World Main;
    Bounds worldBorder = new Bounds(new Vector3(50,50,0), new Vector3(100,100,2));
    public float slope;
    public float height;
    public void Start()
    {
        Main = this;
        calcrotation = transform.eulerAngles.x;
    }
    //C
    public bool GetTerrainHeight(Vector2 vec, out float result)
    {
        Vector2 m = ToTerrainCoord(vec);
        
        result = 0;
        if (!worldBorder.Contains(m)) return false;
        m /= 10f;
        result = Mathf.Pow(Mathf.Abs(ShaderFunctions.ClassicNoise(new Vector3(m.x,m.y,Time.fixedTime / 35f))), slope) * height * 100f;
        return true; 
    }
    //Precalc for rotation transformation in the next function
    float calcrotation = -45f;
    float sin45 = Mathf.Sin(Mathf.Deg2Rad * -45f);
    float cos45 = Mathf.Cos(Mathf.Deg2Rad * -45f);
    /// <summary>
    /// Gets the 'terrain' coordinates from world coordinates to match with the shader noise
    /// </summary>
    public Vector2 ToTerrainCoord(Vector2 vec)
    {
        if (transform.eulerAngles.x != calcrotation) 
        {
            calcrotation = transform.eulerAngles.x ;
            sin45 = Mathf.Sin(Mathf.Deg2Rad * calcrotation); 
            cos45 = Mathf.Cos(Mathf.Deg2Rad * calcrotation); 
        }
        return new Vector2((vec.x * cos45) - (vec.y * sin45), (vec.x * sin45) + (vec.y * cos45)); 
    }
}
