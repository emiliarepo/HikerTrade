using HikerTrade.Enums;

namespace HikerTrade.Models;

public class Item(ItemType itemType, int quantity)
{
    public ItemType Type { get; } = itemType;
    public int Points => (int)Type;
    public int TotalPoints => Points * Quantity;
    public int Quantity { get; } = quantity;

    public override string ToString()
    {
        return $"{Quantity}x {Type} ({TotalPoints} pts)";
    }
}