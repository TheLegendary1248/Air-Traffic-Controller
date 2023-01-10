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
    [SerializeField]
    protected Vector2 _scale;
    [SerializeField]
    protected Vector2 _origin;
    [SerializeField]
    protected Vector3 _offset;
    public virtual Vector2 scale { get; set; }
    public virtual Vector2 origin { get; set; }
    public virtual Vector3 offset { get; set; }
    public virtual void Start()
    {
        Main = this;
        calcrotation = transform.eulerAngles.x;
    }
    //C
    public virtual bool GetTerrainHeight(Vector2 vec, out float result)
    {
        Vector2 m = ToTerrainCoord(vec);
        
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
