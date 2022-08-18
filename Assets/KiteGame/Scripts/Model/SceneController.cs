using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    

    void Awake()
    {
        instance = this;
    }

    public void LoadScene(GameScene sceneToLoad)
    {
        SceneManager.LoadScene(((int)sceneToLoad));
    }

}

public enum GameScene
{
    MainMenu = 0,
    IslandCrash = 1,
}
