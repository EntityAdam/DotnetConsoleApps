using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceLifetime.Models;

var hostBuilder = Host.CreateApplicationBuilder(args);

hostBuilder.Services
    .AddTransient<IPetStoreTransient, PetStoreTransient>()
    .AddScoped<IPetStoreScoped, PetStoreScoped>();

IHost host = hostBuilder.Build();

PrintHeader("Transient");
for (int i = 1; i < 5; i++)
{
    var singleton = host.Services.GetRequiredService<IPetStoreTransient>();
    Console.WriteLine($"Transient Instance {i} hashcode '{singleton.GetHashCode()}'");
}

PrintHeader("Scoped");
for (int i = 1; i < 5; i++)
{
    var transient = host.Services.GetRequiredService<IPetStoreScoped>();
    Console.WriteLine($"Scoped Instance {i} hashcode '{transient.GetHashCode()}'");
}

PrintHeader("Doing Work Transient");
using (var manager1 = new PetStoreManagerTransient(host.Services.GetRequiredService<IServiceScopeFactory>()))
{
    for (int i = 1; i < 5; i++)
    {
        manager1.Create(new("Dog"));
        Console.WriteLine($"Transient: Found '{manager1.GetAll().Count()}' Pets");
    }
}

PrintHeader("Doing Work Scoped");
using var manager2 = new PetStoreManagerScoped(host.Services.GetRequiredService<IServiceScopeFactory>());
for (int i = 1; i < 5; i++)
{
    manager2.Create(new("Dog"));
    Console.WriteLine($"Scoped: Found '{manager2.GetAll().Count()}' Pets");
}


void PrintHeader(string v)
{
    const int HEADER_SIZE = 40;
    Console.WriteLine(new string('-', HEADER_SIZE));
    Console.WriteLine(v.PadLeft(HEADER_SIZE / 2, ' ').PadRight(HEADER_SIZE, ' '));
    Console.WriteLine(new string('-', HEADER_SIZE));
}

public class PetStoreManagerTransient : IDisposable
{
    private readonly IServiceScope scope;
    private readonly IPetStoreTransient petStoreTransient1;
    private readonly IPetStoreTransient petStoreTransient2;
    private bool disposedValue;

    public PetStoreManagerTransient(IServiceScopeFactory scopeFactory)
    {
        scope = scopeFactory.CreateScope();
        petStoreTransient1 = scope.ServiceProvider.GetRequiredService<IPetStoreTransient>();
        Console.WriteLine($"Transient: Got PetStore '{petStoreTransient1.GetHashCode()}' in scope '{scope.GetHashCode()}'");
        petStoreTransient2 = scope.ServiceProvider.GetRequiredService<IPetStoreTransient>();
        Console.WriteLine($"Transient: Got PetStore '{petStoreTransient2.GetHashCode()}' in scope '{scope.GetHashCode()}'");
    }
    public Pet Create(Pet pet)
    {
        using var s = scope;
        Console.WriteLine($"Transient: Creating Pet in PetStore '{petStoreTransient1.GetHashCode()}' in scope '{scope?.GetHashCode().ToString() ?? "Disposed"}'");
        return petStoreTransient1.Create(pet);
    }

    public IEnumerable<Pet> GetAll()
    {
        using var s = scope;
        Console.WriteLine($"Transient: Reading PetStore '{petStoreTransient2.GetHashCode()}' in scope '{scope.GetHashCode().ToString() ?? "Disposed"}'");
        return petStoreTransient2.GetAll();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                scope.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Console.WriteLine($"Transient: Disposing Scope '{scope.GetHashCode()}'");
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}

public class PetStoreManagerScoped : IDisposable
{
    private readonly IServiceScope scope;
    private readonly IPetStoreScoped petStoreScoped1;
    private readonly IPetStoreScoped petStoreScoped2;
    private bool disposedValue;

    public PetStoreManagerScoped(IServiceScopeFactory scopeFactory)
    {
        scope = scopeFactory.CreateScope();
        petStoreScoped1 = scope.ServiceProvider.GetRequiredService<IPetStoreScoped>();
        Console.WriteLine($"Scoped: Got PetStore '{petStoreScoped1.GetHashCode()}' in scope '{scope.GetHashCode()}'");
        petStoreScoped2 = scope.ServiceProvider.GetRequiredService<IPetStoreScoped>();
        Console.WriteLine($"Scoped: Got PetStore '{petStoreScoped2.GetHashCode()}' in scope '{scope.GetHashCode()}'");
    }
    public Pet Create(Pet pet)
    {
        using var s = scope;
        Console.WriteLine($"Scoped: Creating Pet in PetStore '{petStoreScoped1.GetHashCode()}' in scope '{scope?.GetHashCode().ToString() ?? "Disposed"}'");
        return petStoreScoped1.Create(pet);
    }

    public IEnumerable<Pet> GetAll()
    {
        using var s = scope;
        Console.WriteLine($"Scoped: Reading PetStore '{petStoreScoped2.GetHashCode()}' in scope '{scope.GetHashCode().ToString() ?? "Disposed"}'");
        return petStoreScoped2.GetAll();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                scope.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Console.WriteLine($"Scoped: Disposing Scope '{scope.GetHashCode()}'");
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
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
    public interface IPetStoreScoped
    {
        public Pet Create(Pet pet);
        public IEnumerable<Pet> GetAll();
    }

    public sealed class PetStoreScoped : IPetStoreScoped
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