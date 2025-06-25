namespace KnoxAPIConsole.Menu;

public class QuitCommand : IKnoxCommand {
    public Task ExecuteAsync() {
        Environment.Exit(0);
        return Task.CompletedTask;
    }
}