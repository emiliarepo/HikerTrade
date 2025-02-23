using System.Collections.Immutable;
using HikerTrade.Enums;

namespace HikerTrade.Models;

public record Inventory(ImmutableList<Item> Items)
{
    public int GetItemAmount(ItemType itemType)
    {
        return Items.FirstOrDefault(i => i.Type == itemType)?.Quantity ?? 0;
    }

    public int GetTotalPoints()
    {
        return Items.Sum(item => item.TotalPoints);
    }

    public override string ToString()
    {
        return Items.Count == 0 ? "No items" : string.Join(", ", Items);
    }
}