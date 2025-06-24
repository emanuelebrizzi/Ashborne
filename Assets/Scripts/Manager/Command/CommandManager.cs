using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    Dictionary<KeyCode, ICommand> commandBindings = new Dictionary<KeyCode, ICommand>();


    void Update()
    {
        bool isMoving = false;

        foreach (var pair in commandBindings)
        {
            var command = pair.Value;

            if (command.Type == CommandType.ContinuousMovement)
            {
                if (Input.GetKey(pair.Key))
                {
                    command.Execute();
                    isMoving = true;
                }
            }
            else if (Input.GetKeyDown(pair.Key))
            {
                command.Execute();
            }
        }

        if (!isMoving)
            playerMovement.Stop();

    }
    

    public void BindKey(KeyCode key, ICommand command)
    {
        commandBindings[key] = command;
    }
}
