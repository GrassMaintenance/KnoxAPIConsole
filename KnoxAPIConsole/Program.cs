using KnoxAPIConsole.Client;
using KnoxAPIConsole.Menu;

namespace KnoxAPIConsole;

public class Program {
    public static async Task Main(string[] args) {
        ConfigureConsole();

        try {
            await RunWithAnimation("Setting up HttpClient", ClientManager.SetupClient());
        } catch (Exception ex) {
            Console.WriteLine($"[ERROR] Failed to initialize HttpClient {ex.Message}");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            return;
        }

        string tabletNumber = PromptTabletNumber();
        OptionMenu optionMenu = new(tabletNumber);
        await optionMenu.SetupMenu();
    }

    private static void ConfigureConsole() {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
        Console.CursorVisible = false;
    }

    private static async Task RunWithAnimation(string label, Task task) {
        await Animator.PlayUntilComplete(label, task);
    }

    private static string PromptTabletNumber() {
        Console.Clear();
        Console.CursorVisible = true;
        Console.Write("Enter tablet number without the \"11-\" prefix: ");
        string? input = Console.ReadLine();

        if (string.IsNullOrEmpty(input)) {
            Console.WriteLine("Tablet number cannot be empty. Press any key to exit...");
            Console.ReadKey();
            Environment.Exit(1);
        }

        return "11-" + input.Trim();
    }
}