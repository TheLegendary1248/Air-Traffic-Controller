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
        //Click
        Vector2 pt = _.WorldPoint();
        if (vehicles.Count == 0) return; //If there are no vehicles, then this class has no function anyways
        ControlledVehicle c = GetNearestVehicle(pt);
        Debug.DrawLine(c.transform.position, (Vector3)pt + new Vector3(0f,0f,-3f), Color.red);
        for (int i = 0; i < c.path.pts.Count - 1; i++)
        {
            Debug.DrawLine((Vector3)c.path.pts[i] + new Vector3(0f, 0f, -3f), (Vector3)c.path.pts[i + 1] + new Vector3(0f, 0f, -3f));
        }


        if(Input.GetMouseButton(0))
        {
            c.path.AppendPath(pt);
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
