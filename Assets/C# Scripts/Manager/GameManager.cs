using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public enum GameState
    {
        Load,
        Dialogue,
        Interact
    }

    public GameState CurrentState {  get; set; }

    public bool CanInteractNow => CurrentState == GameState.Interact;
}
