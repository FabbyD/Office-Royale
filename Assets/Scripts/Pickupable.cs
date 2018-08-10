using UnityEngine;

public class Pickupable : MonoBehaviour {

    Collider2D collider;

    private void Start()
    {
        collider = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject hit = collision.gameObject;
        Pickup pickup = hit.GetComponent<Pickup>();
        if (pickup != null)
        {
            collider.enabled = false;
            pickup.PickUp(this);
        }
    }

    public virtual void OnPickUp(Pickup pickup)
    {
        pickup.AddToInventory(this);
    }
}
