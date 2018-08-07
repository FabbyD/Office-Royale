using System;
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
        if (!isLocalPlayer)
        {
            return;
        }

        velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * speed;

        if (Input.GetButtonDown("Fire1"))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            networkAnimator.SetTrigger("Attack");
            CmdFire(mousePosition);
            if (isServer)
            {
                // Bug where animation is played twice when you're also the host
                animator.ResetTrigger("Attack");
            }
        }
    }

    void LateUpdate()
    {
        CheckFlip();
        MovementAnimation();
    }

    private void MovementAnimation()
    {
        Vector2 velocity = GetVelocity();
        animator.SetFloat("Speed", Mathf.Abs(velocity.magnitude));
    }

    private bool IsAnimationActive(string state)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(state);
    }

    private bool AnyAnimationActive(string[] states)
    {
        foreach (string state in states)
        {
            if (IsAnimationActive(state))
            {
                return true;
            }
        }
        return false;
    }

    private void CheckFlip()
    {
        // <-- Positive scale
        // --> Negative scale
        Vector2 velocity = GetVelocity();
        if (velocity.x < 0 && transform.localScale.x < 0 ||
            velocity.x > 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        }
    }

    private Vector2 GetVelocity()
    {
        return isLocalPlayer ? velocity : rb2d.velocity;
    }

    [Command]
    void CmdFire(Vector2 towards)
    {
        // Compute direction
        Vector2 center = transform.position;
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

