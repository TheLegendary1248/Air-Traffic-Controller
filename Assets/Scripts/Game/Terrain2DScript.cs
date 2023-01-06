using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain2DScript : MonoBehaviour
{
    public Gradient primaryGradient;
    public Gradient secondaryGradient;
    public MeshFilter landMFil;
    public MeshRenderer landMRend;
    public MeshFilter waterMFil;
    public MeshRenderer waterMRend;
    Material landMat;
    Material waterMat;
    // Start is called before the first frame update
    void Start()
    {
        //Instance each material to the renderer
        landMat = landMRend.material;
        waterMat = waterMRend.material;
        //Other stuff
        Texture2D tex = GradientToTex(primaryGradient,256);
        landMFil.mesh = CreatePlane(new Vector2Int(150, 150));
        landMat.SetTexture("_MainTex", tex);
        waterMFil.mesh = CreatePlane(new Vector2Int(1, 1));
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
    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        landMat.SetFloat("_ZOffset", Time.time / 35f);
        waterMat.SetFloat("_ZOffset", Time.time / 35f);
    }
    /// <summary>
    /// Creates a one pixel high texture gradient for the shader
    /// </summary>
    /// <param name="col"></param>
    /// <param name="resolution"></param>
    /// <returns></returns>
    public static Texture2D GradientToTex(Gradient col, int resolution)
    {
        Texture2D tex = new Texture2D(resolution, 1);
        for (int pix = 0; pix < resolution; pix++) tex.SetPixel(pix, 0, col.Evaluate((float)pix / resolution));
        tex.Apply();
        tex.filterMode = FilterMode.Point;
        return tex;
    }
}
