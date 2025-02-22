using HikerTrade.Enums;

namespace HikerTrade.Models;

public class Hiker(string name, int age, Gender gender, Coordinates lastLocation, bool isInjured, List<Item> inventory)
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; } = name;
    public int Age { get; } = age;
    public Gender Gender { get; } = gender;
    public Coordinates LastLocation { get; } = lastLocation;
    public bool IsInjured { get; } = isInjured;
    public List<Item> Inventory { get; } = inventory;

    public int GetTotalInventoryPoints()
    {
        return Inventory.Sum(item => item.TotalPoints);
    }

    public override string ToString()
    {
        var inventoryString = Inventory.Count > 0
            ? string.Join(", ", Inventory)
            : "No items";

        return $"{Name} ({Age}, {Gender}) - Location: ({LastLocation.Longitude}, {LastLocation.Latitude})\n" +
               $"Injured: {(IsInjured ? "Yes" : "No")}\n" +
               $"Inventory: {inventoryString} - Total Points: {GetTotalInventoryPoints()}";
    }
}