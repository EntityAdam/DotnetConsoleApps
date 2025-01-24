using System.Collections.Concurrent;
using System.Diagnostics;

// define total lines to be produced
// we start to see the consumer struggle to dequeue bit at
//   20_000_000 (20 million) lines with 0ms delay at greater than 1 thread; produces 1GB File; 2GB Mem consumed;
// settings for demo purposes
//   16_000 lines with 1ms delay
//      4 threads, 60 seconds
//     16 threads, 15 seconds
//     64 threads, 3.5 seconds
//    128 threads, 1 second
int totalLines = 16_000;

// define total producer threads
// your limitations here are
// 1: cpu time: you don't want to peg 100%
// 2: hardware: you won't get any benefit on 1 vCPU
int threads = 128;

// define an artifical delay, aka 'this is how long my parser might take to parse something'.
// the lower the better, the queue and the consumer can keep up.
int parseTimeInMilliseconds = 5;

Demo demo = new Demo(threads, totalLines, parseTimeInMilliseconds);
demo.Run();

internal class Demo(int threads, int totalLines, int delayInMilliseconds)
{
    //blocking collection is thread-safe.
    private static BlockingCollection<string> queue = new BlockingCollection<string>();

    private readonly int threads = threads;
    private readonly int totalLines = totalLines;
    private readonly int delayInMilliseconds = delayInMilliseconds;
    private readonly int totalLinesPerThread = (totalLines / threads);

    public void Run()
    {
        // generic info
        Console.WriteLine($"Threads [{threads}] Lines Per Thread [{totalLinesPerThread}] Total Lines [{totalLines}] Artificial Parse Delay [{delayInMilliseconds}ms]");
        var sw = new Stopwatch();
        sw.Start();

        // create and start the single consumer thread
        // this thread is responsible for dequeuing and writing lines to a file.
        // we can start this now, because the blocking collection will block the thread
        // until it has something to dequeue, without fussing with locks.
        // see: BlockingCollection<T>.GetConsumingEnumerable()
        StartConsumer();

        // create and start the producer threads
        var producerThreads = StartProducers(threads);

        // join the threads so we know when they are done producing
        foreach (var t in producerThreads)
        {
            t.Join();
        }
        
        // now that the threads are done producing, we need to tell the blockingcollection
        // that we're done adding to it
        queue.CompleteAdding();

        // generic output of info
        sw.Stop();
        Console.WriteLine("Job's Done!");
        Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds}ms");
        var fileInfo = new FileInfo(@"C:\temp\output.txt");
        Console.WriteLine($"File Size: {fileInfo.Length}");
    }

    private static void StartConsumer()
    {
        var consumerThread1 = new Thread(Consume);
        consumerThread1.Start();
    }

    private List<Thread> StartProducers(int numThreads)
    {
        List<Thread> threads = new();
        for (var i = 1; i <= numThreads; i++)
        {
            Thread t = new(StartProducer);
            t.Start();
            threads.Add(t);
        }
        return threads;
    }

    void StartProducer()
    {
        uint index = 0;
        while (index < totalLinesPerThread)
        {
            index++;
            if (delayInMilliseconds > 0)
            {
                Thread.Sleep(delayInMilliseconds);
            }
            string data = $"Sample line: {index} from {Thread.CurrentThread.ManagedThreadId}";
            queue.Add(data);
        }

    }
    static void Consume()
    {
        using var writer = new StreamWriter(@"C:\temp\output.txt");
        foreach (var item in queue.GetConsumingEnumerable())
        {
            writer.WriteLine($"{item} Queue Size: {queue.Count}");
        }
    }
}