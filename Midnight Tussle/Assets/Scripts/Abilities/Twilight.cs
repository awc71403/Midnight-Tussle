using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Twilight")]
public class Twilight : Ability {
    private const int TWILIGHTAMOUNT = 1;

    public override void TriggerAbility(Unit unit)
    {
        foreach (Unit dogUnit in TussleManager.instance.dogPlayer.GetUnits)
        {
            dogUnit.attack += TWILIGHTAMOUNT;
            dogUnit.health += TWILIGHTAMOUNT;
        }
    }
}
