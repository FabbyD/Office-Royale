
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : PhysicsObject {

    public GameObject weaponPrefab;
    public float speed = 3;

    private float spawnRadius = 1;

    protected override void AdditionalStart()
    {
        
    }

    protected override void AdditionalUpdate()
    {
        velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * speed;

        if (Input.GetButtonDown("Fire1"))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CmdFire(mousePosition);
        }
    }

    [Command]
    void CmdFire(Vector2 towards)
    {
        // Compute direction
        Vector2 direction = (towards - (Vector2)transform.position).normalized;

        // Create the Weapon from the Weapon Prefab
        Vector2 spawnPosition = (Vector2)transform.position + direction * spawnRadius;
        var weapon = Instantiate(
            weaponPrefab,
            spawnPosition,
            Quaternion.identity);

        // Add velocity to the weapon
        Rigidbody2D weaponRb2d = weapon.GetComponent<Rigidbody2D>();
        weaponRb2d.velocity = direction * 8;
        weaponRb2d.angularVelocity = -900f;

        // Spawn the weapon on the Clients
        NetworkServer.Spawn(weapon);

        // Destroy the bullet after 2 seconds
        Destroy(weapon, 2.0f);
    }




}
