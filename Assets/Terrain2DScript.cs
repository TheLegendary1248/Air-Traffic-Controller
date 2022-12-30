using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain2DScript : MonoBehaviour
{
    public Gradient primaryGradient;
    public Gradient secondaryGradient;
    public SpriteRenderer spriteRend;
    // Start is called before the first frame update
    void Start()
    {
        spriteRend.sprite = Sprite.Create(GradientToTex(primaryGradient,256), Rect.MinMaxRect(0,0,1,1), Vector2.zero);
    }

    // Update is called once per frame
    void Update()
    {
        
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
