using KnoxAPIConsole.APICalls;

namespace KnoxAPIConsole.Menu;

public class OptionMenu {
    private string _tabletNumber;
    private List<MenuOption> menuOptions;

    public OptionMenu(string tabletNumber) {
        _tabletNumber = tabletNumber;
        menuOptions = new List<MenuOption>();
        BuildMenuOptions();
    }

    private void BuildMenuOptions() {
        menuOptions.Clear();
        menuOptions = new() {
            new("Get Device Version", new GetDeviceVersionCommand(_tabletNumber)),
            new("Update Password", new UpdatePasswordCommand(_tabletNumber)),
            new("Change Organization", new ChangeOrganizationCommand(_tabletNumber)),
            new("Push Profile", new PushProfileCommand(_tabletNumber)),
            new("Power Off Device", new PowerOffCommand(_tabletNumber)),
            new("Clear App Data", new ClearAppDataCommand(_tabletNumber)),
            new("Uninstall App", new UninstallAppCommand(_tabletNumber)),
            new("Factory Reset", new FactoryResetCommand(_tabletNumber)),
            new("Unenroll Device", new UnenrollDeviceCommand(_tabletNumber)),
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
        int padding = (Console.WindowWidth - _tabletNumber.Length) / 2;
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.ForegroundColor = ConsoleColor.White;

        Console.WriteLine(new string('*', Console.WindowWidth));
        Console.WriteLine(_tabletNumber.PadLeft(padding + _tabletNumber.Length).PadRight(Console.WindowWidth));
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

        if (command is SwitchTabletCommand) {
            var newTablet = await command.ExecuteAsync() as string;
            if (!string.IsNullOrWhiteSpace(newTablet)) {
                _tabletNumber = newTablet;
                BuildMenuOptions();
            }
            await SetupMenu();
            return null;
        } else {
            await command.ExecuteAsync();
            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();
            return null;
        }        
    }
}
