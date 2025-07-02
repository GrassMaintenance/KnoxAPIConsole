namespace KnoxAPIConsole;

public class SwitchTabletCommand : IKnoxCommand{
    string? input;
    
    public async Task<object?> ExecuteAsync() {
        await Task.Run(() => {
            Console.Clear();
            Console.CursorVisible = true;
            Console.Write("Enter tablet number without the \"11-\" prefix: ");
            input = Console.ReadLine();
        });

        return string.IsNullOrWhiteSpace(input) ? null : "11-" + input.Trim();
    }
}