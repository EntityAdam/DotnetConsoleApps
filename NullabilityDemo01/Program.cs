var person1 = new Person("Adam", 39);

Person person2 = new("Adam", 39);

using var personService = new PersonService();

var person3 = personService.GetPerson();


if (person1.Name == "Adam")
{
    Console.WriteLine($"Hello {person1.Name}");
}

if (person2.Name == "Adam")
{
    Console.WriteLine($"Hello {person1.Name}");
}



public record Person(string Name, int Age);









public class PersonService : IDisposable
{
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    internal Person GetPerson()
    {
        throw new NotImplementedException();
    }
}