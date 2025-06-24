using System;
using UnityEngine;

public class PlayerCommandManager : MonoBehaviour
{
    [SerializeField] CommandManager commandManager;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerAttack playerAttack;
    [SerializeField] Transform playerTransform;

    void Start()
    {
        commandManager.BindKey(KeyCode.A, new MoveLeftCommand(playerMovement));
        commandManager.BindKey(KeyCode.D, new MoveRightCommand(playerMovement));
        commandManager.BindKey(KeyCode.W, new JumpCommand(playerMovement));
        commandManager.BindKey(KeyCode.Q, new MeleeAttackCommand(playerAttack));
        commandManager.BindKey(KeyCode.F, new RangedAttackCommand(playerAttack));
        commandManager.BindKey(KeyCode.E, new InteractCommand(playerTransform));
    }
}