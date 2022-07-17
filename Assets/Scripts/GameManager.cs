using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState currentState;
    [SerializeField] private GameState InitialState;

    public static event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateGameState(GameState newState)
    {
        // update the state
        currentState = newState;
    
        // handle the state change
        // switch (newState)
        // {
        //     case GameState.Playing:
        //         break;
        //     case GameState.Paused:
        //         break;
        //     default:
        //         throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        // }

        // trigger state changed event to subscribed scripts
        OnGameStateChanged?.Invoke(newState);
    }

    void Start()
    {
        // set initial state
        UpdateGameState(InitialState);
    }
}

public enum GameState
{
    Playing,
    Paused
}