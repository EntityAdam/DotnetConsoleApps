using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Diagnostics;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Hydrating");

        List<Guid> customers = new();
        ConcurrentBag<Invoice> concurrentInvoices = new();

        //1k customers 100k invoices per, 
        //immutable: 52661573 ticks
        //concurrent 15836555 ticks

        //10 customers 100 invoices per, 
        //immutable: 41825 ticks
        //concurrent 4008 ticks
        for (var i = 0; i < 1_000; i++)
        {
            customers.Add(Guid.NewGuid());
        }
        foreach (var customer in customers)
        {
            for (var i = 0; i < 10_000; i++)
            {
                concurrentInvoices.Add(new(customer, 100m));
            }
        }

        ImmutableList<Invoice> immutableInvoices = concurrentInvoices.ToImmutableList();
        Console.WriteLine("Done hydrating");

        var customer4 = customers.Skip(4).Take(1).Single();
        var customer9 = customers.Skip(9).Take(1).Single();

        var sw1 = Stopwatch.StartNew();
        Console.WriteLine($"Customer '{customer4}' Paid '{Calculator.GetAmountPaid(immutableInvoices, customer4)}'");
        sw1.Stop();
        Console.WriteLine($"{sw1.ElapsedTicks} to read immutable collection");

        var sw2 = Stopwatch.StartNew();
        Console.WriteLine($"Customer '{customer9}' Paid '{Calculator.GetAmountPaid(concurrentInvoices, customer9)}'");
        sw2.Stop();
        Console.WriteLine($"{sw2.ElapsedTicks} to read concurrent collection");

        var sw3 = Stopwatch.StartNew();
        Calculator.AddInvoices(concurrentInvoices, customer9);
        sw3.Stop();
        Console.WriteLine($"Customer '{customer9}' Now Paid '{Calculator.GetAmountPaid(concurrentInvoices, customer9)}'");
        Console.WriteLine($"{sw3.ElapsedTicks} to write concurrent collection");

        var sw4 = Stopwatch.StartNew();
        var newInvoices = await Calculator.AddInvoices(immutableInvoices, customer4);
        sw4.Stop();
        Console.WriteLine($"Customer '{customer4}' Now Paid '{Calculator.GetAmountPaid(newInvoices, customer4)}'");
        Console.WriteLine($"{sw4.ElapsedTicks} to write immutable collection");

        await Calculator.AddInvoicesAsync(concurrentInvoices, customer9);
    }
}

public class Invoice
{
    public Invoice(Guid customerId, decimal amountPaid)
    {
        CustomerId = customerId;
        AmountPaid = amountPaid;
    }
    public Guid CustomerId { get; }
    public decimal AmountPaid { get; }
}

public static class Calculator
{
    public static decimal GetAmountPaid(ImmutableList<Invoice> invoices, Guid customerId)
    {
        return invoices.Where(c => c.CustomerId == customerId).Sum(c => c.AmountPaid);
    }

    public static decimal GetAmountPaid(ConcurrentBag<Invoice> invoices, Guid customerId)
    {
        return invoices.Where(c => c.CustomerId == customerId).Sum(c => c.AmountPaid);
    }

    //This was surprisingly easy to accomplish!
    //We're mutating a collection of invoices using parallel stuffs!
    public static void AddInvoices(ConcurrentBag<Invoice> invoices, Guid customerId)
    {
        List<Task> bagAddTasks = new List<Task>();
        for (int i = 0; i < 500; i++)
        {
            bagAddTasks.Add(Task.Run(() => invoices.Add(new(customerId, 100m))));
        }

        // Wait for all tasks to complete
        Task.WaitAll(bagAddTasks.ToArray());
    }

    public static async Task AddInvoicesAsync(ConcurrentBag<Invoice> invoices, Guid customerId)
    {
        List<Task> bagAddTasks = new List<Task>();
        for (int i = 0; i < 500; i++)
        {
            await AddInvoiceToBag(invoices, new(customerId, 100m));
            await ReadBag(invoices, customerId);
        }
    }

    public static async Task ReadBag(ConcurrentBag<Invoice> invoices, Guid customerId)
    {
        Console.WriteLine($"Bag Read: Invoice Num {invoices.Count} from {Thread.CurrentThread.ManagedThreadId}");
    }

    public static async Task AddInvoiceToBag(ConcurrentBag<Invoice> invoices, Invoice invoice)
    {
        invoices.Add(invoice);
        Console.WriteLine($"Bag Add: Invoice Num {invoices.Count} from {Thread.CurrentThread.ManagedThreadId}");
        //await Task.Delay(100);
    }

    public static async Task<ImmutableList<Invoice>> AddInvoices(ImmutableList<Invoice> invoices, Guid customerId)
    {
        List<Invoice> newInvoices = new List<Invoice>();
        for (int i = 0; i < 500; i++)
        {
            invoices.Add(new(customerId, 100m));
        }
        return invoices.AddRange(newInvoices);
    }

    public static async Task<ImmutableList<Invoice>> AddInvoice(ImmutableList<Invoice> invoices, Invoice invoice)
    {
        return invoices.Add(invoice);
    }
}