using System;
using UnityEngine;

public class PlayerCommandManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CommandManager commandManager;
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

    void Start()
    {
        commandManager.BindKey(moveLeftKey, new MoveLeftCommand(playerMovement));
        commandManager.BindKey(moveRightKey, new MoveRightCommand(playerMovement));
        commandManager.BindKey(jumpKey, new JumpCommand(playerMovement));
        commandManager.BindKey(meleeAttackKey, new MeleeAttackCommand(playerAttack));
        commandManager.BindKey(rangedAttackKey, new RangedAttackCommand(playerAttack));
        commandManager.BindKey(interactKey, new InteractCommand(playerTransform));
    }
}
