using UnityEngine;
using UnityEngine.Networking;

public class Projectile : MonoBehaviour {

    public NetworkInstanceId Shooter;
    public int Damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
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
}
