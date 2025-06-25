namespace KnoxAPIConsole.Menu;

public class MenuOption {
    public string Label { get; }
    public IKnoxCommand Command { get; }

    public MenuOption(string label, IKnoxCommand command) {
        Label = label;
        Command = command;
    }
}
