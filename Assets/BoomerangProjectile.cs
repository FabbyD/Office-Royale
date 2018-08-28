using UnityEngine;

public class BoomerangProjectile : Projectile {
    
    public float angularSpeed = 720f;

    protected override void OnDirectionChanged(Vector2 direction)
    {
        base.OnDirectionChanged(direction);
        float clockwise = Direction.x > 0 ? -1 : 1;
        GetComponent<Rigidbody2D>().angularVelocity = clockwise * angularSpeed;
    }
}
