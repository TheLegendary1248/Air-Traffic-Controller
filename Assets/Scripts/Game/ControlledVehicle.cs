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
    public void Start()
    {
        VehicleController.Main.AddVehicle(this);
    }
    public void OnDestroy()
    {
        VehicleController.Main.RemoveVehicle(this);
    }
    private void FixedUpdate()
    {
        transform.Translate(Vector2.up * speed * Time.fixedDeltaTime, Space.Self);
    }

    public void GoToPoint(Vector2 v)
    {
        float angle = Vector2.SignedAngle(Vector2.down, (Vector2)transform.position - v);
        transform.eulerAngles = _.Angle(angle);
    }
}
