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
    ControlledVehicle selected;
    
    // Start is called before the first frame update
    void Start()
    {
        Main = this;
    }
    //Functions of the controller for the player
    void Update()
    {
        //Click
        Vector2 pt = _.WorldPoint();
        if (vehicles.Count == 0) { return; } //If there are no vehicles, then this class has no function anyways
        ControlledVehicle c = GetNearestVehicle(pt);
        if(Input.GetMouseButtonDown(0))
        {
            selected = c;
            selected.path.ClearPath();
        }
        if(Input.GetMouseButton(0))
        {
            selected?.path.AppendPath(pt);
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
