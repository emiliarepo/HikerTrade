using System.Collections.Immutable;
using HikerTrade.Enums;
using HikerTrade.Models;
using HikerTrade.Repositories;
using Sharprompt;

namespace HikerTrade.Services;

public interface IHikerInputService
{
    Guid CreateHikerFromInput();
}

public class HikerInputService(IHikerRepository hikerRepository) : IHikerInputService
{
    public Guid CreateHikerFromInput()
    {
        var name = Prompt.Input<string>("Name", validators: [Validators.Required()]);
        var age = Prompt.Input<int>("Age", validators: [Validators.Required()]);
        var gender = Prompt.Select<Gender>("Gender");
        var longitude = Prompt.Input<double>("Last location longitude", validators: [Validators.Required()]);
        var latitude = Prompt.Input<double>("Last location latitude", validators: [Validators.Required()]);
        var isInjured = Prompt.Confirm("Is the hiker injured?");

        List<Item> items = new();
        foreach (var item in Enum.GetValues<ItemType>())
        {
            var quantity = Prompt.Input<int>($"Enter amount of {item} ({(int)item} pts each)",
                validators: [Validators.Required()]);
            items.Add(new Item(item, quantity));
        }

        var hiker = new Hiker(name, age, gender, new Coordinates(longitude, latitude), isInjured,
            new Inventory(items.Where(item => item.Quantity > 0).ToImmutableList()));
        hikerRepository.AddHiker(hiker);
        return hiker.Id;
    }
}