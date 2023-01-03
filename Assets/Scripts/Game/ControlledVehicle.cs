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
    // Start is called before the first frame update
    void Start()
    {

    }
    private void FixedUpdate()
    {
        transform.Translate(Vector2.up * speed * Time.fixedDeltaTime, Space.Self);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void GoToPoint()
    {

    }
}
