using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Fury")]
public class Fury : Ability {

    public override void TriggerAbility(Unit unit) {
        Direction movingDirection = unit.movingDirection;

        foreach (Direction direction in Enum.GetValues(typeof(Direction))) {
            if (direction != movingDirection) {
                Tile adjTile = unit.occupiedTile.directionMap[direction];
                if (adjTile != null) {
                    Unit adjUnit = adjTile.Unit;
                    if (adjUnit != null && unit.playertype != adjUnit.playertype) {
                        adjUnit.TakeDamage(unit.attack, false, null);
                    }
                }
            }
        }
    }
}
