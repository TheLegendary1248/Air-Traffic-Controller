using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// Base class for all controllable vehicles
/// </summary>
public class ControlledVehicle : MonoBehaviour
{
    /// <summary>
    /// The constant speed of the vehicle
    /// </summary>
    public float speed = 1f;
    /// <summary>
    /// The maximum turning ability of the vehicle
    /// </summary>
    public float maxturn = 2f;
    /// <summary>
    /// The current turning rate
    /// </summary>
    public float turn = 0f;
    public Rigidbody2D rb;
    public Path path = new Path();
    public GameObject cosmetic;
    public LineRenderer line;
    public GameObject deathFX;
    bool hasEntered = false;
    /// <summary>
    /// The turning rate of the vehicle over a second
    /// </summary>    
    public float turnrate;

    /// <summary>
    /// When a vehicle has left the scene
    /// </summary>
    public static event Action OnVehicleLeft;
    /// <summary>
    /// When a vehicle has entered the scene
    /// </summary>
    public static event Action OnVehicleEnter;
    /// <summary>
    /// LIKELY TEMPORARY. When a vehicle has been destroyed
    /// </summary>
    public static event Action OnVehicleCrash;
    public void Start()
    {
        //VehicleController.Main.AddVehicle(this);
        //Setup path component
        path.line = line;
        path.range = speed;
        OnVehicleEnter?.Invoke();
    }
    public void OnDestroy()
    {   
        if (!gameObject.scene.isLoaded) return;
        //Automatically remove the vehicle from the controller
        VehicleController.Main.RemoveVehicle(this);
        //Remove trail gameobject to preserve them from destruction
        TrailRenderer[] trails = GetComponentsInChildren<TrailRenderer>();
        foreach (TrailRenderer trail in trails)
        {
            trail.autodestruct = true;
            trail.transform.parent = null;
        }
        OnVehicleLeft?.Invoke();
    }
    private void FixedUpdate()
    {
        //Check plane has not hit terrain. Cast to Vector2 to lose Z
        float terra = 0;
        if (World.Main.GetTerrainHeight((Vector2)transform.position, out terra))
        {
            if (!hasEntered) { VehicleController.Main.AddVehicle(this); hasEntered = true; }
            Debug.DrawLine(transform.position, transform.position + new Vector3(0f, 0f, terra * 10f), Color.red);
            if (terra > -transform.position.z) Collision();
        }
        //Has left the playing field
        else if (hasEntered) Collision();

        Vector2 v = Vector2.zero;
        //Follow path
        if(path.CheckPoint(transform.position, transform.up, out v))
        {
            float timeToStop = Mathf.Abs(turn) / turnrate;
            float angleToPt = Vector2.SignedAngle(v - (Vector2)transform.position, transform.up);
            //TODO Extract this
            //Quadratic Formula to solve from time 
            float absTurn = Mathf.Abs(turn);
            float b = turn * -Mathf.Sign(angleToPt); //Base turn
            float c = Mathf.Abs(angleToPt * 2f); //Target distance
            float a = turnrate; //Acceleration
            float l = (-b + Mathf.Sqrt((b*b) + (2*a*c))) / a;
            
            float turndir = turnrate * Time.fixedDeltaTime
                    * Mathf.Sign(angleToPt)
                    * Mathf.Sign(l - timeToStop)
                    ;
            turn -= turndir;
            turn = Mathf.Clamp(turn, -maxturn, maxturn);

        }
        //Else if we're still turning, stop turning
        else if(turn != 0)
        {
            float rot = turnrate * Time.fixedDeltaTime;
            if (turn < rot) turn = 0;
            else turn -= rot * Mathf.Sign(turn);
        }
        transform.eulerAngles = _.Angle(transform.eulerAngles.z + (turn * Time.fixedDeltaTime));
        transform.Translate(Vector2.up * speed * Time.fixedDeltaTime, Space.Self);

        if(cosmetic) cosmetic.transform.localEulerAngles = new Vector2(-90f, turn / 2f);
    }
    ///<summary>
    ///Causes to surrender it's normal behaviour and remove it from the controller
    ///</summary>
    public void Dock()
    {
        enabled = false;
        path.ClearPath();
        GetComponent<Collider2D>().enabled = false;
        VehicleController.Main.RemoveVehicle(this);
    }
    ///<summary>
    ///Destroys the vehicle 
    ///</summary>
    public void Collision()
    {
        //Destroy with style
        Instantiate(deathFX, transform.position, transform.rotation);
        Destroy(gameObject);
        OnVehicleCrash?.Invoke();
    }
    public void OnCollisionEnter2D(Collision2D collision) => Collision();
}
/// <summary>
/// Object for keeping tracking of points
/// </summary>
public class Path
{
    public List<Vector2> pts = new List<Vector2>();
    /// <summary>
    /// Range from vehicle position that which points will be checked
    /// </summary>
    public float range = 4f;
    /// <summary>
    /// Minimum forced distance from the last point when adding new points
    /// </summary>
    public float unitRange = 0.7f;
    public LineRenderer line;
    
    /// <summary>
    /// Get's the closest defined path point
    /// </summary>
    public Vector2 GetClosestPoint(Vector2 vec)
    {
        int closest = 0;
        float dist = Mathf.Infinity;
        for (int i = 0; i < pts.Count; i++)
        {
            float objDist = (pts[i] - vec).sqrMagnitude;
            if (objDist < dist) { closest = i; dist = objDist; }
        }
        return pts[closest];
    }
    public void InsertIntoPath(Vector2 vec)
    {

    }
    /// <summary>
    /// Clears vehicle path
    /// </summary>
    public void ClearPath() { pts.Clear(); SetLine(); if (line) line.material.SetColor("_Color", Color.HSVToRGB(Time.fixedTime / 30 % 1, 1, 1)); }
    /// <summary>
    /// Adds onto the existing path
    /// </summary>
    /// <param name="vec"></param>
    public void AppendPath(Vector2 vec)
    {
        //If one or less, just add
        if (pts.Count <= 1) pts.Add(vec);
        //If the second point from the end is too close, set the last point
        else if ((pts[pts.Count - 2] - vec).sqrMagnitude < unitRange) { pts[pts.Count - 1] = vec; }
        else pts.Add(vec);
        SetLine();
    }
    /// <summary>
    /// Rebuilds the line renderer if given
    /// </summary>
    void SetLine()
    {
        if (line)
            if (pts.Count > 1) //If the line renderer has enough points to render
            {
                Vector3 start = line.GetPosition(0);
                line.positionCount = pts.Count + 1;
                Vector3[] arr = new Vector3[pts.Count + 1];
                Array.ConvertAll(pts.ToArray(), i => new Vector3(i.x, i.y, -3f)).CopyTo(arr, 1);
                arr[0] = start;         //Set follower point
                line.SetPositions(arr); //Set line
            }
            else line.positionCount = pts.Count; //Keep the one vertex on edge cases
    }
    /// <summary>
    /// Updates the first point in the given line renderer to match the follower
    /// </summary>
    public void UpdateFollowerPoint(Vector2 pos) { if (line && line.positionCount != 0) line.SetPosition(0, new Vector3(pos.x, pos.y, -3f)); }
    /// <summary>
    /// Checks the vehicles
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="dir"></param>
    public bool CheckPoint(Vector2 pos, Vector2 dir, out Vector2 next)
    {
        int i;
        int b = 0;
        for (i = 0; i < pts.Count; i++)
        {   //Check each point from the beginning
            Vector2 pt = pts[i];
            Vector2 dif = pos - pt;
            //If the point is close enough, continue iterating
            if (dif.sqrMagnitude < range * range)
            {
                //If the point is behind the object, remove it
                if (Vector2.Dot(dir, pos - pt) > 0.75f) b = i + 1;
                continue;
            }
            break;
        }
        if (pts.Count != 0) next = pts[Mathf.Min(i, pts.Count - 1)];//If not empty, give the point furthest in given range
        else next = pos + dir;                                      //Point ahead the given position anyways for convenience
        //Remove past points
        pts.RemoveRange(0, b);
        SetLine();
        UpdateFollowerPoint(pos);
        return pts.Count != 0;
    }
}
