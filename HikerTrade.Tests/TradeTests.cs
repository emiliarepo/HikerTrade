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

        var hiker1Inventory = new Inventory([new Item(ItemType.Food, 4)]);
        var hiker2Inventory = new Inventory([new Item(ItemType.Water, 3)]);

        var hiker1Id = repository.AddHiker(new Hiker("Alice", 28, Gender.Female, new Coordinates(60.123, 24.1234),
            false, hiker1Inventory));
        var hiker2Id = repository.AddHiker(new Hiker("Bob", 18, Gender.Male, new Coordinates(60.123, 24.1234), false,
            hiker2Inventory));

        // Act
        tradeService.AttemptItemSwap(hiker1Id, hiker2Id);

        // Assert
        Assert.Equal(4, repository.GetHiker(hiker2Id)!.Inventory.GetItemAmount(ItemType.Food));
        Assert.Equal(3, repository.GetHiker(hiker1Id)!.Inventory.GetItemAmount(ItemType.Water));
    }

    [Fact]
    public void Injured_Hiker_Cant_Trade()
    {
        // Arrange
        var repository = new InMemoryHikerRepository();
        var tradeService = new TradeService(repository);

        var hiker1Inventory = new Inventory([new Item(ItemType.Food, 4)]);
        var hiker2Inventory = new Inventory([new Item(ItemType.Water, 3)]);

        var hiker1Id = repository.AddHiker(new Hiker("Alice", 28, Gender.Female, new Coordinates(60.123, 24.1234),
            false, hiker1Inventory));
        var hiker2Id = repository.AddHiker(new Hiker("Bob", 18, Gender.Male, new Coordinates(60.123, 24.1234), true,
            hiker2Inventory));

        // Act & Assert
        Assert.Throws<TradeService.TradeException>(() => tradeService.AttemptItemSwap(hiker1Id, hiker2Id));
    }

    [Fact]
    public void Cant_Trade_Different_Amounts()
    {
        // Arrange
        var repository = new InMemoryHikerRepository();
        var tradeService = new TradeService(repository);

        var hiker1Inventory = new Inventory([new Item(ItemType.Food, 4000)]);
        var hiker2Inventory = new Inventory([new Item(ItemType.Water, 3)]);

        var hiker1Id = repository.AddHiker(new Hiker("Alice", 28, Gender.Female, new Coordinates(60.123, 24.1234),
            false, hiker1Inventory));
        var hiker2Id = repository.AddHiker(new Hiker("Bob", 18, Gender.Male, new Coordinates(60.123, 24.1234), false,
            hiker2Inventory));

        // Act & Assert
        Assert.Throws<TradeService.TradeException>(() => tradeService.AttemptItemSwap(hiker1Id, hiker2Id));
    }

    [Fact]
    public void Cant_Trade_Over_Balance()
    {
        // Arrange
        var repository = new InMemoryHikerRepository();
        var tradeService = new TradeService(repository);

        var hiker1Inventory = new Inventory([new Item(ItemType.Food, 4)]);
        var hiker2Inventory = new Inventory([new Item(ItemType.Water, 3)]);

        var hiker1Id = repository.AddHiker(new Hiker("Alice", 28, Gender.Female, new Coordinates(60.123, 24.1234),
            false, hiker1Inventory));
        var hiker2Id = repository.AddHiker(new Hiker("Bob", 18, Gender.Male, new Coordinates(60.123, 24.1234), false,
            hiker2Inventory));

        var expensiveInventory = new Inventory([new Item(ItemType.Food, 4000)]);
        var expensiveInventory2 = new Inventory([new Item(ItemType.Water, 3000)]);

        // Act & Assert
        Assert.Throws<TradeService.TradeException>(() =>
            tradeService.AttemptTrade(hiker1Id, expensiveInventory, hiker2Id, expensiveInventory2));
    }

    [Fact]
    public void Can_Swap_Partial_Inventory()
    {
        // Arrange
        var repository = new InMemoryHikerRepository();
        var tradeService = new TradeService(repository);

        var hiker1Inventory = new Inventory([new Item(ItemType.Food, 4), new Item(ItemType.Medication, 8)]);
        var hiker2Inventory = new Inventory([new Item(ItemType.Food, 20), new Item(ItemType.Water, 3)]);

        var hiker1Id = repository.AddHiker(new Hiker("Alice", 28, Gender.Female, new Coordinates(60.123, 24.1234),
            false, hiker1Inventory));
        var hiker2Id = repository.AddHiker(new Hiker("Bob", 18, Gender.Male, new Coordinates(60.123, 24.1234), false,
            hiker2Inventory));

        var hiker1Trade = new Inventory([new Item(ItemType.Medication, 3)]);
        var hiker2Trade = new Inventory([new Item(ItemType.Food, 5)]);

        // Act
        tradeService.AttemptTrade(hiker1Id, hiker1Trade, hiker2Id, hiker2Trade);

        // Assert
        Assert.Equal(5, repository.GetHiker(hiker1Id)!.Inventory.GetItemAmount(ItemType.Medication));
        Assert.Equal(9, repository.GetHiker(hiker1Id)!.Inventory.GetItemAmount(ItemType.Food));
        Assert.Equal(15, repository.GetHiker(hiker2Id)!.Inventory.GetItemAmount(ItemType.Food));
        Assert.Equal(3, repository.GetHiker(hiker2Id)!.Inventory.GetItemAmount(ItemType.Medication));
    }
}