using System.Collections.Concurrent;

Action action = new Action(() => Console.WriteLine("Hello Wold!"));
action.Invoke();
Func<int> add = new Func<int>(() => 40 + 2);
int result = add.Invoke();

// 1. Task Run
Task.Run(action).Wait();
Console.WriteLine("Task Done!");

// 2. What is Task.Run doing?
// Task.Run() is Task.StartNew() with default options
// This:
Task.Factory.StartNew(action, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default).Wait();
Console.WriteLine("Task Done!");

// 3. So, what is Task.Factory.StartNew() doing?!
// This:
Task task1 = new Task(action, CancellationToken.None, TaskCreationOptions.DenyChildAttach);
task1.Start(TaskScheduler.Default);
task1.Wait();
Console.WriteLine("Task 1 Done!");