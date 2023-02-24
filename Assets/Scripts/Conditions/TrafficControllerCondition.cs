using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    public Image barFill;
    
    // Start is called before the first frame update
    public void FixedUpdate()
    {
        reputation = Mathf.Min(maxReputation, reputation + (regen * Time.fixedDeltaTime));
        UpdateBar();
    }
    public void VehicleCrash()
    {
        //Give extra penalty for crashes that follow other crashes
        float errorMultiCalc = Mathf.Lerp(1f, multiplier, (timeToCalm - (Time.fixedTime - timestampSinceLastFail)) / timeToCalm);
        reputation -= errorMultiCalc * baseLoss;
        timestampSinceLastFail = Time.fixedTime;
        UpdateBar();
        if(reputation < 0)
        {
            LoseCondition();
        }
    }
    public void LoseCondition()
    {
        GameManager.Lose();
    }
    void UpdateBar()
    {
        float interpol = reputation / maxReputation;
        barFill.fillAmount = interpol;
        barFill.color = Color.HSVToRGB((127 * interpol) / 360f, .58f, 1);
    }
    
    public void Start()
    {
        ControlledVehicle.OnVehicleCrash += VehicleCrash;

    }
    public void OnDestroy()
    {
        ControlledVehicle.OnVehicleCrash -= VehicleCrash;
    }

}
