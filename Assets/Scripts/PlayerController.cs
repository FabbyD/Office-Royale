using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PhysicsObject {

    public GameObject weaponPrefab;
    public Transform weaponSpawn;
    public float speed = 3;

    protected override void AdditionalStart()
    {
        
    }

    protected override void AdditionalUpdate()
    {
        velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * speed;

        if (Input.GetButtonDown("Fire1"))
        {
            CmdFire();
        }
    }

    void CmdFire()
    {
        // Create the Bullet from the Bullet Prefab
        var weapon = Instantiate(
            weaponPrefab,
            weaponSpawn.position,
            weaponSpawn.rotation);

        // Add velocity to the bullet
        weapon.GetComponent<Rigidbody2D>().velocity = Vector2.right * 8;

        //// Spawn the bullet on the Clients
        //NetworkServer.Spawn(bullet);

        // Destroy the bullet after 2 seconds
        Destroy(weapon, 2.0f);

    }




}
