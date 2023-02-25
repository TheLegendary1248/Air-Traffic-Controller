using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Class responsible for UI
/// </summary>
public class UIManager : MonoBehaviour
{
    
    public static UIManager self;
    Animation animation;
    public void Awake()
    {
        //Initialize
        GameManager.OnLose += OnLose;
        animation = GetComponent<Animation>();
    }
    public void OnLose()
    {
        animation.Play("RemoveScreen");
    }
    
}
