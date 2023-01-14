using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Undescribed
/// </summary>
public class TrafficControllerCondition : MonoBehaviour
{
    /// <summary>
    /// The 'Health' of this condition
    /// </summary>
    public float reputation = 100f;
    public float maxReputation = 100f;
    /// <summary>
    /// Time of last failure
    /// </summary>
    public float timestampSinceLastFail = 0f;
    /// <summary>
    /// The amount of time it takes since last crash for the multiplier to have no effect
    /// </summary>
    public float timeToCalm = 15f;
    /// <summary>
    /// The multiplier
    /// </summary>
    public float multiplier = 2.5f;
    /// <summary>
    /// The base reputation loss per crash
    /// </summary>
    public float baseLoss = 10f;
    /// <summary>
    /// The base regen of reputation per second
    /// </summary>
    public float regen = 0.5f;

    
    // Start is called before the first frame update
    public void FixedUpdate()
    {
        reputation = Mathf.Max(maxReputation, reputation + (regen * Time.fixedDeltaTime));
        //Update UI visual
    }
    public void VehicleCrash()
    {
        
    }
    
    public void Start()
    {
        //Subscribe to vehicle crash event   
    }

}
