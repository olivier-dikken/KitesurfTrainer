using System;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;
    public MenuState state;


    private void Awake()
    {
        instance = this;
        state = MenuState.active;
    }

    public void MenuActionPlay()
    {
        if(state == MenuState.active)
        {
            GameManager.instance.UpdateGameState(GameState.Starting);
            SceneController.instance.LoadScene(GameScene.IslandCrash);
            //TODO get level from profile, and set level
            GameLevel currentLevel = ProfileManager.instance.GetCurrentProfileFurthestLevel();
            GameLevelManager.instance.UpdateCurrentLevel(currentLevel);
        } else
        {
            Debug.Log("MenuActionPlay() was triggered but state == MenuStae.active is false");
        }
        
    }

}

public enum MenuState
{
    active,
    inactive
}
