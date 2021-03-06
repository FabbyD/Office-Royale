﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {

    public const int maxHealth = 100;

    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;

    public RectTransform healthBar;

    public void TakeDamage(int amount)
    {
        if (!isServer)
        {
            return;
        }

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            RpcElimination();
        }
    }

    public void TakeDamage(Projectile projectile)
    {
        if (!isServer)
        {
            return;
        }

        currentHealth -= projectile.Damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            RpcElimination();

            // Add elimination to player
            AddElimination(projectile.Shooter);
        }
    }

    void OnChangeHealth (int health)
    {
        healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
    }

    [ClientRpc]
    void RpcElimination()
    {
        Destroy(gameObject);
        GameManager.instance.PlayerEliminated(gameObject, isLocalPlayer);
    }

    void AddElimination(NetworkInstanceId to)
    {
        NetworkServer.FindLocalObject(to).GetComponent<ScoreManager>().eliminations++;
    }
}
