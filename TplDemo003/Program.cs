// threads
Action action = new Action(() => Console.WriteLine($"Hello World! From {Thread.CurrentThread.ManagedThreadId}"));

Thread thread1 = new Thread(new ThreadStart(action));
Thread thread2 = new Thread(new ThreadStart(action));
thread1.Start();
//thread1.Join();
thread2.Start();
//thread2.Join();
Console.WriteLine("Jobs Done!");