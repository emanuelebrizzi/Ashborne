using UnityEngine;
public class JumpCommand : ICommand
{
    public CommandType Type => CommandType.Instant;
    PlayerMovement movement;
    public JumpCommand(PlayerMovement movement)
    {
        this.movement = movement;
    }

    public void Execute()
    {
        movement.Jump();
    }
}