using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Shove")]
public class Shove : Ability {
        
    public override void TriggerAbility(Unit unit)
    {
        if (unit.playertype == PlayerType.DOG)
        {
            Tile rightTile = unit.occupiedTile.directionMap[Direction.RIGHT];
            if (rightTile != null)
            {
                Unit rightUnit = rightTile.Unit;
                if (rightUnit != null && rightUnit.playertype == PlayerType.CAT)
                {
                    Tile rightrightTile = rightUnit.occupiedTile.directionMap[Direction.LEFT];
                    if (rightrightTile != null && rightrightTile.Unit == null)
                    {
                        rightUnit.MoveUnitInDirection(Direction.LEFT);
                    }
                    rightUnit.stun = true;
                }
            }
        }
        else
        {
            Tile leftTile = unit.occupiedTile.directionMap[Direction.LEFT];
            if (leftTile != null)
            {
                Unit leftUnit = leftTile.Unit;
                if (leftUnit != null && leftUnit.playertype == PlayerType.DOG)
                {
                    Tile leftleftTile = leftUnit.occupiedTile.directionMap[Direction.LEFT];
                    if (leftleftTile != null && leftleftTile.Unit == null) {
                        leftUnit.MoveUnitInDirection(Direction.LEFT);
                    }
                    leftUnit.stun = true;
                }
            }
        }
    }

}
