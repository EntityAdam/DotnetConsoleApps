using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

var x = new ResourceTypeCodeBillabilityMap()
{
    Map = new List<ResourceTypeCodeBillability>
    {
        new() { ResourceTypeCodeName = "Billable Resource", ResourceTypeCode = "Billable", TargetBillability = 95m },
        new() { ResourceTypeCodeName = "Billable Manager", ResourceTypeCode = "BillableManager", TargetBillability = 50m },
        new() { ResourceTypeCodeName = "Billable Subcontractor", ResourceTypeCode = "BillableSub", TargetBillability = 100m },
        new() { ResourceTypeCodeName = "Non-Billable Resource", ResourceTypeCode = "NonBillable", TargetBillability = 0m },
        new() { ResourceTypeCodeName = "Billability Target 40%", ResourceTypeCode = "BT40", TargetBillability = 40m },
        new() { ResourceTypeCodeName = "Billability Target 72%", ResourceTypeCode = "BT72", TargetBillability = 72m },
        new() { ResourceTypeCodeName = "Billability Target 87%", ResourceTypeCode = "BT87", TargetBillability = 87m },
        new() { ResourceTypeCodeName = "Billability Target 93%", ResourceTypeCode = "BT93", TargetBillability = 93m },
        new() { ResourceTypeCodeName = "Billability Target 95%", ResourceTypeCode = "BT95", TargetBillability = 95m },
        new() { ResourceTypeCodeName = "Billable Contractor", ResourceTypeCode = "BC", TargetBillability = 95m },
        new() { ResourceTypeCodeName = "Billable IDC", ResourceTypeCode = "BIDC95", TargetBillability = 95m },
        new() { ResourceTypeCodeName = "Billability Target 0%", ResourceTypeCode = "BT00", TargetBillability = 0m },
    }
};

var y = new ResourceTypeCodeBillabilityMap2()
{
    Map = new Dictionary<string, decimal>
    {
        { "Billable", 95m },
        { "BillableManager", 50m },
        { "BillableSub", 100m },
        { "NonBillable", 0m },
        { "BT40", 40m },
        { "BT72", 72m },
        { "BT87", 87m },
        { "BT93", 93m },
        { "BT95", 95m },
        { "BC", 95m },
        { "BIDC95", 95m },
        { "BT00", 0m },
    }
};

var serialized = System.Text.Json.JsonSerializer.Serialize(x);
var serialized1 = System.Text.Json.JsonSerializer.Serialize(y);
var serialized2 = JsonConvert.SerializeObject(y);
Console.WriteLine(JsonConvert.SerializeObject(y));

File.WriteAllText(@"C:\temp\map.json", serialized);
File.WriteAllText(@"C:\temp\map1.json", serialized1);
File.WriteAllText(@"C:\temp\map2.json", serialized2);

var deserialized1 = JsonConvert.DeserializeObject<ResourceTypeCodeBillabilityMap2>(File.ReadAllText(@"C:\temp\map1.json"));
var deserialized2 = System.Text.Json.JsonSerializer.Deserialize<ResourceTypeCodeBillabilityMap2>(File.ReadAllText(@"C:\temp\map1.json"));
var deserialized3 = JsonConvert.DeserializeObject<ResourceTypeCodeBillabilityMap2>(File.ReadAllText(@"C:\temp\map1.json"));
Console.WriteLine("done");

var yy = deserialized3.Map.ContainsKey("BT00");
var zz = deserialized3.Map.ContainsKey("FOOP");
var x2 = deserialized3.Map["BT00"];
var x3 = deserialized3.Map["FOOP"]; 



public class ResourceTypeCodeBillabilityMap
{
    public List<ResourceTypeCodeBillability> Map { get; set; } = new List<ResourceTypeCodeBillability>();
}

public class ResourceTypeCodeBillabilityMap2
{
    public Dictionary<string, decimal> Map { get; set; } = new();
}

public class ResourceTypeCodeBillability
{
    public string ResourceTypeCodeName { get; set; } = string.Empty;
    public string ResourceTypeCode { get; set; } = string.Empty;
    public decimal TargetBillability { get; set; }
}