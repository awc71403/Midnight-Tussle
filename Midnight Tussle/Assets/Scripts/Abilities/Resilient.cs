using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Resilient")]
public class Resilient : Ability {
    public const int RESILIENTHEAL = 1;

    public override void TriggerAbility(Unit unit) {
        if (unit.health > 0) {
            unit.IncreaseHP(RESILIENTHEAL);
        }
    }
}
