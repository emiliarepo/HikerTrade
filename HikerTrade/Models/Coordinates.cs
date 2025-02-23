namespace HikerTrade.Models;

public record Coordinates(double Longitude, double Latitude)
{
    public override string ToString()
    {
        return $"({Longitude}, {Latitude})";
    }
}