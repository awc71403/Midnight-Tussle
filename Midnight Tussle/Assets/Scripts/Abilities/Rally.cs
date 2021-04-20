using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Rally")]
public class Rally : Ability {

    public const int RALLYAMOUNT = 2;

    public override void TriggerAbility(Unit unit) {
        List<Unit> units;
        if (unit.playertype == PlayerType.DOG) {
            units =  new List<Unit>(TussleManager.instance.dogPlayer.GetUnits);


        }
        else {
            units = new List<Unit>(TussleManager.instance.catPlayer.GetUnits);
        }

        units.Remove(unit);
        if (units.Count != 0)
        {
            units[Random.Range(0, units.Count)].IncreaseHP(RALLYAMOUNT);
        }
    }
}
