using KnoxAPIConsole.Client;

namespace KnoxAPIConsole;

public class Program {
    public static async Task Main(string[] args) {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
        Console.CursorVisible = false;

        // Play animation while trying to receive access token
        Task clientTask = ClientManager.SetupClient();
        while (!clientTask.IsCompleted) {
            await Animator.Play("Setting up HttpClient");
        }
        await clientTask;

        //Get tablet number without the "11-" prefix
        Console.Clear();
        Console.CursorVisible = true;
        
        Console.Write("Enter tablet number without the \"11-\" prefix: ");
        string tabletNumber = "11-" + Console.ReadLine();

        //Launch options menu for tablet
        OptionMenu optionMenu = new(tabletNumber);
        await optionMenu.SetupMenu();
    }
}