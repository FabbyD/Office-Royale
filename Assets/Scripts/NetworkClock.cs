using UnityEngine;
using UnityEngine.Networking;
using System;

using UnityTime = UnityEngine.Time;

public class NetworkClock : NetworkBehaviour {

    // Network time in seconds
    public static double Time {
        get
        {
            return Now() + ServerOffset;
        }
    }

    // Calculate the mean of the offset between client and server clock
    private static double ServerOffset {
        get
        {
            double mean = 0;
            foreach (double offset in serverOffsets)
            {
                mean += offset;
            }
            mean /= serverOffsets.Length;
            return mean;
        }
    }

    private static void AddOffset(double offset)
    {
        serverOffsets[offsetIndex] = offset;
        offsetIndex = (offsetIndex + 1) % OFFSETS_SIZE;
    }

    private const int OFFSETS_SIZE = 10;
    private static int offsetIndex = 0;
    private static double[] serverOffsets = new double[OFFSETS_SIZE];

    // Return unix timestamp in seconds
    private static double Now()
    {
        return (double)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() / 1000;
    }

    // Interval between each ping
    public int pingInterval = 2;
    private float currentInterval = 0;
    private double lastPingSent = 0;

    // Client to send pings to server
    NetworkClient client;

    public void SetupClient(NetworkClient client)
    {
        this.client = client;
        client.RegisterHandler(ORMsgType.Ping, OnPong);
    }

    private void Start()
    {
        if (isServer)
        {
            NetworkServer.RegisterHandler(ORMsgType.Ping, OnPing);
        }
    }

    private void Update()
    {
        if (isServer)
        {
            // No need to ping itself
            return;
        }

        // Ping the server every time interval
        if (pingInterval > 0)
        {
            currentInterval += UnityTime.deltaTime;
            if (currentInterval >= pingInterval)
            {
                currentInterval = 0;
                SendPing();
            }
        }
    }

    // Send timestamp on provided connection
    private void SendTime(NetworkConnection conn, double timestamp)
    {
        PingMessage pingMessage = new PingMessage();
        pingMessage.timestamp = timestamp;
        conn.Send(ORMsgType.Ping, pingMessage);
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
}
