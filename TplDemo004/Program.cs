using System.Collections.Concurrent;
using System.Diagnostics;

//scenario:
//multi-threaded producer, single consuming thread
//assumes that the consumer can dequeue and append a line to a file faster than producers can enqueue items

int totalLines = 320_000;
int threads = 4;
int totalLinesPerThread = totalLines / threads;
int parseTimeInMilliseconds = 0;

Producer.Run(threads, totalLinesPerThread, parseTimeInMilliseconds);
//await Producer2.Produce(threads, totalLinesPerThread, parseTimeInMilliseconds);

static class Producer
{
    private static BlockingCollection<string> queue = new BlockingCollection<string>();
    private static int _numThreads = 0;
    private static int _totalLinesPerThread = 0;
    private static int _parseTimeInMilliseconds = 0;

    public static void Run(int numThreads, int totalLinesPerThread, int parseTimeInMilliseconds)
    {
        _numThreads = numThreads;
        _totalLinesPerThread = totalLinesPerThread;
        _parseTimeInMilliseconds = parseTimeInMilliseconds;

        var sw = new Stopwatch();
        sw.Start();

        //with blocking collection, we can kick off the consumer first.
        var consumerThread1 = new Thread(Consume);
        consumerThread1.Start();

        Console.WriteLine($"Starting {numThreads} producers, each producing {_totalLinesPerThread} with a delay of {_parseTimeInMilliseconds}ms");

        var producerThreads = StartProducers(_numThreads);
        //var monitorThread = new Thread(() => TrackProgress(producerThreads));
        //monitorThread.Start();

        foreach (var t in producerThreads)
        {
            t.Join();
        }
        // this tells the blocking collections we're not producing anymore.
        queue.CompleteAdding();
        Console.WriteLine("Job's Done!");
        sw.Stop();
        Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds}");
        var fileInfo = new FileInfo(@"C:\temp\output.txt");
        Console.WriteLine($"File Size: {fileInfo.Length}");
    }

    private static void TrackProgress(List<Thread> producerThreads)
    {
        while(!queue.IsCompleted)
        {
            Thread.Sleep(1000);
            foreach(Thread thread in producerThreads)
            {
                Console.WriteLine($"ID: {thread.ManagedThreadId} State {thread.ThreadState}");
            }
        }
    }

    private static List<Thread> StartProducers(int numThreads)
    {
        List<Thread> threads = new List<Thread>();
        for (var i = 1; i <= numThreads; i++)
        {
            threads.Add(new(StartProducer));
        }

        foreach (var thread in threads)
        {
            thread.Start();
        }
        return threads;
    }

    static void StartProducer()
    {
        uint index = 0;
        while (index < _totalLinesPerThread)
        {
            index++;
            if (_parseTimeInMilliseconds > 0)
            {
                Thread.Sleep(_parseTimeInMilliseconds);
            }
            string data = $"Sample line: {index} from {Thread.CurrentThread.ManagedThreadId}";
            queue.Add(data);
        }
        
    }
    static void Consume()
    {
        using var writer = new StreamWriter(@"C:\temp\output.txt");
        // using blocking collection and GetConsumingEnumerable()
        // if the collection is drained it will 'wait' until it's 
        foreach (var item in queue.GetConsumingEnumerable())
        {
            writer.WriteLine($"{item} Queue Size: {queue.Count}");
        }
    }
}

public static class Producer2
{
    private static Queue<string> queue = new Queue<string>();
    private static int _numThreads = 0;
    private static int _totalLinesPerThread = 0;
    private static int _parseTimeInMilliseconds = 0;

    public static async Task Produce(int numThreads, int totalLinesPerThread, int parseTimeInMilliseconds)
    {
        _numThreads = numThreads;
        _totalLinesPerThread = totalLinesPerThread;
        _parseTimeInMilliseconds = parseTimeInMilliseconds;

        var sw = new Stopwatch();
        sw.Start();
        uint index = 0;
        while (index < _totalLinesPerThread)
        {
            index++;
            if (_parseTimeInMilliseconds > 0)
            {
                await Task.Delay(_parseTimeInMilliseconds);
            }
            string data = $"Sample line: {index} from {Thread.CurrentThread.ManagedThreadId}";
            queue.Enqueue(data);
        }
        await Consume(queue);
        sw.Stop();
        Console.WriteLine("Job's Done!");
        Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds}");
    }
    static async Task Consume(IEnumerable<string> lines)
    {
        await File.WriteAllLinesAsync(@"C:\temp\output.txt", lines);
    }
}