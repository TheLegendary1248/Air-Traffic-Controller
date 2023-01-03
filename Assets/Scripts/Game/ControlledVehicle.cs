using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Base class for all controllable vehicles
/// </summary>
public class ControlledVehicle : MonoBehaviour
{
    public float speed;
    public float turnrate;
    public Rigidbody2D rb;
    //EXPERIMENTAL: time it takes to change in between 
    public float agility;

    private void FixedUpdate()
    {
        transform.Translate(Vector2.up * speed * Time.fixedDeltaTime, Space.Self);
    }

    void GoToPoint()
    {

    }
}
