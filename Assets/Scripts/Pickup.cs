using UnityEngine;

public class Pickup : MonoBehaviour {

    private Player player;
    private const int INVENTORY_SIZE = 2;
    private Inventory inventory = new Inventory(INVENTORY_SIZE);

	// Use this for initialization
	void Start () {
        player = GetComponent<Player>();
	}
	
	public void PickUp(Pickupable pickupable)
    {
        pickupable.OnPickUp(this);
    }
    
    public void AddToInventory(Pickupable pickupable)
    {
        if (inventory.HasFreeSpace())
        {
            inventory.Add(pickupable);
        }
    }
}
