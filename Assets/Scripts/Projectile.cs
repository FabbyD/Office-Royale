using UnityEngine;
using UnityEngine.Networking;

public class Projectile : MonoBehaviour {

    public NetworkInstanceId Shooter;
    public int Damage;
    public float speed = 8;

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

    public GameObject Fire(GameObject prefab, Vector2 position, Vector2 direction)
    {
        var projectile = Instantiate(
            prefab,
            position,
            Quaternion.LookRotation(direction));

        // Add velocity to the projectile
        //Rigidbody2D projectileRb2d = projectile.GetComponent<Rigidbody2D>();
        //projectileRb2d.velocity = direction * speed;

        return projectile;
    }
}
