using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Profile : MonoBehaviour
{
    bool isGuest;
    string name;
    private string pw;
    public GameLevel maxLevel { get; }

    //save scores and other progress


    public Profile(string name = "Kiter", string pw = "", GameLevel maxLevel = null, bool isGuest = true)
    {
        this.name = name;
        this.pw = pw;
        this.maxLevel = maxLevel;
        this.isGuest = true;
    }
}
