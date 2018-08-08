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
            pickup.Pick(this);
        }
    }

    public void AddToPlayer(Player player)
    {
        collider.enabled = false;
        OnPickUp(player);
    }

    protected virtual void OnPickUp(Player player)
    {

    }
}
