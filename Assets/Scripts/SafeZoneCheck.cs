using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZoneCheck : MonoBehaviour {

    public int damage = 5;
    public int damageRate = 1;

    private Health health;
    public bool isSafe = true;
    private float lastDamageTick = 0;

	// Use this for initialization
	void Start () {
        health = GetComponent<Health>();
	}

    void Update()
    {
        if (!isSafe && lastDamageTick >= damageRate)
        {
            lastDamageTick = 0;
            health.TakeDamage(damage);
        } else
        {
            lastDamageTick += Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Safe Zone")
        {
            isSafe = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Safe Zone")
        {
            isSafe = false;
        }
    }
}
