using HikerTrade.Enums;

namespace HikerTrade.Models;

public record Hiker(string Name, int Age, Gender Gender, Coordinates LastLocation, bool IsInjured, Inventory Inventory)
{
    public Guid Id { get; } = Guid.NewGuid();
    public override string ToString()
    {
        return $"{Name} ({Age}, {Gender}) - Location: ({LastLocation.Longitude}, {LastLocation.Latitude})\n" +
               $"Injured: {(IsInjured ? "Yes" : "No")}\n" +
               $"Inventory: {Inventory} - Total Points: {Inventory.GetTotalPoints()}";
    }
}