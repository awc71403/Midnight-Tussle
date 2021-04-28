using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Pressure")]
public class Pressure : Ability
{
    public const int PRESSUREAMOUNT = 2;

    public override void TriggerAbility(Unit unit) {
        foreach (Direction direction in Enum.GetValues(typeof(Direction))) {
            Tile adjTile = unit.occupiedTile.directionMap[direction];
            if (adjTile != null) {
                Unit adjUnit = adjTile.Unit;
                if (adjUnit != null && unit.playertype != adjUnit.playertype) {
                    adjUnit.TakeDamageBase(PRESSUREAMOUNT, unit);
                }
            }
        }
    }
}
