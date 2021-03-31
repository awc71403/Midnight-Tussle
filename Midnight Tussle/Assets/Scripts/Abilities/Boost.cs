using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Boost")]
public class Boost : Ability
{
    public const int BOOSTAMOUNT = 1;

    public override void TriggerAbility(Unit unit) {
        if (unit.species == Unit.Species.DOG) {
            Tile rightTile = unit.occupiedTile.directionMap[Direction.RIGHT];
            if (rightTile != null) {
                Unit rightUnit = rightTile.Unit;
                if (rightUnit != null) {
                    rightUnit.UpdateMovementLeft(rightUnit.movement + BOOSTAMOUNT);
                }
            }
        }
        else {
            Tile leftTile = unit.occupiedTile.directionMap[Direction.LEFT];
            if (leftTile != null) {
                Unit leftUnit = leftTile.Unit;
                if (leftUnit != null) {
                    leftUnit.UpdateMovementLeft(leftUnit.movement + BOOSTAMOUNT);
                }
            }
        }
    }

}
