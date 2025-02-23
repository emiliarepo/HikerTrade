using HikerTrade.Models;
using HikerTrade.Repositories;

namespace HikerTrade.Services;

public interface IHikerInfoService
{
    void UpdateInventory(Hiker hiker, Inventory inventory);
}

public class HikerInfoService(IHikerRepository hikerRepository) : IHikerInfoService
{
    public void UpdateInventory(Hiker hiker, Inventory inventory)
    {
        var newHiker = hiker with { Inventory = inventory };
        hikerRepository.UpdateHiker(newHiker);
    }
}