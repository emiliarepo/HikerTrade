using HikerTrade.Enums;

namespace HikerTrade.Models;

public record Item(ItemType Type, int Quantity)
{
    public int Points => (int)Type;
    public int TotalPoints => Points * Quantity;

    public override string ToString()
    {
        return $"{Quantity}x {Type} ({TotalPoints} pts)";
    }
}