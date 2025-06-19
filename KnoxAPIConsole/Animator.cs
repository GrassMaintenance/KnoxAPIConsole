namespace KnoxAPIConsole;

public class Animator {
    private static int counter = 0;
    private static char[] animationChars = { '/', '-', '\\', '|' };

    public static async Task Play(string message) {
        int charValue = counter % animationChars.Length;
        Console.SetCursorPosition(0, 0);
        Console.Write($"{message}...{animationChars[charValue]}");
        counter++;
        counter %= animationChars.Length;

        await Task.Delay(75);
    }
}
