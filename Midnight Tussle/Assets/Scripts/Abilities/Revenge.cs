using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Revenge")]
public class Revenge : Ability {

    public Sprite dogMinionSprite;
    public Sprite catMinionSprite;
    public Ability minionAbility;

    public override void TriggerAbility(Unit unit) {
        if (unit.killedBy.health > 0) {
            unit.killedBy.health = 1;
            unit.killedBy.attack = 1;
            unit.movement = 1;
            unit.ability = minionAbility;

            if (unit.killedBy.playertype == PlayerType.DOG) {
                unit.killedBy.GetComponent<SpriteRenderer>().sprite = dogMinionSprite;
            } else {
                unit.killedBy.GetComponent<SpriteRenderer>().sprite = catMinionSprite;
            }
        }
    }
}
