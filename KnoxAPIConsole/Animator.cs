namespace KnoxAPIConsole;

public class Animator {
    private static char[] animationChars = { '/', '-', '\\', '|' };

    public static async Task PlayUntilComplete(string message, Task untilTask) {
        int spinnerIndex = 0;
        Console.Write($"{message}... ");
        int spinnerStartLeft = Console.CursorLeft;

        Task spinnerTask = Task.Run(async () => {
            while (!untilTask.IsCompleted) {
                Console.Write(animationChars[spinnerIndex]);
                await Task.Delay(75);

                if (untilTask.IsCompleted) break;

                Console.Write('\b');
                spinnerIndex = (spinnerIndex + 1) % animationChars.Length;
            }
        });

        await Task.WhenAll(untilTask, spinnerTask);

        Console.SetCursorPosition(spinnerStartLeft, 0);
        Console.Write("Done");
        Console.WriteLine();
    }
}
