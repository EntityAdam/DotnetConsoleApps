using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceLifetime.Models;

var hostBuilder = Host.CreateApplicationBuilder(args);

hostBuilder.Services
    .AddTransient<IPetStoreTransient, PetStoreTransient>()
    .AddSingleton<IPetStoreSingleton, PetStoreSingleton>();

IHost host = hostBuilder.Build();

PrintHeader("Singleton");
for (int i = 1; i < 5; i++)
{
    var singleton = host.Services.GetRequiredService<IPetStoreSingleton>();
    Console.WriteLine($"Singleton Instance {i} hashcode '{singleton.GetHashCode()}'");
}

PrintHeader("Transient");
for (int i = 1; i < 5; i++)
{
    var transient = host.Services.GetRequiredService<IPetStoreTransient>();
    Console.WriteLine($"Transient Instance {i} hashcode '{transient.GetHashCode()}'");
}

void PrintHeader(string v)
{
    const int HEADER_SIZE = 40;
    Console.WriteLine(new string('-', HEADER_SIZE));
    Console.WriteLine(v.PadLeft(HEADER_SIZE / 2, ' ').PadRight(HEADER_SIZE, ' '));
    Console.WriteLine(new string('-', HEADER_SIZE));
}

namespace ServiceLifetime.Consumer
{
    public class PetCreator();

    public class PetStoreManagerSingleton(IPetStoreSingleton petStoreSingleton1, IPetStoreSingleton petStoreSingleton2)
    {
        private readonly IPetStoreSingleton petStoreSingleton1 = petStoreSingleton1;
        private readonly IPetStoreSingleton petStoreSingleton2 = petStoreSingleton2;

        public Pet Create(Pet pet)
        {
            Console.WriteLine($"Singleton instances the same? {petStoreSingleton1 == petStoreSingleton2}");
            return petStoreSingleton1.Create(pet);
        }

        public IEnumerable<Pet> GetAll() => petStoreSingleton2.GetAll();
    }

    public class PetStoreManagerTransient(IPetStoreTransient petStoreTransient1, IPetStoreTransient petStoreTransient2)
    {
        private readonly IPetStoreTransient petStoreTransient1 = petStoreTransient1;
        private readonly IPetStoreTransient petStoreTransient2 = petStoreTransient2;

        public Pet Create(Pet pet)
        {
            Console.WriteLine($"Transient instances the same? {petStoreTransient1 == petStoreTransient2}");
            return petStoreTransient1.Create(pet);
        }
        public IEnumerable<Pet> GetAll() => petStoreTransient2.GetAll();
    }
}

namespace ServiceLifetime.Models
{
    public record Pet(string Name);

    public interface IPetStoreTransient
    {
        public Pet Create(Pet pet);
        public IEnumerable<Pet> GetAll();
    }
    public interface IPetStoreSingleton
    {
        public Pet Create(Pet pet);
        public IEnumerable<Pet> GetAll();
    }

    public sealed class PetStoreSingleton : IPetStoreSingleton
    {
        private readonly List<Pet> _pets = [];

        public Pet Create(Pet pet)
        {
            _pets.Add(pet);
            return pet;
        }

        public IEnumerable<Pet> GetAll() => _pets;
    }
    public sealed class PetStoreTransient : IPetStoreTransient
    {
        private readonly List<Pet> _pets = [];

        public Pet Create(Pet pet)
        {
            _pets.Add(pet);
            return pet;
        }

        public IEnumerable<Pet> GetAll() => _pets;
    }
}
