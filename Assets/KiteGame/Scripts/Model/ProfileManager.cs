using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileManager : MonoBehaviour
{
    public static ProfileManager instance;
    public ConnectionState connectionState;
    
    public Profile currentProfile;

    private void Awake()
    {
        instance = this;
    }

    void UpdateProfile(Profile newProfile)
    {
        currentProfile = newProfile;
        connectionState = ConnectionState.loggedIn;
    }

    void Logout()
    {
        currentProfile = null;
        connectionState = ConnectionState.loggedOut;
    }

    public GameLevel GetCurrentProfileFurthestLevel()
    {
        if (currentProfile != null && connectionState == ConnectionState.loggedIn)
            return GetProfileFurthestLevel(currentProfile);
        else
        {
            Debug.Log("Not logged in. Furthest level is level 0.");
            return GameLevelManager.instance.GameLevels[0];
        }
    }

    public GameLevel GetProfileFurthestLevel(Profile profile)
    {
        return profile.maxLevel;
    }

}

public enum ConnectionState
{
    loggedIn,
    loggedOut
}
