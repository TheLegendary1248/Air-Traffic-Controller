using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNoiseFunction : MonoBehaviour
{
    public SpriteRenderer spriteRend;
    public Gradient col;
    public Vector2Int res;
    public Vector2 multi;
    public float scale = 10f;
    // Start is called before the first frame update
    private void OnValidate()
    {
        Texture2D tex = new Texture2D(res.x, res.y);
        for (int x = 0; x < res.x; x++)
        {
            for (int y = 0; y < res.y; y++)
            {
                float m = ShaderFunctions.ClassicNoise(new Vector3(x * multi.x / res.x, y * multi.y / res.y,0f));
                m = Mathf.Repeat(m, 1f);
                tex.SetPixel(x, y, new Color(m, m, m));
                transform.localScale = (Vector2.one / res) * scale;
            }
        }
        tex.Apply();
        tex.filterMode = FilterMode.Point;
        spriteRend.sprite = Sprite.Create(tex, Rect.MinMaxRect(0, 0, res.x, res.y), Vector2.zero);
    }
    void Start()
    {
        
    }
}
