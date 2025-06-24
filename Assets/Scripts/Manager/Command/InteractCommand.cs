using UnityEngine;

public class InteractCommand : ICommand
{
    public CommandType Type => CommandType.Instant;
    Transform playerTransform;

    public InteractCommand(Transform playerTransform)
    {
        this.playerTransform = playerTransform;
    }

    public void Execute()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(playerTransform.position,1);

            foreach (var collider in colliders)
            {
                IInteractable interactable = collider.GetComponent<IInteractable>();
                if (interactable != null && interactable.IsInteractable())
                {
                    interactable.Interact();
                    break;
                }
            }
        
    }
}
