using UnityEngine;
using UnityEngine.Networking;

public class Projectile : NetworkBehaviour {

    public NetworkInstanceId Shooter;
    public int Damage = 10;
    public float speed = 8;

    [SyncVar(hook = "OnDirectionChanged")]
    public Vector2 Direction = Vector2.zero;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isServer)
        {
            return;
        }

        var hit = collision.gameObject;
        var player = hit.GetComponent<Player>();
        if (player != null && player.netId == Shooter ||
            hit.GetComponent<Projectile>() != null)
        {
            // Don't collide with owner or another projectile
            return;
        }

        Destroy(gameObject);
        var health = hit.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(this);
        }
    }

    protected virtual void OnDirectionChanged(Vector2 direction)
    {
        GetComponent<Rigidbody2D>().velocity = direction * speed;
    }
}
