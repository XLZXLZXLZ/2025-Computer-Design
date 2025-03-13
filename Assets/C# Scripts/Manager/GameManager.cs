using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Platform
{
    Windows,
    Web,
    Android
}

public class GameManager : Singleton<GameManager>
{
    public enum GameState
    {
        Load,
        Dialogue,
        Interact
    }

    public Platform Platform {  get; private set; }

    public GameState CurrentState {  get; set; }

    public bool CanInteractNow => CurrentState == GameState.Interact;

    protected override void Awake()
    {
        base.Awake();

        if (Application.platform == RuntimePlatform.Android)
            Platform = Platform.Android;
        else if (Application.platform == RuntimePlatform.WebGLPlayer)
            Platform = Platform.Web;
        else
            Platform = Platform.Windows;
    }
}
