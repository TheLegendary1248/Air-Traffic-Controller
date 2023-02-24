using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Generic functions to be called by the main menu for a game loop
/// </summary>
public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Load a game into session
    void LoadGame()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }
    static void Exit() => Application.Quit();
    
    
}
