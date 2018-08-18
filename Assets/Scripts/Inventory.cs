using UnityEngine;
using UnityEngine.UI;

public class Inventory: MonoBehaviour {

    public int selectedItem = 0;

    private GameObject inventoryUI;

    private const int INVENTORY_SIZE = 2;
    private int index = 0;
    private Pickupable[] items = new Pickupable[INVENTORY_SIZE];

    void Start()
    {
        inventoryUI = GameObject.FindGameObjectWithTag("InventoryUI");
    }

    public void Add(Pickupable item)
    {
        if (HasFreeSpace())
        {
            UpdateCurrentCell(item.GetSprite());
            items[index] = item;
            index++;
        }
    }

    public void Remove()
    {
        index--;
    }

    public bool HasFreeSpace()
    {
        return index < items.Length;
    }

    private void UpdateCurrentCell(Sprite sprite)
    {
        var cell = inventoryUI.transform.GetChild(index);
        var image = cell.GetComponent<Image>();
        image.preserveAspect = true;
        image.sprite = sprite;
    }
}
