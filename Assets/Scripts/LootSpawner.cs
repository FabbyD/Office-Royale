using UnityEngine;
using UnityEngine.Networking;

public class LootSpawner : NetworkBehaviour {

    public override void OnStartServer()
    {
        foreach (var spawn in FindObjectsOfType<WeaponSpawn>())
        {
            var weapon = Instantiate(spawn.weaponPrefab, spawn.transform.position, spawn.transform.rotation);
            NetworkServer.Spawn(weapon);
        }
    }
}
