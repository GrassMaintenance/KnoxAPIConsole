using KnoxAPIConsole.APICalls;

namespace KnoxAPIConsole;

public class OptionMenu {
    private string tabletNumber;
    Dictionary<string, IKnoxCommand> options;

    public OptionMenu(string tabletNumber) {
        this.tabletNumber = tabletNumber;
        options = new() {
            { "Get Device Version", new GetDeviceVersionCommand(tabletNumber) },
            { "Update Password", new UpdatePasswordCommand(tabletNumber) },
            //{ "Change Organization", ChangeOrganization },
            //{ "Push Profile", PushProfile },
            //{ "Clear App Data", ClearAppData },
            { "Update App", new UpdateAppCommand(tabletNumber) },
            //{ "Uninstall App", UninstallApp },
            { "Factory Reset", new FactoryResetCommand(tabletNumber) },
            //{ "Unenroll Device", UnenrollDevice },
            //{ "Quit", Quit }
        };
    }

    public async Task SetupMenu() {
        Console.Clear();

        // Add header to the top of the options menu
        int padding = (Console.WindowWidth - tabletNumber.Length) / 2;
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.ForegroundColor = ConsoleColor.White;

        Console.WriteLine(new string('*', Console.WindowWidth));
        string paddedText = tabletNumber.PadLeft(padding + tabletNumber.Length).PadRight(Console.WindowWidth);
        Console.WriteLine(paddedText);
        Console.WriteLine(new string('*', Console.WindowWidth));

        Console.ResetColor();
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
        Console.CursorVisible = true;

        // Add numbers in front of options
        Console.WriteLine();
        for (int i = 0; i < options.Count; i++) {
            if (i == options.Count - 1)
                Console.WriteLine("0. Quit");
            else
                Console.WriteLine($"{i + 1}. {options.Keys.ElementAt(i)}");
        }

        await GetUserInput();
    }

    private async Task GetUserInput() {
        while (true) {
            Console.Write("\nEnter a number: ");
            string? input = Console.ReadLine();
            int selection = 0;

            if (!int.TryParse(input, out selection)) {
                Console.Write("Invalid input! Please enter a number.");
                Console.Read();
                await SetupMenu();
            } else {
                if (selection == 0)
                    Environment.Exit(0);
                else {
                    IKnoxCommand selectedAction = options.ElementAt(selection - 1).Value;
                    Task task = selectedAction.ExecuteAsync();

                    Console.Clear();
                    while (!task.IsCompleted) {
                        await Animator.Play("Calling API endpoint");
                    }

                    await task;
                }
            }
        }
    }
}
