using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject {

    public string aName = "New Ability";
    public string aDesc = "Ability Description";
    public enum ActivationType { NONE, SUMMON, DEATH, DAMAGE, ATTACK }
    public ActivationType type;

    public abstract void TriggerAbility(Unit unit);
}
