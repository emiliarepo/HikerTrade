using HikerTrade.Models;
using HikerTrade.Repositories;

namespace HikerTrade.Services;

public class TradeService(IHikerRepository hikerRepository)
{
    public void AttemptTrade(Guid hiker1Id, Guid hiker2Id)
    {
        var hiker1 = hikerRepository.GetHiker(hiker1Id) ?? throw new TradeException("Hiker not found");
        var hiker2 = hikerRepository.GetHiker(hiker2Id) ?? throw new TradeException("Hiker not found");

        if (hiker1.IsInjured || hiker2.IsInjured)
            throw new TradeException($"Trade between {hiker1.Name} and {hiker2.Name}: Trading with an injured hiker is not possible.");

        var hiker1Points = hiker1.GetTotalInventoryPoints();
        var hiker2Points = hiker2.GetTotalInventoryPoints();

        if (hiker1Points != hiker2Points)
        {
            var comparison = hiker1Points > hiker2Points ? "more" : "fewer";
            throw new TradeException(
                $"Trade between {hiker1.Name} and {hiker2.Name}: Not possible, {hiker1.Name} has {comparison} points to exchange");
        }

        PerformTrade(hiker1, hiker2);

        hikerRepository.UpdateHiker(hiker1);
        hikerRepository.UpdateHiker(hiker2);
    }

    private void PerformTrade(Hiker hiker1, Hiker hiker2)
    {
        List<Item> hiker1Items = hiker1.Inventory.ToList();
        List<Item> hiker2Items = hiker2.Inventory.ToList();

        hiker1.Inventory.Clear();
        hiker2.Inventory.Clear();

        hiker1.Inventory.AddRange(hiker2Items);
        hiker2.Inventory.AddRange(hiker1Items);
    }

    public class TradeException(string message) : Exception(message);
}