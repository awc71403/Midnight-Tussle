using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Revenge")]
public class Revenge : Ability {

    public GameObject dogMinionPrefab;
    public GameObject catMinionPrefab;

    public const int MINIONHP = 1;
    public const int MINIONDMG = 1;
    public const int MINIONSPEED = 1;

    public override void TriggerAbility(Unit unit) {
        if(unit.killedBy == null) return;
        
        Unit minion;
        if (unit.killedBy.playertype == PlayerType.DOG) {
            minion = Instantiate(dogMinionPrefab).GetComponent<Unit>();
            minion.playertype = PlayerType.DOG;
        }
        else {
            minion = Instantiate(catMinionPrefab).GetComponent<Unit>();
            minion.playertype = PlayerType.CAT;
        }

        Tile tile = unit.killedBy.occupiedTile;
        tile.ClearUnit();
        TussleManager.instance.PlaceMinion(minion, tile, unit.killedBy.player);
        unit.killedBy.Revenged();

        minion.initialHealth = MINIONHP;
        minion.attack = MINIONDMG;
        minion.movement = MINIONSPEED;

        minion.movementLeft = 0;

        minion.health = MINIONHP;
        minion.RecalculateDepth();
        
    }
}
