using UnityEngine;

public class Item : MonoBehaviour {
    

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject hit = collision.gameObject;
        OnPickUp(hit);
        Disappear();
    }

    protected virtual void OnPickUp(GameObject looter)
    {
        Inventory inventory = looter.GetComponent<Inventory>();
        if (inventory != null)
        {
            inventory.Add(this);
        }
    }

    protected virtual void Disappear()
    {
        Destroy(gameObject);
    }

    public virtual Sprite GetSprite()
    {
        return GetComponent<SpriteRenderer>().sprite;
    }
}
