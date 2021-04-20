using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Reinforcement")]
public class Reinforcement : Ability
{
    public GameObject dogMinionPrefab;
    public GameObject catMinionPrefab;

    public const int MINIONHP = 2;
    public const int MINIONDMG = 2;
    public const int MINIONSPEED = 1;

    public override void TriggerAbility(Unit unit) {
        Unit minion;
        if (unit.playertype == PlayerType.DOG) {
            minion = Instantiate(dogMinionPrefab).GetComponent<Unit>();
        }
        else {
            minion = Instantiate(catMinionPrefab).GetComponent<Unit>();
        }

        minion.gameObject.transform.SetParent(unit.gameObject.transform.parent);

        minion.initialHealth = MINIONHP;
        minion.attack = MINIONDMG;
        minion.movement = MINIONSPEED;

        minion.occupiedTile = unit.occupiedTile;
        unit.occupiedTile.Unit = minion;
        unit.player.AddUnit(minion);
        minion.movementLeft = 0;

        minion.health = MINIONHP;
        minion.RecalculateDepth();
    }
}
