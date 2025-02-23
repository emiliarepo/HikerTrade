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
        PerformItemTrade(hiker1, hiker1TradeInventory, hiker2, hiker2TradeInventory);

        hikerRepository.UpdateHiker(hiker1);
        hikerRepository.UpdateHiker(hiker2);
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
            .TrueForAll(item => item.Quantity <= hiker.Inventory.GetItemAmount(item.Type));

        if (!hasSufficientItems)
            throw new TradeException(
                $"Trade between {hiker.Name} and another hiker: Not possible, {hiker.Name} has insufficient items.");
    }

    private void PerformItemTrade(Hiker hiker1, Inventory hiker1TradeInventory, Hiker hiker2,
        Inventory hiker2TradeInventory)
    {
        ExchangeItems(hiker1, hiker2, hiker1TradeInventory);
        ExchangeItems(hiker2, hiker1, hiker2TradeInventory);
    }

    private void ExchangeItems(Hiker giver, Hiker receiver, Inventory inventory)
    {
        foreach (var item in inventory.GetItems())
        {
            receiver.Inventory.AddItem(item.Type, item.Quantity);
            giver.Inventory.RemoveItem(item.Type, item.Quantity);
        }
    }

    public class TradeException(string message) : Exception(message);
}