using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main Controller for all vehicles, or also
/// </summary>
public class VehicleController : MonoBehaviour
{
    public List<ControlledVehicle> vehicles = new List<ControlledVehicle>();
    public static VehicleController Main;
    // Start is called before the first frame update
    void Start()
    {
        Main = this;
    }
    //Functions of the controller for the player
    void Update()
    {
        //Click down
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 pt = _.WorldPoint();
            GetNearestVehicle(pt).GoToPoint(pt);
        }
    }
    public void AddVehicle(ControlledVehicle v)
    {
        vehicles.Add(v);
    }
    public void RemoveVehicle(ControlledVehicle v)
    {
        vehicles.Remove(v);
    }
    public ControlledVehicle GetNearestVehicle(Vector2 vec)
    {
        int closest = 0;
        float dist = Mathf.Infinity;
        for (int i = 0; i < vehicles.Count; i++)
        {
            float objDist = ((Vector2)vehicles[i].gameObject.transform.position - vec).sqrMagnitude;
            if (objDist < dist) { closest = i; dist = objDist; }
        }
        return vehicles[closest];
    }
}
