using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
/// <summary>
/// Handles a game session
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// The singleton instance
    /// </summary>
    public static GameManager self;
    /// <summary>
    /// When the player is exiting the game session
    /// </summary>
    public static event Action OnExit;
    /// <summary>
    /// When the player has reached a win or lose condition(or any other game ending condition)
    /// </summary>
    public static event Action OnGameOver;
    /// <summary>
    /// When the player has reached a lose condition
    /// </summary>
    public static event Action OnLose;
    /// <summary>
    /// When the player has reached a win condition
    /// </summary>
    public static event Action OnWin;

}
