public interface IKnoxCommand {
   Task<object?> ExecuteAsync();
   bool UseAnimation { get; }
}