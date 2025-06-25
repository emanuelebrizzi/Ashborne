using UnityEngine;
public class MeleeAttackCommand : ICommand
{
    public CommandType Type => CommandType.Instant;
    PlayerAttack attack;
    public MeleeAttackCommand(PlayerAttack attack)
    {
        this.attack = attack;
    }

    public void Execute()
    {
        attack.StartAttack();
    }
}