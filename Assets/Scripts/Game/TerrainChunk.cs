using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChunk : MonoBehaviour, ITerrain
{
    public Texture2D differentialTexture;
    public Gradient primaryGradient;
    public Gradient secondaryGradient;
    //TODO Extract each visual as it's own object
    public MeshFilter landMFil;
    public MeshRenderer landMRend;
    public MeshFilter waterMFil;
    public MeshRenderer waterMRend;
    Material landMat;
    Material waterMat;
    public float slope;
    public float height;
    [Tooltip("Scale of the noise generator"), SerializeField]
    Vector2 _scale;
    public Vector2 scale 
    {
        get => _scale;
        set
        {
            _scale = value;
            landMat.SetTextureScale("_MainTex", value);
            waterMat.SetTextureScale("_MainTex", value);
        }
    }
    public Vector2 origin { get; set; }
    [Tooltip("Offset of the noise generator"), SerializeField]
    Vector3 _offset;
    public Vector3 offset 
    {
        get => _offset;
        set
        {
            _offset = value;
            landMat.SetTextureOffset("_MainTex", value);
            waterMat.SetTextureOffset("_MainTex", value);
            landMat.SetFloat("_ZOffset", value.z);
            waterMat.SetFloat("_ZOffset", value.z);
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        //Instance each material to the renderer
        landMat = landMRend.material;
        waterMat = waterMRend.material;
        //Other stuff
        Texture2D tex = GradientToTex(primaryGradient, secondaryGradient,256);
        landMFil.mesh = CreatePlane(new Vector2Int(150, 150));
        landMat.SetTexture("_MainTex", tex);
        waterMFil.mesh = CreatePlane(new Vector2Int(1, 1));
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
    }
    /// <summary>
    /// Gets the terrain height relative to offsets and scale
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public float GetTerrainHeight(Vector2 pos)
    {
        pos *= _scale;
        Debug.Log($"Position {pos}, scale {_scale}");
        return Mathf.Pow(
            Mathf.Abs(
                ShaderFunctions.ClassicNoise(
                    (Vector3)pos + offset))
            , slope) 
        * height * 100f;
    }


    Mesh CreatePlane(Vector2Int size)
    {
        Mesh m = new Mesh();
        int vx = size.x + 1;
        int vz = size.y + 1;
        Vector3[] verts = new Vector3[vx * vz];
        Vector2[] uv = new Vector2[vx * vz];
        int[] tris = new int[size.x * size.y * 2 * 3];
        //Create Verts
        for (int x = 0; x <= size.x; x++)
        {
            for (int z = 0; z <= size.y; z++)
            {
                verts[(vx * x) + z] = new Vector3(x / (float)size.x, 0, z / (float)size.y);
                uv[(vx * x) + z] = new Vector2(x / (float)size.x, z / (float)size.y);
            }
        }
        //Create Triangles //THANK YOU CATLIKE https://catlikecoding.com/unity/tutorials/procedural-grid/
        for (int ti = 0, vi = 0, y = 0; y < size.y; y++, vi++) {
			for (int x = 0; x < size.x; x++, ti += 6, vi++) {
				tris[ti] = vi;
                tris[ti + 1] = tris[ti + 3] = vi + 1;
                tris[ti + 4] = vi + size.x + 2;
				tris[ti + 5] = tris[ti + 2] = vi + size.x + 1;				
			}
		}
        m.vertices = verts;
        m.uv = uv;
        m.triangles = tris;
        return m;
    }
    
    /// <summary>
    /// Creates a one pixel high texture gradient for the shader
    /// </summary>
    /// <param name="col"></param>
    /// <param name="resolution"></param>
    /// <returns></returns>
    public static Texture2D GradientToTex(Gradient col, int resolution)
    {
        //TODO Map to 'slope'
        Texture2D tex = new Texture2D(resolution, 1);
        for (int pix = 0; pix < resolution; pix++) tex.SetPixel(pix, 0, col.Evaluate((float)pix / resolution));
        tex.Apply();
        tex.filterMode = FilterMode.Point;
        tex.wrapMode = TextureWrapMode.Clamp;
        return tex;
    }
    public static Texture2D GradientToTex(Gradient col1, Gradient col2, int resolution)
    {
        //TODO Map to 'slope'
        Texture2D tex = new Texture2D(resolution, 1);
        int half = resolution / 2;
        for (int pix = 0; pix < resolution; pix++) 
            tex.SetPixel(pix, 0,
                pix < half
                ?
                col1.Evaluate((float)(half - pix)/ half)
                : col2.Evaluate((float)(pix - half)/ half));
        tex.Apply();
        tex.filterMode = FilterMode.Point;
        tex.wrapMode = TextureWrapMode.Clamp;
        return tex;
    }
    
}
