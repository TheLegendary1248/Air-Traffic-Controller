using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Reimplementations of the imported shader code so that in-game mechanics can be direct representation of shader's
public static class ShaderFunctions
{
    static float ClassicNoise_impl(Vector3 pi0, Vector3 pf0, Vector3 pi1, Vector3 pf1)
    {
        pi0 = wglnoise_mod289(pi0);
        pi1 = wglnoise_mod289(pi1);

        Vector4 ix = new Vector4(pi0.x, pi1.x, pi0.x, pi1.x);
        Vector4 iy = new Vector4(pi0.y, pi0.y, pi1.y, pi1.y);
        Vector4 iz0 = Vector4.one * pi0.z;
        Vector4 iz1 = Vector4.one * pi1.z;

        Vector4 ixy = wglnoise_permute(wglnoise_permute(ix) + iy);
        Vector4 ixy0 = wglnoise_permute(ixy + iz0);
        Vector4 ixy1 = wglnoise_permute(ixy + iz1);

        Vector4 gx0 = lerp(-Vector4.one, Vector4.one, frac(floor(ixy0 / 7) / 7));
        Vector4 gy0 = lerp(-Vector4.one, Vector4.one, frac(floor(ixy0 % 7) / 7));
        Vector4 gz0 = Vector4.one - gx0.Abs() - gy0.Abs();

        bool4 zn0 = gz0 < -0.01f;
        gx0 += zn0 * (gx0 < -0.01f ? 1 : -1);
        gy0 += zn0 * (gy0 < -0.01f ? 1 : -1);

        Vector4 gx1 = lerp(-Vector4.one, Vector4.one, frac(floor(ixy1 / 7) / 7));
        Vector4 gy1 = lerp(-Vector4.one, Vector4.one, frac(floor(ixy1 % 7) / 7));
        Vector4 gz1 = Vector4.one - gx1.Abs() - gy1.Abs();

        bool4 zn1 = gz1 < -0.01f;
        gx1 += zn1 * (gx1 < -0.01f ? 1 : -1);
        gy1 += zn1 * (gy1 < -0.01f ? 1 : -1);

        Vector3 g000 = Vector3.Normalize(new Vector3(gx0.x, gy0.x, gz0.x));
        Vector3 g100 = Vector3.Normalize(new Vector3(gx0.y, gy0.y, gz0.y));
        Vector3 g010 = Vector3.Normalize(new Vector3(gx0.z, gy0.z, gz0.z));
        Vector3 g110 = Vector3.Normalize(new Vector3(gx0.w, gy0.w, gz0.w));
        Vector3 g001 = Vector3.Normalize(new Vector3(gx1.x, gy1.x, gz1.x));
        Vector3 g101 = Vector3.Normalize(new Vector3(gx1.y, gy1.y, gz1.y));
        Vector3 g011 = Vector3.Normalize(new Vector3(gx1.z, gy1.z, gz1.z));
        Vector3 g111 = Vector3.Normalize(new Vector3(gx1.w, gy1.w, gz1.w));

        float n000 = Vector3.Dot(g000, pf0);
        float n100 = Vector3.Dot(g100, new Vector3(pf1.x, pf0.y, pf0.z));
        float n010 = Vector3.Dot(g010, new Vector3(pf0.x, pf1.y, pf0.z));
        float n110 = Vector3.Dot(g110, new Vector3(pf1.x, pf1.y, pf0.z));
        float n001 = Vector3.Dot(g001, new Vector3(pf0.x, pf0.y, pf1.z));
        float n101 = Vector3.Dot(g101, new Vector3(pf1.x, pf0.y, pf1.z));
        float n011 = Vector3.Dot(g011, new Vector3(pf0.x, pf1.y, pf1.z));
        float n111 = Vector3.Dot(g111, pf1);

        Vector3 fade_xyz = wglnoise_fade(pf0);
        Vector4 n_z = Vector4.Lerp(new Vector4(n000, n100, n010, n110),
                          new Vector4(n001, n101, n011, n111), fade_xyz.z);
        Vector2 n_yz = Vector2.Lerp(n_z/*.xy*/, new Vector2(n_z.z, n_z.w), fade_xyz.y);
        float n_xyz = Mathf.Lerp(n_yz.x, n_yz.y, fade_xyz.x);
        return 1.46f * n_xyz;
    }

    // Classic Perlin noise
    static float ClassicNoise(Vector3 p)
    {
        Vector3 i = floor(p);
        Vector3 f = frac(p);
        return ClassicNoise_impl(i, f, i + Vector3.one, f - Vector3.one);
    }

    // Classic Perlin noise, periodic variant
    static float PeriodicNoise(Vector3 p, Vector3 rep)
    {
        Vector3 i0 = wglnoise_mod(floor(p), rep);
        Vector3 i1 = wglnoise_mod(i0 + Vector3.one, rep);
        Vector3 f = frac(p);
        return ClassicNoise_impl(i0, f, i1, f - Vector3.one);
    }

    static Vector3 frac(Vector3 val)
    {
        return new Vector3(val.x % 1, val.y % 1, val.z % 1);
    }
    #region VectorFloor
    static Vector2 floor(Vector2 val)
    {
        return new Vector2(Mathf.Floor(val.x), Mathf.Floor(val.y));
    }
    static Vector3 floor(Vector3 val)
    {
        return new Vector3(Mathf.Floor(val.x), Mathf.Floor(val.y), Mathf.Floor(val.z));
    }
    static Vector4 floor(Vector4 val)
    {
        return new Vector4(Mathf.Floor(val.x), Mathf.Floor(val.y), Mathf.Floor(val.z), Mathf.Floor(val.w));
    }
    #endregion
    static float wglnoise_mod(float x, float y)
    {
        return x - y * Mathf.Floor(x / y);
    }

    static Vector2 wglnoise_mod(Vector2 x, Vector2 y)
    {
        return x - y * floor(x / y);
    }

    static Vector3 wglnoise_mod(Vector3 x, Vector3 y)
    {
        return x - y * floor(x / y);
    }

    static Vector4 wglnoise_mod(Vector4 x, Vector4 y)
    {
        return x - y * floor(x / y);
    }

    static Vector2 wglnoise_fade(Vector2 t)
    {
        return t * t * t * (t * (t * 6 - 15) + 10);
    }

    static Vector3 wglnoise_fade(Vector3 t)
    {
        return t * t * t * (t * (t * 6 - 15) + 10);
    }

    static float wglnoise_mod289(float x)
    {
        return x - floor(x / 289) * 289;
    }

    static Vector2 wglnoise_mod289(Vector2 x)
    {
        return x - floor(x / 289) * 289;
    }

    static Vector3 wglnoise_mod289(Vector3 x)
    {
        return x - floor(x / 289) * 289;
    }

    static Vector4 wglnoise_mod289(Vector4 x)
    {
        return x - floor(x / 289) * 289;
    }

    static Vector3 wglnoise_permute(Vector3 x)
    {
        Vector3 m = x * 34 + Vector3.one;
        m.Scale(x);
        return wglnoise_mod289(m);
    }

    static Vector4 wglnoise_permute(Vector4 x)
    {
        Vector4 m = x * 34 + Vector4.one;
        m.Scale(x);
        return wglnoise_mod289(m);
    }
    public static Vector2 lerp(Vector2 a, Vector2 b, Vector2 c)
        => new Vector2(Mathf.Lerp(a.x, b.x, c.x), Mathf.Lerp(a.y, b.y, c.y));
    public static Vector3 lerp(Vector3 a, Vector3 b, Vector3 c)
        => new Vector3(Mathf.Lerp(a.x, b.x, c.x), Mathf.Lerp(a.y, b.y, c.y), Mathf.Lerp(a.z, b.z, c.z));
    public static Vector4 lerp(Vector4 a, Vector4 b, Vector4 c) 
        => new Vector4(Mathf.Lerp(a.x,b.x,c.x), Mathf.Lerp(a.y, b.y, c.y), Mathf.Lerp(a.z, b.z, c.z), Mathf.Lerp(a.w, b.w, c.w));
}
public static class VectorExt
{
    public static Vector3 Div(this Vector3 a, Vector3 b) 
        => new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
    public static Vector4 Mod (this Vector4 vec, float a) 
        => new Vector4(vec.x % a, vec.y % a, vec.z % a, vec.w % a);
    public static Vector4 Abs(this Vector4 vec) 
        => new Vector4(Mathf.Abs(vec.x), Mathf.Abs(vec.y), Mathf.Abs(vec.z), Mathf.Abs(vec.z));
    
}
public struct bool4
{
    public bool x;
    public bool y;
    public bool z;
    public bool w;
    public bool4(bool x, bool y, bool z, bool w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }
    public static bool4 operator < (bool4 a, float b) 
        => new bool4(a.x < b, a.y < b, a.z < b, a.w < b);
}