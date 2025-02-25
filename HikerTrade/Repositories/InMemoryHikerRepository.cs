using HikerTrade.Models;

namespace HikerTrade.Repositories;

public class InMemoryHikerRepository : IHikerRepository
{
    private readonly Dictionary<Guid, Hiker> _hikers = new();

    public Guid AddHiker(Hiker hiker)
    {
        _hikers[hiker.Id] = hiker;
        return hiker.Id;
    }

    public Hiker? GetHiker(Guid id)
    {
        return _hikers.TryGetValue(id, out var hiker) ? hiker : null;
    }

    public List<Hiker> GetAllHikers()
    {
        return _hikers.Values.ToList();
    }

    public void UpdateHiker(Hiker hiker)
    {
        if (_hikers.ContainsKey(hiker.Id)) _hikers[hiker.Id] = hiker;
    }

    public void DeleteHiker(Hiker hiker)
    {
        if (_hikers.ContainsKey(hiker.Id)) _hikers.Remove(hiker.Id);
    }
}