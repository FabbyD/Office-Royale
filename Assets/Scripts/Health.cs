using System.Collections;
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
            RpcDie();
        }
    }

    public void TakeDamage(Weapon weapon)
    {
        if (!isServer)
        {
            return;
        }

        currentHealth -= weapon.Damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            RpcDie();

            // Add elimination to player
            AddElimination(weapon.Owner);
        }
    }

    void OnChangeHealth (int health)
    {
        healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
    }

    [ClientRpc]
    void RpcDie()
    {
        Destroy(gameObject);
    }

    void AddElimination(NetworkInstanceId to)
    {
        NetworkServer.FindLocalObject(to).GetComponent<ScoreManager>().eliminations++;
    }
}
