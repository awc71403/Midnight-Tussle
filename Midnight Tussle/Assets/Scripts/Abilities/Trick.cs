using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Trick")]
public class Trick : Ability {

    public override void TriggerAbility(Unit unit)
    {
        Direction movingDirection = unit.movingDirection;

        Tile adjTile = unit.occupiedTile.directionMap[movingDirection];
        if (adjTile != null)
        {
            Unit adjUnit = adjTile.Unit;
            if (adjUnit != null && unit.playertype != adjUnit.playertype)
            {
                adjUnit.occupiedTile = unit.occupiedTile;
                unit.occupiedTile.Unit = adjUnit;
                unit.occupiedTile = adjTile;
                adjTile.Unit = unit;

                adjUnit.RecalculateDepth();
                unit.RecalculateDepth();
            }
        }
    }
}
