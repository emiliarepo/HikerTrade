using HikerTrade.Enums;

namespace HikerTrade.Models;

public class Hiker(string name, int age, Gender gender, Coordinates lastLocation, bool isInjured, Inventory inventory)
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; } = name;
    public int Age { get; } = age;
    public Gender Gender { get; } = gender;
    public Coordinates LastLocation { get; } = lastLocation;
    public bool IsInjured { get; } = isInjured;
    public Inventory Inventory { get; } = inventory;

    public override string ToString()
    {
        return $"{Name} ({Age}, {Gender}) - Location: ({LastLocation.Longitude}, {LastLocation.Latitude})\n" +
               $"Injured: {(IsInjured ? "Yes" : "No")}\n" +
               $"Inventory: {Inventory} - Total Points: {Inventory.GetTotalPoints()}";
    }
}