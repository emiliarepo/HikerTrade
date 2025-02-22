namespace HikerTrade;

using Models;
using Repositories;
using Services;

internal static class Program
{
    private static readonly IHikerRepository HikerRepository = new InMemoryHikerRepository();
    private static readonly TradeService TradeService = new(HikerRepository);
    private static readonly HikerInputService HikerInputService = new(HikerRepository);

    private static void Main()
    {
        Console.WriteLine("--- Enter data for Hiker #1 ---");
        Guid hiker1Id = HikerInputService.CollectHikerData();
        Console.WriteLine("\n--- Enter data for Hiker #2 ---");
        Guid hiker2Id = HikerInputService.CollectHikerData();
        
        Console.WriteLine("\nHiker data before trade:");
        Console.WriteLine($"\n{HikerRepository.GetHiker(hiker1Id)}");
        Console.WriteLine($"\n{HikerRepository.GetHiker(hiker2Id)}");
        
        try
        {
            TradeService.AttemptTrade(hiker1Id, hiker2Id);
        }
        catch (TradeService.TradeException ex)
        {
            Console.WriteLine($"\n{ex.Message}");
            return;
        }
        
        Hiker hiker1 = HikerRepository.GetHiker(hiker1Id)!;
        Hiker hiker2 = HikerRepository.GetHiker(hiker2Id)!;
        
        Console.WriteLine($"\nTrade between {hiker1.Name} and {hiker2.Name}: Trade completed, see updated info below:");
        Console.WriteLine($"\n{hiker1}");
        Console.WriteLine($"\n{hiker2}");
    }
}