using UnityEngine;
using UnityEngine.Networking;

public class LootSpawner : NetworkBehaviour {

    public WeaponSpawn[] weaponSpawns;

    public override void OnStartServer()
    {
        foreach (var spawn in weaponSpawns)
        {
            var weapon = Instantiate(spawn.weaponPrefab, spawn.transform.position, spawn.transform.rotation);
            NetworkServer.Spawn(weapon);
        }
    }
}
