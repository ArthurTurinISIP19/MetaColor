using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CustomNetwork : NetworkManager
{
    
    public override void OnClientConnect(NetworkConnection conn)
    {
        if (!clientLoadedScene)
        {
            // Ready/AddPlayer is usually triggered by a scene load
            // completing. if no scene was loaded, then Ready/AddPlayer it
            // here instead.
            if (!NetworkClient.ready) NetworkClient.Ready();
            if (autoCreatePlayer)
            {
                NetworkClient.AddPlayer();
            }
        }
    }


}
