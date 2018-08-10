public class Inventory {

    public int selectedItem = 0;

    private int index = 0;
    private Pickupable[] items;
	
    public Inventory(int size)
    {
        items = new Pickupable[size];
    }

    public void Add(Pickupable item)
    {
        if (index < items.Length - 1)
        {
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
}
