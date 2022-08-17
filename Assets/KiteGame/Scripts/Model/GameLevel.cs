using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevel : MonoBehaviour
{
    protected string name;
    protected Vector3 spawnLocation;
    protected string informationMessage;


    public GameLevel(string name, Vector3 spawnLocation, string informationMessage)
    {
        this.name = name;
        this.spawnLocation = spawnLocation;
        this.informationMessage = informationMessage;
    }    
}
