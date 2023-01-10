using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Generic Class that all 'worlds' should maintain
/// </summary>
public class World : MonoBehaviour, ITerrain
{
    //Main instance
    public static World Main;
    public Rect worldBorder;
    public float slope;
    public float height;
    public Terraformer terraformer;
    //Values to expose in the inspector for fine tuning
    [SerializeField]
    protected Vector2 _scale;
    [SerializeField]
    protected Vector2 _origin;
    [SerializeField]
    protected Vector3 _offset;
    public Vector2 scale { get; set; }
    public Vector2 origin { get; set; }
    public Vector3 offset { get; set; }
    public virtual void Start()
    {

    }
    public virtual void Awake()
    {
        Main = this;
        calcrotation = transform.eulerAngles.x;
    }
    /// <summary>
    /// Gets the terrain height at position
    /// </summary>
    /// <param name="pos">Position in world</param>
    /// <param name="result">Height at given position</param>
    /// <returns>If given position is above any kind of valid terrain</returns>
    public virtual bool GetTerrainHeight(Vector2 pos, out float result)
    {
        //Transform into terrain coordinates
        Vector2 m = ToTerrainCoord(pos);
        
        result = 0;
        if (!worldBorder.Contains(m)) return false;
        result = Mathf.Pow(Mathf.Abs(ShaderFunctions.ClassicNoise(new Vector3(m.x,m.y,Time.fixedTime / 35f))), slope) * height * 100f;
        return true; 
    }
    /// <summary>
    /// Returns if the position is within world border
    /// </summary>
    /// <param name="vec"></param>
    /// <returns></returns>
    public virtual bool WithinBorder(Vector2 vec) => worldBorder.Contains(vec);
    //Precalc for rotation transformation in the next function
    protected float calcrotation = -45f;
    float sinRot = Mathf.Sin(Mathf.Deg2Rad * -45f);
    float cosRot = Mathf.Cos(Mathf.Deg2Rad * -45f);
    
    /// <summary>
    /// Gets the 'terrain' coordinates from world coordinates to match with the shader noise
    /// </summary>
    public Vector2 ToTerrainCoord(Vector2 vec)
    {
        if (transform.eulerAngles.x != calcrotation)
        {   //If the world's rotation doesn't match the already calculated values, update them
            calcrotation = transform.eulerAngles.x;
            sinRot = Mathf.Sin(Mathf.Deg2Rad * calcrotation);
            cosRot = Mathf.Cos(Mathf.Deg2Rad * calcrotation);
        }
        //Calculate rotation
        return new Vector2((vec.x * cosRot) - (vec.y * sinRot), (vec.x * sinRot) + (vec.y * cosRot)); 
    }
}
//Interface for all forms of terrain
public interface ITerrain
{
    /// <summary>
    /// Scale of the noise generator
    /// Determines the frequency of the noise
    /// </summary>
    public Vector2 scale { get; set; }
    /// <summary>
    /// Origin of the noise generator
    /// Determines what point in space is used as 0,0 for scale and origin
    /// NOT IMPLEMENTED
    /// SHOULD NOT AFFECT 'OFFSET' WHEN BEING SET
    /// </summary>
    public Vector2 origin { get; set; }
    /// <summary>
    /// Offset of the generator
    /// Determines the offset from the origin that the noise is sampled from
    /// </summary>
    public Vector3 offset { get; set; }
    //TODO Considering that it's 3D noise, consider a transformation matrix to mess around with
}
