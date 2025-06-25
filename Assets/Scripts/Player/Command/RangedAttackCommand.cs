using UnityEngine;
public class RangedAttackCommand : ICommand
{
    public CommandType Type => CommandType.Instant;
    PlayerAttack attack;
    public RangedAttackCommand(PlayerAttack attack)
    {
        this.attack = attack;
    }

    public void Execute()
    {
        attack.RangedAttack();
    }
}