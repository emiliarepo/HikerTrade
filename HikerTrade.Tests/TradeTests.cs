using HikerTrade.Enums;
using HikerTrade.Models;
using HikerTrade.Repositories;
using HikerTrade.Services;

namespace HikerTrade.Tests;

public class TradeTests
{
    [Fact]
    public void Trade_Finishes_Successfully()
    {
        // Arrange
        var repository = new InMemoryHikerRepository();
        var tradeService = new TradeService(repository);

        var hiker1Inventory = new Inventory();
        hiker1Inventory.AddItem(ItemType.Food, 4);
        var hiker2Inventory = new Inventory();
        hiker2Inventory.AddItem(ItemType.Water, 3);

        var hiker1 = new Hiker("Alice", 28, Gender.Female, new Coordinates(60.123, 24.1234), false,
            hiker1Inventory);

        var hiker2 = new Hiker("Bob", 18, Gender.Male, new Coordinates(60.123, 24.1234), false,
            hiker2Inventory);

        repository.AddHiker(hiker1);
        repository.AddHiker(hiker2);

        // Act
        tradeService.AttemptItemSwap(hiker1.Id, hiker2.Id);

        // Assert
        Assert.Equal(4, hiker2.Inventory.GetItemAmount(ItemType.Food));
        Assert.Equal(3, hiker1.Inventory.GetItemAmount(ItemType.Water));
    }

    [Fact]
    public void Injured_Hiker_Cant_Trade()
    {
        // Arrange
        var repository = new InMemoryHikerRepository();
        var tradeService = new TradeService(repository);

        var hiker1Inventory = new Inventory();
        hiker1Inventory.AddItem(ItemType.Food, 4);
        var hiker2Inventory = new Inventory();
        hiker2Inventory.AddItem(ItemType.Water, 3);

        var hiker1 = new Hiker("Alice", 28, Gender.Female, new Coordinates(60.123, 24.1234), false,
            hiker1Inventory);

        var hiker2 = new Hiker("Bob", 18, Gender.Male, new Coordinates(60.123, 24.1234), true,
            hiker2Inventory);

        repository.AddHiker(hiker1);
        repository.AddHiker(hiker2);

        // Act & Assert
        Assert.Throws<TradeService.TradeException>(() => tradeService.AttemptItemSwap(hiker1.Id, hiker2.Id));
    }

    [Fact]
    public void Cant_Trade_Different_Amounts()
    {
        // Arrange
        var repository = new InMemoryHikerRepository();
        var tradeService = new TradeService(repository);

        var hiker1Inventory = new Inventory();
        hiker1Inventory.AddItem(ItemType.Food, 4000);
        var hiker2Inventory = new Inventory();
        hiker2Inventory.AddItem(ItemType.Water, 3);

        var hiker1 = new Hiker("Alice", 28, Gender.Female, new Coordinates(60.123, 24.1234), false,
            hiker1Inventory);

        var hiker2 = new Hiker("Bob", 18, Gender.Male, new Coordinates(60.123, 24.1234), false,
            hiker2Inventory);

        repository.AddHiker(hiker1);
        repository.AddHiker(hiker2);

        // Act & Assert
        Assert.Throws<TradeService.TradeException>(() => tradeService.AttemptItemSwap(hiker1.Id, hiker2.Id));
    }

    [Fact]
    public void Cant_Trade_Over_Balance()
    {
        // Arrange
        var repository = new InMemoryHikerRepository();
        var tradeService = new TradeService(repository);

        var hiker1Inventory = new Inventory();
        hiker1Inventory.AddItem(ItemType.Food, 4);
        var hiker2Inventory = new Inventory();
        hiker2Inventory.AddItem(ItemType.Water, 3);

        var hiker1 = new Hiker("Alice", 28, Gender.Female, new Coordinates(60.123, 24.1234), false,
            hiker1Inventory);

        var hiker2 = new Hiker("Bob", 18, Gender.Male, new Coordinates(60.123, 24.1234), false,
            hiker2Inventory);

        repository.AddHiker(hiker1);
        repository.AddHiker(hiker2);

        var expensiveInventory = new Inventory();
        expensiveInventory.AddItem(ItemType.Water, 30000);

        // Act & Assert
        Assert.Throws<TradeService.TradeException>(() =>
            tradeService.AttemptTrade(hiker1.Id, hiker1.Inventory, hiker2.Id, expensiveInventory));
    }

    [Fact]
    public void Can_Swap_Partial_Inventory()
    {
        // Arrange
        var repository = new InMemoryHikerRepository();
        var tradeService = new TradeService(repository);

        var hiker1Inventory = new Inventory();
        hiker1Inventory.AddItem(ItemType.Food, 4);
        hiker1Inventory.AddItem(ItemType.Medication, 8);
        var hiker2Inventory = new Inventory();
        hiker2Inventory.AddItem(ItemType.Food, 20);
        hiker2Inventory.AddItem(ItemType.Water, 3);

        var hiker1 = new Hiker("Alice", 28, Gender.Female, new Coordinates(60.123, 24.1234), false,
            hiker1Inventory);

        var hiker2 = new Hiker("Bob", 18, Gender.Male, new Coordinates(60.123, 24.1234), false,
            hiker2Inventory);

        repository.AddHiker(hiker1);
        repository.AddHiker(hiker2);

        var hiker1Trade = new Inventory();
        hiker1Trade.AddItem(ItemType.Medication, 3);
        var hiker2Trade = new Inventory();
        hiker2Trade.AddItem(ItemType.Food, 5);

        // Act
        tradeService.AttemptTrade(hiker1.Id, hiker1Trade, hiker2.Id, hiker2Trade);

        // Assert
        Assert.Equal(5, hiker1.Inventory.GetItemAmount(ItemType.Medication));
        Assert.Equal(9, hiker1.Inventory.GetItemAmount(ItemType.Food));
        Assert.Equal(15, hiker2.Inventory.GetItemAmount(ItemType.Food));
        Assert.Equal(3, hiker2.Inventory.GetItemAmount(ItemType.Medication));
    }
}