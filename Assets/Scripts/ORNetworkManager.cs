using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class ORNetworkManager : NetworkManager {

    public GameObject networkClock;

    #region Client side

    public override void OnStartClient(NetworkClient client)
    {
        base.OnStartClient(client);
        networkClock.GetComponent<NetworkClock>().InitializeClient(client);
    }

    #endregion
}
