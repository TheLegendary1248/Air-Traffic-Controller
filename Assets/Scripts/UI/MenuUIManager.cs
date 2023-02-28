using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIManager : MonoBehaviour
{
    //FUTURE ME, organize this before you take it out of hand like usual
    //please listen to past you
    public Animation animation;
    public GameObject currentPanel;
    private void Awake()
    {
        GameManager.OnGameStart += OnGameStart;
        GameManager.OnGameOver += OnGameOver;

        //Initialize since i might be editing another panel
        if(true) //Add editor condition here
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            currentPanel = transform.GetChild(0).gameObject;
            currentPanel.SetActive(true);
        }
    }
    /// <summary>
    /// Switches between menus
    /// </summary>
    /// <param name="panelName">Name of the menu as a gameobject</param>
    public void ChangePanel(string panelName)
    {
        GameObject panel;
        if (panel = transform.Find(panelName).gameObject)
        {
            currentPanel.SetActive(false);
            panel.SetActive(true);
        }
    }
    public static void ExitGame() => Application.Quit();
    public static void StartGame() => GameManager.StartGame();
    void OnGameStart()
    {
        gameObject.SetActive(false);
    }
    void OnGameOver()
    {
        animation.Play("RemoveGameUI");
    }
    private void OnEnable()
    {
        //Play anim
        animation.Play("AddGameUI");
    }
}
