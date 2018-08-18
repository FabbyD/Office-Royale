using UnityEngine;
using UnityEngine.UI;

public class Inventory: MonoBehaviour {

    private int selectedIndex = 0;
    public int SelectedIndex {
        get { return selectedIndex; }
        set { selectedIndex = value; ItemSelected(); }
    }

    private GameObject inventoryUI;

    private const int INVENTORY_SIZE = 2;
    private Item[] items = new Item[INVENTORY_SIZE];

    void Start()
    {
        inventoryUI = GameObject.FindGameObjectWithTag("InventoryUI");
    }

    // Returns true if the inventory had enough space to add the item
    public bool Add(Item item)
    {
        int freeIndex = FindNextFreeSpace();
        if (freeIndex >= 0)
        {
            items[freeIndex] = item;
            UpdateCellUI(freeIndex, item.GetSprite());
            if (freeIndex == selectedIndex)
            {
                item.Selected(gameObject);
            }
            return true;
        }
        return false;
    }

    public void Remove()
    {
        var item = items[selectedIndex];
        items[selectedIndex] = null;
        item.Drop();
        item.Unselected(gameObject);
    }

    public bool HasFreeSpace()
    {
        return FindNextFreeSpace() >= 0;
    }

    private int FindNextFreeSpace()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    private void UpdateCellUI(int index, Sprite sprite)
    {
        var cell = inventoryUI.transform.GetChild(index);
        var image = cell.GetComponent<Image>();
        image.preserveAspect = true;
        image.sprite = sprite;
    }

    private void ItemSelected()
    {
    }
}
