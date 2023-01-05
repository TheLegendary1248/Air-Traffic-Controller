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

#if false //Placeholder
    public object recievepath;
    public object sendpath;
#endif
    public void Start()
    {
        

    }
    //Vehicle detections
    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }
}
