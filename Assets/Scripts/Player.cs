using System;
using UnityEngine;
using UnityEngine.Networking;

public class Player : PhysicsObject {
    
    public float speed = 3;
    public Transform weaponPosition;

    private Animator animator;
    private NetworkAnimator networkAnimator;
    private WeaponHolder weaponHolder;

    protected override void AdditionalStart()
    {
        animator = GetComponent<Animator>();
        networkAnimator = GetComponent<NetworkAnimator>();
        weaponHolder = GetComponent<WeaponHolder>();

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
            weaponHolder.CmdFire(weaponHolder.ProjectileSpawnPosition, mousePosition);
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
        // --> Positive scale
        // <-- Negative scale
        Vector2 velocity = GetVelocity();
        if (velocity.x < 0 && transform.localScale.x > 0 ||
            velocity.x > 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        }
    }

    private Vector2 GetVelocity()
    {
        return isLocalPlayer ? velocity : rb2d.velocity;
    }

    

}

