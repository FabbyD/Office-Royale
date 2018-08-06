
using UnityEngine;
using UnityEngine.Networking;

public class Player : PhysicsObject {

    public GameObject weaponPrefab;
    public float speed = 3;
    public float weaponSpawnRadius = 1.5f;

    private Animator animator;
    private NetworkAnimator networkAnimator;

    protected override void AdditionalStart()
    {
        // Get animator
        animator = GetComponent<Animator>();
        networkAnimator = GetComponent<NetworkAnimator>();

        // Disable minimap icon for enemies
        if (!isLocalPlayer)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    protected override void AdditionalUpdate()
    {
        velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * speed;

        Animate();

        CheckFlip();

        if (Input.GetButtonDown("Fire1"))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CmdFire(mousePosition);
        }
    }

    private void Animate()
    {
        string trigger = "Idle";
        string resetTrigger = "Run";
        if (isLocalPlayer)
        {
            if (velocity.magnitude > 0)
            {
                trigger = "Run";
                resetTrigger = "Iddle";
            }
            networkAnimator.SetTrigger(trigger);
        }
        animator.ResetTrigger(resetTrigger);
    }

    private void CheckFlip()
    {
        // <-- Positive scale
        // --> Negative scale
        if (velocity.x < 0 && transform.localScale.x < 0 ||
            velocity.x > 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        }
    }

    [Command]
    void CmdFire(Vector2 towards)
    {
        // Compute direction
        Vector2 center = (Vector2)transform.position;
        Vector2 direction = (towards - center).normalized;

        // Create the Weapon from the Weapon Prefab
        Vector2 spawnPosition = center + direction * weaponSpawnRadius;
        var weapon = Instantiate(
            weaponPrefab,
            spawnPosition,
            Quaternion.identity);

        // Add velocity to the weapon
        Rigidbody2D weaponRb2d = weapon.GetComponent<Rigidbody2D>();
        weaponRb2d.velocity = direction * 8;
        int sign = spawnPosition.x < transform.position.x ? 1 : -1;
        weaponRb2d.angularVelocity = sign * 900f;

        // Add owner of this weapon
        weapon.GetComponent<Weapon>().Owner = netId;

        // Spawn the weapon on the Clients
        NetworkServer.Spawn(weapon);

        // Destroy the bullet after 2 seconds
        Destroy(weapon, 2.0f);
    }




}
