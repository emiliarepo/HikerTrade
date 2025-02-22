using HikerTrade.Enums;
using HikerTrade.Models;
using HikerTrade.Repositories;

namespace HikerTrade.Services;

public class HikerInputService(IHikerRepository hikerRepository)
{
    public Guid CollectHikerData()
    {
        var name = ReadString("Enter name: ");
        var age = ReadInt("Enter age: ");
        var gender = ReadEnum<Gender>("Enter gender (Male/Female/Other): ");
        var longitude = ReadDouble("Enter last known longitude: ");
        var latitude = ReadDouble("Enter last known latitude: ");
        var isInjured = ReadBool("Is the person injured? (true/false): ");
        
        var inventory = Enum.GetValues<ItemType>().Select(item =>
            new Item(item, ReadInt($"Enter amount of {item} (value: {(int)item}): "))).ToList();
        
        var hiker = new Hiker(name, age, gender, new Coordinates(longitude, latitude), isInjured, inventory);
        hikerRepository.AddHiker(hiker);
        return hiker.Id;
    }

    private static string ReadString(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine()!.Trim();
    }

    private static int ReadInt(string prompt)
    {
        while (true)
        {
            if (int.TryParse(ReadString(prompt), out var value)) return value;
            Console.WriteLine("Invalid input. Please enter a valid number.");
        }
    }

    private static double ReadDouble(string prompt)
    {
        while (true)
        {
            if (double.TryParse(ReadString(prompt), out var value)) return value;
            Console.WriteLine("Invalid input. Please enter a valid number.");
        }
    }

    private static bool ReadBool(string prompt)
    {
        while (true)
        {
            if (bool.TryParse(ReadString(prompt), out var value)) return value;
            Console.WriteLine("Invalid input. Please enter 'true' or 'false'.");
        }
    }

    private static T ReadEnum<T>(string prompt) where T : struct, Enum
    {
        while (true)
        {
            if (Enum.TryParse(ReadString(prompt), true, out T value)) return value;
            Console.WriteLine($"Invalid input. Please enter a valid {typeof(T).Name}.");
        }
    }
}