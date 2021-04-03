using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Reinforcement")]
public class Reinforcement : Ability
{
    public Ability minionAbility;
    public const int MINIONHP = 2;
    public const int MINIONDMG = 2;
    public const int MINIONSPEED = 2;

    public override void TriggerAbility(Unit unit) {
        Unit minion = Instantiate(unit);
        minion.ability = minionAbility;
        minion.maxHealth = MINIONHP;
        minion.attack = MINIONDMG;
        minion.movement = MINIONSPEED;

        minion.occupiedTile.Unit = minion;
        unit.player.AddUnit(minion);
        minion.UpdateMovementLeft(0);

        minion.Updatehealth(MINIONHP);
    }
}
