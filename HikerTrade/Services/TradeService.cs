using HikerTrade.Models;
using HikerTrade.Repositories;

namespace HikerTrade.Services;

public interface ITradeService
{
    void AttemptItemSwap(Guid hiker1Id, Guid hiker2Id);
    void AttemptTrade(Guid hiker1Id, Inventory hiker1TradeInventory, Guid hiker2Id, Inventory hiker2TradeInventory);
}

public class TradeService(IHikerRepository hikerRepository) : ITradeService
{
    private readonly IHikerInfoService _hikerInfoService = new HikerInfoService(hikerRepository);

    public void AttemptItemSwap(Guid hiker1Id, Guid hiker2Id)
    {
        var hiker1 = hikerRepository.GetHiker(hiker1Id) ??
                     throw new TradeException($"Hiker 1 with id {hiker1Id} not found");
        var hiker2 = hikerRepository.GetHiker(hiker2Id) ??
                     throw new TradeException($"Hiker 2 with id {hiker2Id} not found");
        AttemptTrade(hiker1Id, new Inventory(hiker1.Inventory.GetItems()), hiker2Id,
            new Inventory(hiker2.Inventory.GetItems()));
    }

    public void AttemptTrade(Guid hiker1Id, Inventory hiker1TradeInventory, Guid hiker2Id,
        Inventory hiker2TradeInventory)
    {
        var hiker1 = hikerRepository.GetHiker(hiker1Id) ??
                     throw new TradeException($"Hiker 1 with id {hiker1Id} not found");
        var hiker2 = hikerRepository.GetHiker(hiker2Id) ??
                     throw new TradeException($"Hiker 2 with id {hiker2Id} not found");

        ValidateTrade(hiker1, hiker1TradeInventory, hiker2, hiker2TradeInventory);

        var hiker1Inventory = hiker1.Inventory.AddItems(hiker2TradeInventory.GetItems())
            .RemoveItems(hiker1TradeInventory.GetItems());
        var hiker2Inventory = hiker2.Inventory.AddItems(hiker1TradeInventory.GetItems())
            .RemoveItems(hiker2TradeInventory.GetItems());

        _hikerInfoService.UpdateInventory(hiker1, hiker1Inventory);
        _hikerInfoService.UpdateInventory(hiker2, hiker2Inventory);
    }

    private void ValidateTrade(Hiker hiker1, Inventory hiker1TradeInventory, Hiker hiker2,
        Inventory hiker2TradeInventory)
    {
        if (hiker1.IsInjured || hiker2.IsInjured)
            throw new TradeException(
                $"Trade between {hiker1.Name} and {hiker2.Name}: Trading with an injured hiker is not possible.");

        var hiker1Points = hiker1TradeInventory.GetTotalPoints();
        var hiker2Points = hiker2TradeInventory.GetTotalPoints();

        if (hiker1Points != hiker2Points)
        {
            var comparison = hiker1Points > hiker2Points ? "more" : "fewer";
            throw new TradeException(
                $"Trade between {hiker1.Name} and {hiker2.Name}: Not possible, {hiker1.Name} has {comparison} points to exchange");
        }

        ValidateCanAfford(hiker1, hiker1TradeInventory);
        ValidateCanAfford(hiker2, hiker2TradeInventory);
    }

    private void ValidateCanAfford(Hiker hiker, Inventory inventory)
    {
        var hasSufficientItems = inventory.GetItems()
            .All(item => item.Quantity <= hiker.Inventory.GetItemAmount(item.Type));

        if (!hasSufficientItems)
            throw new TradeException(
                $"Trade between {hiker.Name} and another hiker: Not possible, {hiker.Name} has insufficient items.");
    }

    public class TradeException(string message) : Exception(message);
}