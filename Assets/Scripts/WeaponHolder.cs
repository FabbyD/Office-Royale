using UnityEngine;
using UnityEngine.Networking;

public class WeaponHolder : NetworkBehaviour {

    public Transform weaponPosition;

    private Weapon weapon;
    public Weapon Weapon {
        get { return weapon; }
        set {
            weapon = value;
            if (weapon != null)
            {
                // Move the weapon to the weapon position
                weapon.gameObject.transform.parent = weaponPosition;
                weapon.transform.localPosition = Vector3.zero;
                weapon.transform.localScale = Vector3.one;
                weapon.transform.localRotation = Quaternion.identity;

                // Set weapon's owner
                weapon.GetComponent<Weapon>().Owner = GetComponent<NetworkIdentity>().netId;
            }
        }
    }

    public Vector2 ProjectileSpawnPosition
    {
        get { return (Vector2)Weapon?.projectileSpawn.position; }
    }

    public bool HasWeapon()
    {
        return weapon != null;
    }

    [Command]
    public void CmdFire(Vector2 from, Vector2 towards)
    {
        if (!HasWeapon())
        {
            return;
        }

        // Instantiate projectile
        GameObject projectile = weapon.Fire(from, towards);

        // Spawn the projectile on the Clients
        NetworkServer.Spawn(projectile);

        // Destroy the projectile after 2 seconds
        Destroy(projectile, 2.0f);
    }
}
