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

}

public enum ConnectionState
{
    loggedIn,
    loggedOut
}
