using System.Collections.Immutable;
using HikerTrade.Models;

namespace HikerTrade.Services;

public interface IInventoryService
{
    Inventory AddItems(Inventory inventory, IEnumerable<Item> itemsToAdd);
    Inventory RemoveItems(Inventory inventory, IEnumerable<Item> itemsToRemove);
}

public class InventoryService : IInventoryService
{
    public Inventory AddItems(Inventory inventory, IEnumerable<Item> itemsToAdd)
    {
        var items = inventory.Items.ToBuilder();

        foreach (var item in itemsToAdd)
        {
            var existingItem = items.FirstOrDefault(i => i.Type == item.Type);

            if (existingItem != null)
            {
                items.Remove(existingItem);
                items.Add(item with { Quantity = item.Quantity + existingItem.Quantity });
            }
            else
            {
                items.Add(new Item(item.Type, item.Quantity));
            }
        }

        return new Inventory(items.ToImmutableList());
    }

    public Inventory RemoveItems(Inventory inventory, IEnumerable<Item> itemsToRemove)
    {
        var items = inventory.Items.ToBuilder();

        foreach (var item in itemsToRemove)
        {
            var existingItem = items.FirstOrDefault(i => i.Type == item.Type);

            if (existingItem == null) continue;
            items.Remove(existingItem);
            if (existingItem.Quantity > item.Quantity)
                items.Add(item with { Quantity = existingItem.Quantity - item.Quantity });
        }

        return new Inventory(items.ToImmutableList());
    }
}