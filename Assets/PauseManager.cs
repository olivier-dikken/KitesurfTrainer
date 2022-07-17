using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    
    // TODO replace simple pauseMenu panel with proper VR pause menu
    [SerializeField] private GameObject pauseMenu;
    private bool _isPaused;
    
    public bool IsPaused()
    {
        return _isPaused;
    }
    
    private void Awake()
    {
        // subscribe to game manager
        GameManager.OnGameStateChanged += GameStateChanged;
    }

    private void OnDestroy()
    {
        // unsubscribe to game manager
        GameManager.OnGameStateChanged += GameStateChanged;
    }

    private void Pause()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        _isPaused = true;
    }

    private void Resume()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        _isPaused = false;
    }

    private void GameStateChanged(GameState state)
    {
        if (state == GameState.Paused)
        {
            Pause();
        }
        else
        {
            Debug.Log("hey");
            Resume();
        }
        
    }
}