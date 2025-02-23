using HikerTrade.Enums;

namespace HikerTrade.Models;

public class Inventory
{
    private readonly List<Item> _items = new();

    public Inventory()
    {
    }

    public Inventory(List<Item> items)
    {
        foreach (var item in items) AddItem(item.Type, item.Quantity);
    }

    public void AddItem(ItemType itemType, int quantity)
    {
        var existingItem = _items.FirstOrDefault(item => item.Type == itemType);
        if (existingItem != null)
            existingItem.IncreaseQuantity(quantity);
        else
            _items.Add(new Item(itemType, quantity));
    }

    public void RemoveItem(ItemType itemType, int quantity)
    {
        var existingItem = _items.FirstOrDefault(item => item.Type == itemType);
        if (existingItem != null)
        {
            existingItem.DecreaseQuantity(quantity);
            if (existingItem.Quantity <= 0) _items.Remove(existingItem);
        }
    }

    public int GetItemAmount(ItemType itemType)
    {
        var item = _items.FirstOrDefault(i => i.Type == itemType);
        return item?.Quantity ?? 0;
    }

    public List<Item> GetItems()
    {
        return _items.ToList();
    }

    public int GetTotalPoints()
    {
        return _items.Sum(item => item.TotalPoints);
    }

    public override string ToString()
    {
        if (_items.Count == 0)
            return "No items";
        return string.Join(", ", _items.Select(item => item.ToString()));
    }
}