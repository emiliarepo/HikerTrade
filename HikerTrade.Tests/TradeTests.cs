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

        var hiker1 = new Hiker("Alice", 28, Gender.Female, new Coordinates(60.123, 24.1234), false,
            [new Item(ItemType.Food, 4)]);

        var hiker2 = new Hiker("Bob", 18, Gender.Male, new Coordinates(60.123, 24.1234), false,
            [new Item(ItemType.Water, 3)]);

        repository.AddHiker(hiker1);
        repository.AddHiker(hiker2);

        // Act
        tradeService.AttemptTrade(hiker1.Id, hiker2.Id);

        // Assert
        Assert.Contains(hiker2.Inventory, i => i.Type == ItemType.Food);
        Assert.Contains(hiker1.Inventory, i => i.Type == ItemType.Water);
    }
    
    [Fact]
    public void Injured_Hiker_Cant_Trade()
    {
        // Arrange
        var repository = new InMemoryHikerRepository();
        var tradeService = new TradeService(repository);

        var hiker1 = new Hiker("Alice", 28, Gender.Female, new Coordinates(60.123, 24.1234), false,
            [new Item(ItemType.Food, 4)]);

        var hiker2 = new Hiker("Bob", 18, Gender.Male, new Coordinates(60.123, 24.1234), true,
            [new Item(ItemType.Water, 3)]);

        repository.AddHiker(hiker1);
        repository.AddHiker(hiker2);

        // Act & Assert
        Assert.Throws<TradeService.TradeException>(() => tradeService.AttemptTrade(hiker1.Id, hiker2.Id));
    }
    
    [Fact]
    public void Cant_Trade_Different_Amounts()
    {
        // Arrange
        var repository = new InMemoryHikerRepository();
        var tradeService = new TradeService(repository);

        var hiker1 = new Hiker("Alice", 28, Gender.Female, new Coordinates(60.123, 24.1234), false,
            [new Item(ItemType.Food, 4000)]);

        var hiker2 = new Hiker("Bob", 18, Gender.Male, new Coordinates(60.123, 24.1234), true,
            [new Item(ItemType.Water, 1)]);

        repository.AddHiker(hiker1);
        repository.AddHiker(hiker2);

        // Act & Assert
        Assert.Throws<TradeService.TradeException>(() => tradeService.AttemptTrade(hiker1.Id, hiker2.Id));
    }
}