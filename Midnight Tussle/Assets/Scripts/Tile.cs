using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum Direction {
    LEFT,
    RIGHT,
    UP,
    DOWN
}

public class Tile : NetworkBehaviour {
    #region Instance Variables

    // THESE ARE ZERO-INDEXED, with 0,0 being the bottom left
    [HideInInspector]
    public int xIndex;
    [HideInInspector]
    public int yIndex;

    public static Tile hoveredTile;

    public Dictionary<Direction, Tile> directionMap = new Dictionary<Direction, Tile>();

    [SyncVar]
    private Unit myUnit;

    #endregion

    #region Getter
    public Unit Unit {
        get { return myUnit; }
        set { myUnit = value; }
    }
    #endregion

    #region Unit Functions

    // Detaches the unit from its current tile and puts it on this
    public void PlaceUnit(Unit unit) {
        myUnit = unit;

        if(myUnit.occupiedTile && myUnit.occupiedTile != this) myUnit.occupiedTile.ClearUnit();
        myUnit.occupiedTile = this;
        myUnit.RecalculateDepth();
    }

    public void ClearUnit() {
        myUnit = null;
    }

    public bool HasUnit(){
        return myUnit != null;
    }
    #endregion
}
