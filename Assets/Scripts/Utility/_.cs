using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A class for not having to write painful code everytime, aka shorthands
/// </summary>
public static class _
{
    static Plane worldPlane = new Plane(Vector3.forward, 3f);
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
        if (worldPlane.Raycast(r, out distance)) worldPosition = r.GetPoint(distance);
        return worldPosition;
    }
    //Ty https://forum.unity.com/threads/pick-random-point-inside-box-collider.541585/

    public static Vector2 GetRandomPointInsideCollider(this BoxCollider2D boxCollider)
    {
        Vector2 extents = boxCollider.size / 2f;
        Vector2 point = new Vector2(
            Random.Range(-extents.x, extents.x),
            Random.Range(-extents.y, extents.y)
        ) + (Vector2)boxCollider.transform.position;
        return boxCollider.transform.TransformPoint(point);
    }
}
