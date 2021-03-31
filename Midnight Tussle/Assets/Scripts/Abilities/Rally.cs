using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Rally")]
public class Rally : Ability {

    public const int RALLYAMOUNT = 2;

    public override void TriggerAbility(Unit unit) {
        if (unit.species == Unit.Species.DOG) {
            List<Unit> units = TussleManager.instance.dogPlayer.GetUnits;
            units[Random.Range(0, units.Count)].IncreaseHP(RALLYAMOUNT);
        }
        else {
            List<Unit> units = TussleManager.instance.catPlayer.GetUnits;
            units[Random.Range(0, units.Count)].IncreaseHP(RALLYAMOUNT);
        }
    }
}
