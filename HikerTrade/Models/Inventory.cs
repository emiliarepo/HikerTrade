using System.Collections.Immutable;
using HikerTrade.Enums;

namespace HikerTrade.Models;

public class Inventory(IEnumerable<Item> items)
{
    private readonly ImmutableList<Item> _items = items.Where(item => item.Quantity > 0).ToImmutableList();

    public Inventory AddItems(IEnumerable<Item> itemsToAdd)
    {
        var items = _items.ToBuilder();

        foreach (var item in itemsToAdd)
        {
            var existingItem = items.FirstOrDefault(i => i.Type == item.Type);

            if (existingItem != null)
            {
                items.Remove(existingItem);
                items.Add(new Item(item.Type, item.Quantity + existingItem.Quantity));
            }
            else
            {
                items.Add(new Item(item.Type, item.Quantity));
            }
        }

        return new Inventory(items);
    }

    public Inventory RemoveItems(IEnumerable<Item> itemsToRemove)
    {
        var items = _items.ToBuilder();

        foreach (var item in itemsToRemove)
        {
            var existingItem = items.FirstOrDefault(i => i.Type == item.Type);

            if (existingItem == null) continue;
            items.Remove(existingItem);
            if (existingItem.Quantity > item.Quantity)
                items.Add(new Item(item.Type, existingItem.Quantity - item.Quantity));
        }

        return new Inventory(items);
    }

    public int GetItemAmount(ItemType itemType)
    {
        return _items.FirstOrDefault(i => i.Type == itemType)?.Quantity ?? 0;
    }

    public IReadOnlyList<Item> GetItems()
    {
        return _items;
    }

    public int GetTotalPoints()
    {
        return _items.Sum(item => item.TotalPoints);
    }

    public override string ToString()
    {
        return _items.Count == 0 ? "No items" : string.Join(", ", _items);
    }
}