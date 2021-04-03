using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/None")]
public class None : Ability
{
    public override void TriggerAbility(Unit unit) {
        return;
    }
}
