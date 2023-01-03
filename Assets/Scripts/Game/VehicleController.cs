using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main Controller for all vehicles
/// </summary>
public class VehicleController : MonoBehaviour
{
    public List<ControlledVehicle> vehicles = new List<ControlledVehicle>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        //Click down
        if(true)
        {

        }
    }
    public ControlledVehicle GetNearestVehicle(Vector2 vec)
    {
        int closest = 0;
        float dist = Mathf.Infinity;
        for (int i = 0; i < vehicles.Count; i++) closest = ((Vector2)vehicles[i].gameObject.transform.position - vec).sqrMagnitude < dist ? closest : i;
        return vehicles[closest];
    }
}
