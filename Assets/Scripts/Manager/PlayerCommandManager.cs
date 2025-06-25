using System;
using UnityEngine;
using System.Collections.Generic;

public class PlayerCommandManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerAttack playerAttack;
    [SerializeField] Transform playerTransform;

    [Header("Key Bindings")]
    [SerializeField] KeyCode moveLeftKey = KeyCode.A;
    [SerializeField] KeyCode moveRightKey = KeyCode.D;
    [SerializeField] KeyCode jumpKey = KeyCode.W;
    [SerializeField] KeyCode meleeAttackKey = KeyCode.Q;
    [SerializeField] KeyCode rangedAttackKey = KeyCode.F;
    [SerializeField] KeyCode interactKey = KeyCode.E;
    [SerializeField] KeyCode pauseKey = KeyCode.Escape;
    Dictionary<KeyCode, ICommand> commandBindings = new Dictionary<KeyCode, ICommand>();

    void Start()
    {
        BindKey(moveLeftKey, new MoveLeftCommand(playerMovement));
        BindKey(moveRightKey, new MoveRightCommand(playerMovement));
        BindKey(jumpKey, new JumpCommand(playerMovement));
        BindKey(meleeAttackKey, new MeleeAttackCommand(playerAttack));
        BindKey(rangedAttackKey, new RangedAttackCommand(playerAttack));
        BindKey(interactKey, new InteractCommand(playerTransform));
        BindKey(pauseKey, new PauseCommand(GameManager.Instance));
    }
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
