using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Provoked")]
public class Provoked : Ability {
    public const int PROVOKEDDAMAGE = 1;

    public override void TriggerAbility(Unit unit)
    {
        if (unit.health > 0)
        {
            unit.attack++;
        }
    }
}
