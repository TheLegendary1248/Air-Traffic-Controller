using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIManager : MonoBehaviour
{
    public Animation animation;
    private void Awake()
    {
        GameManager.OnGameStart += OnGameStart;
        GameManager.OnGameOver += OnGameOver;
    }

    public void OnGameStart()
    {
        gameObject.SetActive(false);
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
