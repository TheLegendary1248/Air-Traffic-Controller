using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    /// <summary>
    /// The turning rate of the vehicle over a second
    /// </summary>    
    public float turnrate;
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
        Vector2 v = Vector2.zero;
        //Follow path
        if(path.CheckPoint(transform.position, transform.forward, out v))
        {
            

            float timeToStop = Mathf.Abs(turn) / turnrate;
            float angleToPt = Vector2.SignedAngle(v - (Vector2)transform.position, transform.up);

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
            Debug.DrawRay(transform.position, transform.right * Mathf.Sign(turndir), Color.cyan);
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
    }
}
/// <summary>
/// Object for keeping tracking of points
/// </summary>
public class Path
{
    public List<Vector2> pts = new List<Vector2>();
    public float threshold = 2f;
    
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
    public void ClearPath() => pts.Clear();
    /// <summary>
    /// Adds onto the existing path
    /// </summary>
    /// <param name="vec"></param>
    public void AppendPath(Vector2 vec)
    {
        pts.Add(vec);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="dir"></param>
    public bool CheckPoint(Vector2 pos, Vector2 dir, out Vector2 next)
    {
        //Check each point
        int i;
        for (i = 0; i < pts.Count; i++)
        {
            Vector2 dif = pos - pts[i];
            Vector2 pt = pts[i];
            //If the point is close enough
            if (dif.sqrMagnitude < threshold * threshold)
            {
                //If the point is behind the object
                if (Vector2.Dot(dir, pos - pt) < 0) 
                {
                    continue;
                }
            }
            break;
        }
        pts.RemoveRange(0, i);
        if (pts.Count != 0) next = pts[0];
        else next = pos + dir; //Point ahead the given position anyways for convenience
        return pts.Count != 0;
    }
}
