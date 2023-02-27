using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Generic functions to be called by the main menu for a game loop
/// </summary>
public class MainMenu : MonoBehaviour
{
  
    //Load a game into session
    
    public static void ExitGame() => Application.Quit();
    public static void StartGame() => GameManager.StartGame();
}
