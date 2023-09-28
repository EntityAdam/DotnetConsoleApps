using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;


Dictionary<string, int> snapshotCommitMap = new(StringComparer.OrdinalIgnoreCase)
{
    ["https://github.com/dotnet/docs"] = 16_465,
    ["https://github.com/dotnet/runtime"] = 114_223,
    ["https://github.com/dotnet/installer"] = 22_436,
    ["https://github.com/dotnet/roslyn"] = 79_484,
    ["https://github.com/dotnet/aspnetcore"] = 48_386
};
foreach (var (repo, commitCount) in snapshotCommitMap)
{
    Console.WriteLine($"The {repo} repository had {commitCount:N0} commits as of November 10th, 2021.");
}



public static void Main()
{
    var (city, population, area) = QueryCityData("New York City"); // Do something with the data.
    
    var x = new ValueTuple<string, string, string>()
                                                                   
}


Dog dog = new("Ed", 5);



var (name, age, _, _, _) = dog;
DogLookup lookup = new(name, age);



await _petService.CreateDog(new DogEntity() { Name = dog.Name, Age = dog.Age });


public record DogLookup(string Name, int Age);
public record Dog(string Name, int Age, string BloodType, bool IsVaccinated, bool IsNeutered);


public class DogEntity
{
    public string? Name { get; set; }
    public int Age { get; set; }
}


