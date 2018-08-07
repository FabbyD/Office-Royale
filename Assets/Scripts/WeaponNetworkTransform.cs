using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponNetworkTransform : NetworkTransform {

    [SyncVar]
    private float targetAngularVelocity;

    private Rigidbody2D rb2d;

    private float currentInterval = 0;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        if (isServer)
        {
            RpcSync(rb2d.angularVelocity);
        }
    }

    void Update()
    {
        if (!isServer)
        {
            rb2d.angularVelocity = targetAngularVelocity;
            return;
        }

        if (GetNetworkSendInterval() == 0)
        {
            return;
        }

        currentInterval += Time.deltaTime;
        if (currentInterval >= GetNetworkSendInterval())
        {
            currentInterval = 0;
            RpcSync(rb2d.angularVelocity);
        }
    }

    [ClientRpc]
    void RpcSync(float angularVelocity)
    {
        targetAngularVelocity = angularVelocity;
    }
}
