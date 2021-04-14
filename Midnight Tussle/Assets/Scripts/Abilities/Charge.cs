using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Charge")]
public class Charge : Ability
{
    public override void TriggerAbility(Unit unit) {
        if (unit.playertype == PlayerType.DOG) {
            unit.player.CallMovement(Direction.RIGHT);
            List<Unit> units = TussleManager.instance.catPlayer.GetUnits;
            foreach (Unit dog in units) {
                dog.movementLeft++;
            }
        }
        else {
            unit.player.CallMovement(Direction.LEFT);
            List<Unit> units = TussleManager.instance.catPlayer.GetUnits;
            foreach (Unit cat in units) {
                cat.movementLeft++;
            }
        }
    }
}
