using UnityEngine;
using UnityEngine.Networking;
using System;

public class NetworkClock : NetworkBehaviour {

    private NetworkClock instance;
    private long ping;
    public float msOffset = 0;

    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(gameObject);
        }

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    public override void OnStartClient()
    {
        if (!isServer)
        {
            ping = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            CmdPing();
            
        }
    }
    
    
    [Command]
    private void CmdPing()
    {
        RpcPong(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
    }

    [ClientRpc]
    private void RpcPong(long pong)
    {
        long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        long round = now - ping;
        long oneway = round / 2;
        msOffset = ping + oneway - pong;
        Debug.Log("RpcPong: " + msOffset);
    }
}
