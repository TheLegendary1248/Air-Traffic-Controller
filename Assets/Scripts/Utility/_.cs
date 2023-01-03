using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A class for not having to write painful code everytime, aka shorthands
/// </summary>
public static class _
{
    static Plane worldPlane = new Plane(Vector3.forward, 0);
    /// <summary>
    /// Creates a Vector representing top down angle for eulerAngles
    /// </summary>
    /// <param name="angle">Angle of the vector</param>
    public static Vector3 Angle(float angle) => new Vector3(0f, 0f, angle);
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static Vector2 WorldPoint() => WorldPoint(Camera.main);
    public static Vector2 WorldPoint(Camera cam) => WorldPoint(cam.ScreenPointToRay(Input.mousePosition));
    public static Vector2 WorldPoint(Ray r)
    {
        float distance; Vector3 worldPosition = Vector2.zero;
        if (worldPlane.Raycast(r, out distance))
        {
            worldPosition = r.GetPoint(distance);
        }
        return worldPosition;
    }

    //Precalc for rotation transformation in the next function
    static float sin45 = Mathf.Sin(Mathf.Deg2Rad * -45f);
    static float cos45 = Mathf.Cos(Mathf.Deg2Rad * -45f);
    /// <summary>
    /// Gets the 'terrain' coordinates from world coordinates to match with the shader noise
    /// </summary>
    public static Vector2 ToTerrainCoord(Vector2 vec) => new Vector2((vec.x * cos45) - (vec.y * sin45), (vec.x * sin45) + (vec.y * cos45));
}
