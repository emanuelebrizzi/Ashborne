using UnityEngine;

public class MeleeEnemy : Enemy
{
    public override void MoveInDirection(float direction)
    {
        base.MoveInDirection(direction);
        Debug.Log("Melee is moving");
    }
}
