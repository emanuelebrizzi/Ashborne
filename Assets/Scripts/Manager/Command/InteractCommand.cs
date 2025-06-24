using UnityEngine;
public class InteractCommand : ICommand
{
    public CommandType Type => CommandType.Instant;
    private IInteractable interactable;

    public InteractCommand(IInteractable target)
    {
        interactable = target;
    }

    public void Execute()
    {
        if (interactable.IsInteractable())
        {
            interactable.Interact();
        }
    }
}
