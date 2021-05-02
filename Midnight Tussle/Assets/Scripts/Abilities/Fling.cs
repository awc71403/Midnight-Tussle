using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Fling")]
public class Fling : Ability {
    public const int FLINGAMOUNT = 3;

    public override void TriggerAbility(Unit unit)
    {
        if (unit.playertype == PlayerType.DOG)
        {
            Tile rightTile = unit.occupiedTile.directionMap[Direction.RIGHT];
            while (rightTile != null) {
                Unit rightUnit = rightTile.Unit;
                if (rightUnit != null && rightUnit.playertype == PlayerType.CAT)
                {
                    rightUnit.TakeDamage(FLINGAMOUNT, unit);
                    break;
                }
                else {
                    rightTile = rightTile.directionMap[Direction.RIGHT];
                }
            }
        }
        else
        {
            Tile leftTile = unit.occupiedTile.directionMap[Direction.LEFT];
            while (leftTile != null)
            {
                Unit rightUnit = leftTile.Unit;
                if (rightUnit != null && rightUnit.playertype == PlayerType.DOG)
                {
                    rightUnit.TakeDamage(FLINGAMOUNT, unit);
                    break;
                }
                else
                {
                    leftTile = leftTile.directionMap[Direction.RIGHT];
                }
            }
        }
    }
}
