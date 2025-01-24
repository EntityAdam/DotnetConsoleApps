using Bogus;
using System.Text;

Console.WriteLine("Hello, World!");
TestCase.GenerateCsvFile(@"C:\temp\input.csv");

class TestCase
{
    internal static List<string> dataColleciton = new List<string>();

    internal static void GenerateCsvFile(string filePath)
    {
        var devices = new Faker<VehicleTelemetryDevice>()
            .StrictMode(true)
            .RuleFor(c => c.Id, f => f.Name.FirstName())
            .RuleFor(c => c.Ip, f => f.Internet.Ip()).Generate(10);

        var telemetry = new Faker<VehicleTelemetryItem>()
            .StrictMode(true)
            .RuleFor(c => c.Device, f => f.PickRandom(devices))
            .RuleFor(c => c.DateTimeOffset, f => f.Date.Future())
            .RuleFor(c => c.VehicleSpeed, f => f.Random.Number(0, 120))
            .GenerateForever();

        int count = 0;
        using var enumerator = telemetry.GetEnumerator();
        while (enumerator.MoveNext() && count < 1000)
        {
            count++;
            var item = enumerator.Current;
            Add($"{item.Device.Id},{item.Device.Ip},{item.DateTimeOffset},{item.VehicleSpeed}");
        }

        Write(filePath);
    }
    private static void Add(string data)
    {
        dataColleciton.Add(data);
    }

    private static void Write(string filepath)
    {
        File.WriteAllLines(filepath, dataColleciton);
    }

    internal class VehicleTelemetryDevice
    {
        public string Id { get; set; }
        public string Ip { get; set; }
    }

    internal class VehicleTelemetryItem
    {
        public VehicleTelemetryDevice Device { get; set; }
        public DateTimeOffset DateTimeOffset { get; set; }
        public int VehicleSpeed { get; set; }
    }


}
