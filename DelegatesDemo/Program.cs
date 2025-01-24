// I'm Bob.
Person bob = new Person("Bob");

// I'm going to the Playground.
// If I want to move about the playground, I have to skip.
Location location1 = new Playground();
location1.Move(bob);

// I'm going to the Ministry of Silly Walks.
// I hope my walk is silly enough.
Location location2 = new MinistryOfSillyWalks();
location2.Move(bob);


internal sealed class Person(string Name)
{
    public string Name { get; } = Name;
}

// At a location, we have one job. That job is to make sure people move.
// We don't particularly care HOW the people move.
// Each location can choose their own rules.
// The default is for a person to walk at a normal pace.
abstract class Location
{
    public delegate void MoveDelegate(string name);

    public MoveDelegate MakeMove { get; set; } = Walk;

    public virtual void Move(Person person)
    {
        MakeMove(person.Name);
    }

    public static void Walk(string name) => Console.WriteLine($"{name} is walking!");

    public static void Run(string name) => Console.WriteLine($"{name} is running!");

    public static void Skip(string name) => Console.WriteLine($"{name} is skipping!");

    public static void SillyWalk(string name) => Console.WriteLine($"{name} is not silly enough!");
}

// At our playground people must skip.
internal sealed class Playground : Location
{
    public Playground()
    {
        MakeMove = Skip;
    }

    public override void Move(Person person)
    {
        MakeMove(person.Name);
    }
}

// At the ministry of silly walks, your walk better be silly or else you're fired.
internal sealed class MinistryOfSillyWalks : Location
{
    public MinistryOfSillyWalks()
    {
        MakeMove = SillyWalk;
    }

    public override void Move(Person person)
    {
        MakeMove(person.Name);
    }
}