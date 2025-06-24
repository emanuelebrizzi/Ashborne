public enum CommandType
{
    Instant,
    ContinuousMovement
}

public interface ICommand
{
    void Execute();
    CommandType Type { get; }
}
