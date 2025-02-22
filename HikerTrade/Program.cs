using HikerTrade.Repositories;
using HikerTrade.Services;

namespace HikerTrade;

internal static class Program
{
    private static readonly IHikerRepository HikerRepository = new InMemoryHikerRepository();
    private static readonly ITradeService TradeService = new TradeService(HikerRepository);
    private static readonly IHikerInputService HikerInputService = new HikerInputService(HikerRepository);

    private static void Main()
    {
        Console.WriteLine("--- Enter data for Hiker #1 ---");
        Guid hiker1Id = HikerInputService.CreateHikerFromInput();
        Console.WriteLine("\n--- Enter data for Hiker #2 ---");
        Guid hiker2Id = HikerInputService.CreateHikerFromInput();

        Console.WriteLine("\nHiker data before trade:");
        Console.WriteLine($"\n{HikerRepository.GetHiker(hiker1Id)}");
        Console.WriteLine($"\n{HikerRepository.GetHiker(hiker2Id)}");

        try
        {
            TradeService.AttemptItemSwap(hiker1Id, hiker2Id);
        }
        catch (TradeService.TradeException ex)
        {
            Console.WriteLine($"\n{ex.Message}");
            return;
        }

        var hiker1 = HikerRepository.GetHiker(hiker1Id)!;
        var hiker2 = HikerRepository.GetHiker(hiker2Id)!;

        Console.WriteLine($"\nTrade between {hiker1.Name} and {hiker2.Name}: Trade completed, see updated info below:");
        Console.WriteLine($"\n{hiker1}");
        Console.WriteLine($"\n{hiker2}");
    }
}