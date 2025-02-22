using HikerTrade.Models;

namespace HikerTrade.Repositories;

// Abstraction to make it simple to add database support in the future
public interface IHikerRepository
{
    void AddHiker(Hiker hiker);
    Hiker? GetHiker(Guid id);
    List<Hiker> GetAllHikers();
    void UpdateHiker(Hiker hiker);

    void DeleteHiker(Hiker hiker);
}