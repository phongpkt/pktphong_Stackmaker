using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { MainMenu, GamePlay, Pause }

public class GameManager : Singleton<GameManager>
{
    private GameState gameState;

    void Start()
    {
        ChangeState(GameState.GamePlay);
    }

    public void ChangeState(GameState gameState)
    {
        this.gameState = gameState;
    }

    public bool IsState(GameState gameState)
    {
        return this.gameState == gameState;
    }
}
