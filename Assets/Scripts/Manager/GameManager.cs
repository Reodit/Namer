using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStates
{
    Lobby,
    InGame,
    Lose,
    Victory
}


public class GameManager : Singleton<GameManager>
{
    private GameStates state;

    public static event Action<GameStates> OnGameStateChanged; 

    private void Start()
    {
    }

    public void UpdateGameState()
    {
        
        switch (state)
        {
            case GameStates.Lobby: 
                HandleLobby();
                break;
            case GameStates.InGame:
                HandleInGame();
                break;
            case GameStates.Victory:
                HandleVictory();
                break;
            case GameStates.Lose:
                HandleLost();
                break;
            
        }
        OnGameStateChanged?.Invoke(state);
    }
    public void ChangeGameState(GameStates newState)
    {
        state = newState;
        UpdateGameState();
    }


    private void HandleLost()
    {
        //menu for reset
        throw new NotImplementedException();
    }

    private void HandleVictory()
    {
        // show viectory ui
        // menu for next lever or quit
        throw new NotImplementedException();
    }

    private void HandleInGame()
    {
        
        //load new scene 
        LoadScene(Scenes.InGame,LoadSceneMode.Single);
        //instantiate player
        //instantiate objects
        //instantiate cards
    }

    void HandleLobby()
    {
        // menu 
        // start game 
    }
    
    public void Reset()
    {
        if (state == GameStates.Lobby) return;
        SceneBehaviorManager.ResetScene();
    }

    public void LoadScene(Scenes scenes, LoadSceneMode loadSceneMode)
    {
        SceneBehaviorManager.LoadScene(scenes,loadSceneMode);
    }
  
}
