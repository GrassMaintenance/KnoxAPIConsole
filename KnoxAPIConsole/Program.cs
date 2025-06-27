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

        string? tabletNumber = null;
        while (true) {
            if (string.IsNullOrEmpty(tabletNumber)) {
                Console.Clear();
                Console.CursorVisible = true;
                Console.Write("Enter tablet number without the \"11-\" prefix: ");
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)) {
                    Console.WriteLine("Tablet number cannot be empty. Press any key to exit...");
                    Console.ReadKey();
                    return;
                }

                tabletNumber = "11-" + input.Trim();
            }

            OptionMenu optionMenu = new(tabletNumber);
            string? newTabletNumber = await optionMenu.SetupMenu();

            if (!string.IsNullOrWhiteSpace(newTabletNumber)) {
                tabletNumber = newTabletNumber;
            }
        }

    }

    private static void ConfigureConsole() {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
        Console.CursorVisible = false;
    }

    private static async Task RunWithAnimation(string label, Task task) {
        await Animator.PlayUntilComplete(label, () => task);
    }
}