using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ORNetworkTransform : NetworkTransform {

    [SyncVar]
    private Vector3 targetScale;

    [SyncVar]
    private float targetAngularVelocity;

    private Rigidbody2D rb2d;

    private float currentInterval = 0;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        CmdSync(rb2d.angularVelocity, transform.localScale);
    }

    void Update()
    {
        if (!isLocalPlayer || GetNetworkSendInterval() == 0)
        {
            return;
        }

        currentInterval += Time.deltaTime;
        if (currentInterval >= GetNetworkSendInterval())
        {
            currentInterval = 0;
            CmdSync(rb2d.angularVelocity, transform.localScale);
        }
    }

    void LateUpdate()
    {
        if (!isLocalPlayer)
        {
            rb2d.angularVelocity = targetAngularVelocity;
            transform.localScale = targetScale;
        }
    }

    [Command]
    void CmdSync(float angularVelocity, Vector3 scale)
    {
        targetAngularVelocity = angularVelocity;
        targetScale = scale;
    }
}
