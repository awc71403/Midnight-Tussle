using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Shroud")]
public class Shroud : Ability {
    private const int SHROUDAMOUNT = 1;

    public override void TriggerAbility(Unit unit)
    {
        foreach (Unit dogUnit in TussleManager.instance.dogPlayer.GetUnits)
        {
            dogUnit.attack -= SHROUDAMOUNT;
            dogUnit.TakeDamage(SHROUDAMOUNT, unit);
        }
    }
}
