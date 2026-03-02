static class ExamUtils

{

    public static async Task<string> Method1Async()

    {

        await Task.Delay(100);

        Console.WriteLine("Method1Async started");

 

        await Task.Delay(100);

        Console.WriteLine("Method1Async completed");

 

        string department = "Informatics";

 

        return department;

    }

 

    public static async Task<string[]> Method2Async()

    {

        await Task.Delay(1200);

        Console.WriteLine("Method2Async started");

 

        await Task.Delay(1200);

        Console.WriteLine("Method2Async completed");

 

        string[] employees = new[]

        {

            "Mary Sue",

            "Gary Stu"

        };

        return employees;

    }

 

    public static async Task Method3Async()

    {

        await Task.Delay(2500);

        Console.WriteLine("Method3Async started");

 

        await Task.Delay(2500);

        throw new InvalidOperationException("Error in Method3Async");

    }

 

    /// <summary>

    /// Ensures that all remaining tasks complete before proceeding.

    /// This is useful after <see cref="Task.WhenAny"/> to prevent unfinished tasks

    /// from interfering with later execution (and thus making the exam confusing).

    /// For example, this prevents tasks started under A() from finishing after B() has started.

    /// </summary>

    /// <param name="tasks">The tasks to wait for.</param>

    /// <returns>A task that completes when all remaining tasks are done.</returns>

    public static async Task AwaitRemainingTasksAsync(params Task[] tasks)

    {

        var remainingTasks = tasks.Where(t => !t.IsCompleted).ToArray();

 

        if (remainingTasks.Length > 0)

        {

            try

            {

                await Task.WhenAll(remainingTasks);

            }

            catch

            {

                // Ignore failures from remaining tasks

            }

        }

    }

}

 

class Program

{

    static async Task A()

    {

        var task1 = ExamUtils.Method1Async();

        var task2 = ExamUtils.Method2Async();

        var task3 = ExamUtils.Method3Async();

 

        try

        {

            await Task.WhenAll(task1, task2, task3);

        }

        catch (Exception)

        {

            Console.WriteLine($"A task1 faulted: {task1.IsFaulted}");

            Console.WriteLine($"A task2 faulted: {task2.IsFaulted}");

            Console.WriteLine($"A task3 faulted: {task3.IsFaulted}");

 

            // True only when the task completed without faulting or being canceled.

            if (task1.IsCompletedSuccessfully)

            {

                Console.WriteLine($"A text: {task1.Result}");

            }

 

            if (task2.IsCompletedSuccessfully)

            {

                foreach (string name in task2.Result)

                {

                    Console.WriteLine($"A name: {name}");

                }

            }

 

            string message = task3.Exception?.InnerException?.Message ?? "No message";

            Console.WriteLine($"A exception message: {message}");

        }

    }

 

    static async Task B()

    {

        var task1 = ExamUtils.Method1Async();

        var task2 = ExamUtils.Method2Async();

        var task3 = ExamUtils.Method3Async();

 

        List<Task> tasks = new() { task1, task2, task3 };

 

        while (tasks.Count > 0)

        {

            Task completed = await Task.WhenAny(tasks);

 

            if (completed == task1)

                Console.WriteLine("B finished: Method1Async");

            else if (completed == task2)

                Console.WriteLine("B finished: Method2Async");

            else if (completed == task3)

                Console.WriteLine("B finished: Method3Async");

 

            try

            {

                if (completed == task1)

                {

                    string text = await task1;

                    Console.WriteLine($"B text: {text}");

                }

                else if (completed == task2)

                {

                    string[] names = await task2;

                    foreach (string name in names)

                    {

                        Console.WriteLine($"B name: {name}");

                    }

                }

                else

                {

                    await task3;

                }

            }

            catch (Exception ex)

            {

                Console.WriteLine($"B caught: {ex.Message}");

            }

 

            tasks.Remove(completed);

        }

    }

 

    static async Task C()

    {

        var task1 = ExamUtils.Method1Async();

        var task2 = ExamUtils.Method2Async();

        var task3 = ExamUtils.Method3Async();

 

        try

        {

            string[] names = await task2;

            foreach (string name in names)

            {

                Console.WriteLine($"C name: {name}");

            }

 

            string text = await task1;

            Console.WriteLine($"C text: {text}");

 

            await task3;

            Console.WriteLine($"C task3 completed: {task3.IsCompleted}");

        }

        catch (Exception ex)

        {

            Console.WriteLine($"C caught: {ex.Message}");

        }

    }

 

    static async Task Main()

    {

        Console.WriteLine("=== Start ===");

 

        Console.WriteLine("A ---");

        await A();

 

        Console.WriteLine("B ---");

        await B();

 

        Console.WriteLine("C ---");

        await C();

 

        Console.WriteLine("=== End ===");

    }

}