using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public Animation animation;
    private void Awake()
    {
        Debug.Log("HELLO FUCKER????");
        GameManager.OnGameStart += OnGameStart;
        GameManager.OnGameOver += OnGameOver;
        gameObject.SetActive(false);
    }
    
    public void OnGameStart()
    {
        gameObject.SetActive(true);
    }
    public void OnGameOver()
    {
        animation.Play("RemoveGameUI");
    }
    private void OnEnable()
    {
        //Play anim
        animation.Play("AddGameUI");
    }
    
}
