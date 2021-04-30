using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Charge")]
public class Charge : Ability
{
    public override void TriggerAbility(Unit unit) {
        if (unit.playertype == PlayerType.DOG) {
            unit.player.ChargeAbility(Direction.RIGHT, unit);
        }
        else {
            unit.player.ChargeAbility(Direction.LEFT, unit);
        }
    }
}
