using UnityEngine;

public class MoveLeftCommand : ICommand
{
    public CommandType Type => CommandType.ContinuousMovement;
    PlayerMovement movement;
    public MoveLeftCommand(PlayerMovement movement)
    {
        this.movement = movement;
    }

    public void Execute()
    {
        movement.Move(-1);
    }
}

public class MoveRightCommand : ICommand
{
    public CommandType Type => CommandType.ContinuousMovement;
    PlayerMovement movement;
    public MoveRightCommand(PlayerMovement movement)
    {
        this.movement = movement;
    }

    public void Execute()
    {
        movement.Move(1);
    }
}
