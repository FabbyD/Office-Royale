using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Boomerang : Weapon {

    public float angularSpeed = 720;

    public override void SetProjectileMovement(Rigidbody2D rb2d, Vector2 direction)
    {
        rb2d.velocity = direction * speed;

        float clockwise = direction.x >= 0 ? -1 : 1;
        rb2d.angularVelocity = clockwise * angularSpeed;
    }

    public override Sprite GetSprite()
    {
        return transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
    }
}
