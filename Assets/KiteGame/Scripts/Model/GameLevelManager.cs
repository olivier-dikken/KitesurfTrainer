using System;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelManager : MonoBehaviour
{

    public static GameLevelManager instance;
    public GameLevel CurrentLevel;
    public List<GameLevel> GameLevels = new List<GameLevel>();

    public static event Action<GameLevel> onGameLevelChanged;

    private void Awake()
    {
        instance = this;
    }


    void SetupAllLevels()
    {
        GameLevel level_1 = new GameLevel("Island Crash Introduction", Vector3.zero, "Player crashed with plane onto island. Needs to find a way to escape.");
        GameLevel level_2 = new GameLevel("Assemble Gear", Vector3.zero, "Player has found kitesurf equipment. Player needs to bring equipment to a launch zone, setup equipment, connect harness and attempt to launch (or auto launch).");
        GameLevel level_3 = new GameLevel("Danger! Quicksand", Vector3.zero, "Player has attempted launch, kite pulls player into quicksand. Player needs to generate power with kite to pull him/herself out of quicksand.");

        GameLevels.Add(level_1);
        GameLevels.Add(level_2);
        GameLevels.Add(level_3);
    }


    void UpdateCurrentLevel(GameLevel newLevel)
    {
        CurrentLevel = newLevel;
    }


}

