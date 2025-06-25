using UnityEngine;
public class PauseCommand : ICommand
{
    public CommandType Type => CommandType.Instant;
    GameManager gameManager;
    public PauseCommand(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void Execute()
    {
        gameManager.TogglePauseMenu();
    }
}