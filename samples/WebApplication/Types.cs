using Intellenum;

[Intellenum]
[Members("Freezing, Bracing, Chilly, Cool, Mild, Warm, Balmy, Hot, Sweltering, Scorching")]
public partial class TemperatureSummary
{
}

[Intellenum]
[Members("London, Paris, Peckham")]
public partial class City
{
    public static City RandomMember()
    {
       return City.FromValue(Random.Shared.Next(List().Count()));
    }
}

[Intellenum]
[Members("PaidNextDay, FreeFiveWorkingDays")]
public partial class DeliveryScheme
{
}

public enum DeliverySchemeEnum
{
    PaidNextDay,
    FreeFiveWorkingDays
}

record WeatherForecast(DateOnly Date, decimal TemperatureC, TemperatureSummary Summary, City City);


public class Order
{
    public int OrderId { get; init; } 

    public string CustomerName { get; init; } = "";

    public DeliveryScheme DeliveryScheme { get; set; } = DeliveryScheme.FreeFiveWorkingDays;
}

