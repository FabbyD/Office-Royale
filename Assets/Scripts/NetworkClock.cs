using UnityEngine;
using UnityEngine.Networking;
using System;

using UnityTime = UnityEngine.Time;

public class NetworkClock : NetworkBehaviour {

    public static NetworkClock Instance { get; private set; } = null;

    // Server offset shenanigans
    private const int OFFSETS_SIZE = 10;
    private int offsetIndex = 0;
    private double[] serverOffsets = new double[OFFSETS_SIZE];

    // Singleton pattern
    private void Awake()
    {
        //Check if instance already exists
        if (Instance == null)
        {
            //if not, set instance to this
            Instance = this;
        }
        //If instance already exists and it's not this:
        else if (Instance != this)
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    // Network time in seconds
    public static double Time {
        get
        {
            return Now() + Instance.ServerOffset;
        }
    }

    // Calculate the mean of the offset between client and server clock
    private double ServerOffset {
        get
        {
            if (isServer)
            {
                // No offset on the server
                return 0;
            }

            double mean = 0;
            foreach (double offset in serverOffsets)
            {
                mean += offset;
            }
            mean /= serverOffsets.Length;
            return mean;
        }
    }

    private void AddOffset(double offset)
    {
        serverOffsets[offsetIndex] = offset;
        offsetIndex = (offsetIndex + 1) % OFFSETS_SIZE;
    }

    // Return unix timestamp in seconds
    private static double Now()
    {
        return (double)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() / 1000;
    }

    // Send timestamp on provided connection
    private void SendTime(NetworkConnection conn, double timestamp)
    {
        PingMessage pingMessage = new PingMessage();
        pingMessage.timestamp = timestamp;
        conn.Send(ORMsgType.Ping, pingMessage);
    }

    #region Client
    // Client to send pings to server
    NetworkClient client;

    // Interval between each ping
    public int pingInterval = 2;
    private float currentInterval = 0;
    private double lastPingSent = 0;

    public void InitializeClient(NetworkClient client)
    {
        this.client = client;
        client.RegisterHandler(ORMsgType.Ping, OnPong);

        // Send initial ping
        SendPing();
    }

    private void Update()
    {
        if (isServer)
        {
            // No need to ping itself
            return;
        }

        // Ping the server every time interval
        if (pingInterval > 0 && client != null)
        {
            currentInterval += UnityTime.deltaTime;
            if (currentInterval >= pingInterval)
            {
                currentInterval = 0;
                SendPing();
            }
        }
    }

    // Send ping from client to server
    private void SendPing()
    {
        lastPingSent = Now();
        SendTime(client.connection, lastPingSent);
    }

    // Receive pong on client
    private void OnPong(NetworkMessage msg)
    {
        // Recalculate serverOffset
        double clientTime = Now();
        PingMessage castMsg = msg.ReadMessage<PingMessage>();
        double serverTime = castMsg.timestamp;
        double serverOffset = serverTime - ((clientTime + lastPingSent) / 2);
        AddOffset(serverOffset);
    }

    #endregion

    #region Server

    private void Start()
    {
        if (isServer)
        {
            NetworkServer.RegisterHandler(ORMsgType.Ping, OnPing);
        }
    }

    // Send pong from server to client
    private void SendPong(NetworkConnection conn)
    {
        SendTime(conn, Now());
    }

    // Receive ping on server
    private void OnPing(NetworkMessage msg)
    {
        SendPong(msg.conn);
    }

    #endregion
}
