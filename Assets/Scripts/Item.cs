using UnityEngine;
using UnityEngine.Networking;

public class Item : NetworkBehaviour {
    

    void OnTriggerEnter2D(Collider2D collision)
    {
        //if (isServer)
        //{
        GameObject hit = collision.gameObject;
        if (OnPickUp(hit))
        {
            // Disable trigger collider
            GetComponent<Collider2D>().enabled = false;
        }
        //}
    }

    // Returns true if the looter could pick up the item
    protected virtual bool OnPickUp(GameObject looter)
    {
        Inventory inventory = looter.GetComponent<Inventory>();
        return inventory?.Add(this) ?? false;
    }

    public virtual Sprite GetSprite()
    {
        return GetComponent<SpriteRenderer>().sprite;
    }

    public virtual void Selected(GameObject gameObject)
    {

    }

    public virtual void Unselected(GameObject gameObject)
    {

    }

    public virtual void Drop()
    {
        // Reenable trigger collider so it can be picked up again
        GetComponent<Collider2D>().enabled = true;
    }
}
