using UnityEngine;
using UnityEngine.Networking;

public class Weapon : Item {

    public NetworkInstanceId Owner;
    public GameObject projectilePrefab;
    public Transform projectileSpawn;
    public float speed = 8;

    public virtual GameObject Fire(Vector2 from, Vector2 towards)
    {
        // Create the projectile from the projectile prefab
        Vector2 direction = (towards - from).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        var projectile = Instantiate(
            projectilePrefab,
            from,
            Quaternion.AngleAxis(angle, Vector3.forward));

        var projectileRb2d = projectile.GetComponent<Rigidbody2D>();
        SetProjectileMovement(projectileRb2d, direction);
        
        // Add owner of this weapon
        projectile.GetComponent<Projectile>().Shooter = Owner;

        return projectile;
    }

    public virtual void SetProjectileMovement(Rigidbody2D rb2d, Vector2 direction)
    {
        rb2d.velocity = direction * speed;
    }


    public override void Selected(GameObject gameObject)
    {
        var weaponHolder = gameObject.GetComponent<WeaponHolder>();
        if (weaponHolder != null)
        {
            weaponHolder.Weapon = this;
        }
    }

    public override void Unselected(GameObject gameObject)
    {
        var weaponHolder = gameObject.GetComponent<WeaponHolder>();
        if (weaponHolder != null)
        {
            weaponHolder.Weapon = null;
        }
    }
}
