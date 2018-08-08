using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    private Player player;

	// Use this for initialization
	void Start () {
        player = GetComponent<Player>();
	}
	
	public void Pick(Pickupable pickupable)
    {
        pickupable.AddToPlayer(player);
    }
}
