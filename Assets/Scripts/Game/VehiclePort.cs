using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls send and recieving of vehicles on respective 'ports'
/// </summary>
public class VehiclePort : MonoBehaviour
{
    public bool canSend;
    public bool canAccept;
    public float angleThreshold = 20f;
    public Transform road;

#if false //Placeholder
    public object recievepath;
    public object sendpath;
#endif
    //Precalc values for anim
    Vector3 endOfPort;

    public void Start()
    {
        endOfPort = road.position + ((road.up * road.localScale.y) / 2f) + ((-road.forward * road.localScale.z) / 2f);
    }
    //Vehicle detections
    private void OnTriggerStay2D(Collider2D collision)
    {
        ControlledVehicle vehicle;
        //If the vehicle is given
        if(vehicle = collision.GetComponent<ControlledVehicle>())
        {
            if(vehicle.enabled)
            {
                //Check that vehicle is positioned correctly to dock
                if (Mathf.DeltaAngle(vehicle.transform.eulerAngles.z, transform.eulerAngles.z) > angleThreshold) return;

                //Proceed to dock
                vehicle.Dock();
                StartCoroutine(DockVehicleAnim(vehicle.gameObject, 5f));
            }
        }
    }
    IEnumerator DockVehicleAnim(GameObject vehicle, float time)
    {
        float timestamp = Time.fixedTime;
        Vector3 fromPos = vehicle.transform.position;
        float fromAngle = vehicle.transform.eulerAngles.z;
        while ((Time.fixedTime - timestamp) < time)
        {
            DockLerp(vehicle.transform, fromPos, fromAngle, (Time.fixedTime - timestamp) / time);
            yield return new WaitForEndOfFrame();
        }
        Destroy(vehicle);
    }
    void DockLerp(Transform vehicle, Vector3 pos, float angle, float t)
    {
        vehicle.position = Vector3.Lerp(pos, endOfPort, Mathf.Pow(t, 0.8f));
        vehicle.eulerAngles = _.Angle(Mathf.LerpAngle(angle, road.eulerAngles.z, Mathf.Pow(t,0.8f)));
    }
}
