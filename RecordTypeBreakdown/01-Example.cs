using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace RecordTypeBreakdown;

public class Example01
{
    internal sealed class Dog(string Name, int Age);
    internal sealed record Pig(string Name, int Age) : EmptyRecordBase;
    internal sealed record Hamster(string Name, int Age) : EmptyRecordBase;
    internal sealed class Elephant : IEquatable<Elephant>
    {
        public Elephant(string Name, int Age)
        {
            this.Name = Name;
            this.Age = Age;

        }

        public string Name { get; set /*init*/; }

        public int Age { get; set /*init*/; }

        private Elephant(Elephant original)
        {
            Name = original.Name;
            Age = original.Age;
        }

        public void Deconstruct(out string Name, out int Age)
        {
            Name = this.Name;
            Age = this.Age;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Dog");
            stringBuilder.Append(" { ");
            if (PrintMembers(stringBuilder))
            {
                stringBuilder.Append(' ');
            }
            stringBuilder.Append('}');
            return stringBuilder.ToString();
        }

        public static bool operator !=(Elephant? left, Elephant? right)
        {
            return !(left == right);
        }


        public static bool operator ==(Elephant? left, Elephant? right)
        {
            return (object)left == right || (left?.Equals(right) ?? false);
        }


        public override int GetHashCode()
        {
            return (EqualityComparer<Type>.Default.GetHashCode(EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(Age);
        }


        public override bool Equals(object? obj)
        {
            return Equals(obj as Elephant);
        }


        public bool Equals(Elephant? other)
        {
            return (object)this == other || ((object)other != null && EqualityContract == other!.EqualityContract && EqualityComparer<string>.Default.Equals(Name, other!.Name) && EqualityComparer<int>.Default.Equals(Age, other!.Age));
        }

        private Type EqualityContract
        {
            get
            {
                return typeof(Elephant);
            }
        }

        private bool PrintMembers(StringBuilder builder)
        {
            RuntimeHelpers.EnsureSufficientExecutionStack();
            builder.Append("Name = ");
            builder.Append((object?)Name);
            builder.Append(", Age = ");
            builder.Append(Age.ToString());
            return true;
        }

    }
    internal sealed class Cat : ValueObject
    {
        public Cat(string Name, int Age)
        {
            this.Name = Name;
            this.Age = Age;
        }

        public string Name { get; set; }
        public int Age { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return Age;
        }
    }
    internal sealed class Rat : ValueObject
    {
        public Rat(string Name, int Age)
        {
            this.Name = Name;
            this.Age = Age;
        }

        public string Name { get; set; }
        public int Age { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return Age;
        }
    }
    public static void AssertEquals()
    {
        Dog dog1 = new Dog("Dog", 1);
        Dog dog2 = new Dog("Dog", 1);
        Debug.Assert(dog1 != dog2, "Reference types should not be value equal");

        Elephant elephant1 = new Elephant("Elephant", 1);
        Elephant elephant2 = new Elephant("Elephant", 1);
        Debug.Assert(elephant1 == elephant2, "Records should be value equal");

        Cat cat1 = new Cat("Cat", 1);
        Cat cat2 = new Cat("Cat", 1);
        Debug.Assert(cat1 == cat2, "ValueObjects should be value equal");

        //Record type Gotcha #1
        Cat cat = new Cat("Frank", 1);
        Rat rat = new Rat("Frank", 1);
        Debug.Assert(cat == rat, "All types derived from ValueObject should be value equal");

        Pig pig = new Pig("Frank", 1);
        Hamster hamster = new Hamster("Frank", 1);
        Debug.Assert(pig != hamster, "All record types derived from a record should be the same type and value equal");
    }
}

public record EmptyRecordBase
{
}
