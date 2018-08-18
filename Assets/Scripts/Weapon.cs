using UnityEngine;
using UnityEngine.Networking;

public class Weapon : Item {

    public NetworkInstanceId Owner;
    public int Damage = 10;
    public GameObject projectilePrefab;
    public Vector2 projectileSpawnPosition;

    protected override void OnPickUp(GameObject looter)
    {
        base.OnPickUp(looter);

        var weaponManager = looter.GetComponent<WeaponManager>();
        if (weaponManager)
        {
            weaponManager.UpdateWeapon(this);
        }
    }

    public virtual GameObject Fire(Vector2 towards)
    {
        // Create the projectile from the projectile prefab
        var projectile = Instantiate(
            projectilePrefab,
            projectileSpawnPosition,
            Quaternion.identity);

        // Add velocity to the projectile
        Vector2 direction = (towards - projectileSpawnPosition).normalized;
        Rigidbody2D projectileRb2d = projectile.GetComponent<Rigidbody2D>();
        projectileRb2d.velocity = direction * 8;

        // TODO Boomerang specific
        //int sign = projectileSpawnPosition.x < transform.position.x ? 1 : -1;
        //projectileRb2d.angularVelocity = sign * 900f;

        // Add owner of this weapon
        projectile.GetComponent<Projectile>().Shooter = Owner;

        return projectile;
    }
    
}
