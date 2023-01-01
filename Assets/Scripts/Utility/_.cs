using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A class for not having to write painful code everytime, aka shorthands
/// </summary>
public static class _
{
    /// <summary>
    /// Creates a Vector representing top down angle for eulerAngles
    /// </summary>
    /// <param name="angle">Angle of the vector</param>
    public static Vector3 Angle(float angle) => new Vector3(0f, 0f, angle);  
}
