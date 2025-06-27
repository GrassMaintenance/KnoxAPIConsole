using KnoxAPIConsole.APICalls;

namespace KnoxAPIConsole.Menu;

public class OptionMenu {
    private string tabletNumber;
    private readonly List<MenuOption> menuOptions;

    public OptionMenu(string tabletNumber) {
        this.tabletNumber = tabletNumber;
        menuOptions = new() {
            new("Get Device Version", new GetDeviceVersionCommand(tabletNumber)),
            new("Update Password", new UpdatePasswordCommand(tabletNumber)),
            new("Change Organization", new ChangeOrganizationCommand(tabletNumber)), // TODO: Implement profile pushing
            new("Push Profile", new PushProfileCommand(tabletNumber)),
            new("Clear App Data", new ClearAppDataCommand(tabletNumber)),
            //new("Install/Update App", new UpdateAppCommand(tabletNumber)), TODO: Implement app installation
            new("Uninstall App", new UninstallAppCommand(tabletNumber)),
            new("Factory Reset", new FactoryResetCommand(tabletNumber)),
            new("Unenroll Device", new UnenrollDeviceCommand(tabletNumber)),
            new("Switch Tablet", new SwitchTabletCommand())
        };
    }

    public async Task<string?> SetupMenu() {
        while (true) {
            Console.Clear();
            DrawHeader();

            for (int i = 0; i < menuOptions.Count; i++) {
                Console.WriteLine($"{i + 1}. {menuOptions[i].Label}");
            }

            int choice = GetMenuChoice(menuOptions.Count);
            Console.Clear();

            var selected = menuOptions[choice - 1].Command;
            object? result = await RunCommand(selected);

            if (selected is SwitchTabletCommand && result is string newTabletNumber) {
                return newTabletNumber;
            }
        }
    }

    private void DrawHeader() {
        int padding = (Console.WindowWidth - tabletNumber.Length) / 2;
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.ForegroundColor = ConsoleColor.White;

        Console.WriteLine(new string('*', Console.WindowWidth));
        Console.WriteLine(tabletNumber.PadLeft(padding + tabletNumber.Length).PadRight(Console.WindowWidth));
        Console.WriteLine(new string('*', Console.WindowWidth));

        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.White;
        Console.CursorVisible = true;
        Console.WriteLine();
    }

    private int GetMenuChoice(int max) {
        while (true) {
            Console.Write("\nEnter a number: ");

            if (int.TryParse(Console.ReadLine(), out int selection) && selection >= 1 && selection <= max) {
                return selection;
            }

            Console.WriteLine("Invalid input! Please enter a valid number.");
        }
    }

    private async Task<object?> RunCommand(IKnoxCommand command) {
        Console.Clear();

        await command.ExecuteAsync();

        Console.WriteLine("\n\nPress any key to return to the menu...");
        Console.ReadKey();
        return null;
    }
}
