using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Reinforcement")]
public class Reinforcement : Ability
{
    public Sprite dogMinionSprite;
    public Sprite catMinionSprite;
    public Ability minionAbility;
    public const int MINIONHP = 2;
    public const int MINIONDMG = 2;
    public const int MINIONSPEED = 2;

    public override void TriggerAbility(Unit unit) {
        Unit minion = Instantiate(unit);
        if (unit.playertype == PlayerType.DOG) {
            minion.GetComponent<SpriteRenderer>().sprite = dogMinionSprite;
        }
        else {
            minion.GetComponent<SpriteRenderer>().sprite = catMinionSprite;
        }

        minion.ability = minionAbility;
        minion.initialHealth = MINIONHP;
        minion.attack = MINIONDMG;
        minion.movement = MINIONSPEED;

        minion.occupiedTile.Unit = minion;
        unit.player.AddUnit(minion);
        minion.movementLeft = 0;

        minion.health  = MINIONHP;
    }
}
