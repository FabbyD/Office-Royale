using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour {

    public SpriteRenderer weaponSpriteRenderer;

    private Weapon weapon;

    public bool HasWeapon()
    {
        return weapon != null;
    }

    public void UpdateWeapon(Weapon weapon)
    {
        this.weapon = weapon;
        UpdateWeaponSprite(weapon.GetSprite());
    }

    public void UpdateWeaponSprite(Sprite sprite)
    {
        weaponSpriteRenderer.sprite = sprite;
    }

    [Command]
    public void CmdFire(Vector2 towards)
    {
        if (!HasWeapon())
        {
            return;
        }

        // Instantiate projectile
        GameObject projectile = weapon.Fire(towards);

        // Spawn the projectile on the Clients
        NetworkServer.Spawn(projectile);

        // Destroy the projectile after 2 seconds
        Destroy(projectile, 2.0f);
    }
}
