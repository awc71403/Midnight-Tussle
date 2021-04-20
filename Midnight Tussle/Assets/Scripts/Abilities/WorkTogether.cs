using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/WorkTogether")]
public class WorkTogether : Ability
{
    public override void TriggerAbility(Unit unit) {
        if (unit.playertype == PlayerType.DOG) {
            Tile rightTile = unit.occupiedTile.directionMap[Direction.RIGHT];
            if (rightTile != null) {
                Unit rightUnit = rightTile.Unit;
                if (rightUnit != null && rightUnit.playertype == PlayerType.DOG) {
                    rightUnit.health += unit.health;
                    rightUnit.attack += unit.attack;
                }
            }
        }
        else {
            Tile leftTile = unit.occupiedTile.directionMap[Direction.LEFT];
            if (leftTile != null) {
                Unit leftUnit = leftTile.Unit;
                if (leftUnit != null && leftUnit.playertype == PlayerType.CAT) {
                    leftUnit.health += unit.health;
                    leftUnit.attack += unit.attack;
                }
            }
        }

        unit.occupiedTile.ClearUnit();
        unit.player.RemoveUnit(unit);
        Destroy(unit.gameObject);
    }
}
