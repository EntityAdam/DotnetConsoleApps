// domain abstractions
public interface IFacade
{
    public Task PersonNew(Person person);
    public Task PersonChangeName(Person person, string name);
    public Task PersonRecordDeath(Person person, DateOnly date);

    Task PlaceNew(Place place);
    Task PlaceChangeAddress(Place place);
    Task PlaceDemolished(Place place);

    Task ResidencyNew(Person person, Place place);
    Task ResidencyChange(Person person, Place newPlace);
}
public interface IPeopleStore
{
    Task ChangeName(Person person, string name);
    Task New(Person person);
}
public interface IPlaceStore { }
public interface IResidencyStore { }

// domain impl
internal sealed class Facade : IFacade
{
    private readonly IPeopleStore peopleStore;
    private readonly IPlaceStore placeStore;
    private readonly IResidencyStore residencyStore;

    public Facade(IPeopleStore peopleStore, IPlaceStore placeStore, IResidencyStore residencyStore)
    {
        this.peopleStore = peopleStore;
        this.placeStore = placeStore;
        this.residencyStore = residencyStore;
    }
    public async Task PersonChangeName(Person person, string name)
    {
        await peopleStore.ChangeName(person, name);
    }

    public async Task PersonNew(Person person)
    {
        if (!Person.IsValid(person)) { throw new ArgumentException(nameof(Person)); }
        await peopleStore.New(person);
    }

    public Task PersonRecordDeath(Person person, DateOnly date) => throw new NotImplementedException();

    public Task PlaceChangeAddress(Place place) => throw new NotImplementedException();

    public Task PlaceDemolished(Place place) => throw new NotImplementedException();

    public Task PlaceNew(Place place) => throw new NotImplementedException();

    public Task ResidencyChange(Person person, Place newPlace) => throw new NotImplementedException();

    public Task ResidencyNew(Person person, Place place) => throw new NotImplementedException();
}
internal sealed class PeopleStore : IPeopleStore
{
    public Task ChangeName(Person person, string name)
    {
        throw new NotImplementedException();
    }

    public Task New(Person person)
    {
        throw new NotImplementedException();
    }
}
internal sealed class PlaceStore : IPlaceStore { }

//unit tests
//[InternalsVisibleTo()]
public class PeopleStoreDummy : IPeopleStore
{
    public Task ChangeName(Person person, string name)
    {
        throw new NotImplementedException();
    }

    public Task New(Person person)
    {
        throw new NotImplementedException();
    }
}
public class PeopleStoreMock : IPeopleStore
{
    private List<Person> _people { get; set; } = new List<Person>();

    public Task ChangeName(Person person, string name)
    {
        var personToChange = _people.Single(p => p.Id == person.Id);
        personToChange.Name = name;
        return Task.CompletedTask;
    }

    public Task New(Person person)
    {
        throw new NotImplementedException();
    }
}
public class PlaceStoreDummy : IPlaceStore { }
public class PlaceStoreMock : IPlaceStore { }



public sealed class Employee { }
public sealed class Person
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    internal static bool IsValid(Person person)
    {
        return !string.IsNullOrEmpty(person.Name);
    }
}
public sealed class Place { }
public sealed class City { }
public sealed class State { }
public sealed class Country { }
public sealed class Residency { }