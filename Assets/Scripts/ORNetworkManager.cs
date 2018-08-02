using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class ORNetworkManager : NetworkManager {

    public GameObject networkClock;

    #region Server

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        base.OnServerAddPlayer(conn, playerControllerId);
        GameManager.instance.PlayerJoined();
    }

    #endregion

    #region Client side

    public override void OnStartClient(NetworkClient client)
    {
        base.OnStartClient(client);
        networkClock.GetComponent<NetworkClock>().InitializeClient(client);
        GameManager.instance.SetupGame();
    }

    #endregion
}
