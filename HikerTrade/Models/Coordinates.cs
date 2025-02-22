namespace HikerTrade.Models;

public class Coordinates(double longitude, double latitude)
{
    public double Longitude { get; } = longitude;
    public double Latitude { get; } = latitude;

    public override string ToString()
    {
        return $"({Longitude}, {Latitude})";
    }
}