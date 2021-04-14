using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum Direction {
    LEFT,
    RIGHT,
    UP,
    DOWN
}

public class Tile : MonoBehaviour {
    #region Instance Variables

    // THESE ARE ZERO-INDEXED, with 0,0 being the bottom left
    [HideInInspector] public int xIndex;
    [HideInInspector] public int yIndex;

    public Vector3 unitOffset;

    public Dictionary<Direction, Tile> directionMap = new Dictionary<Direction, Tile>();

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
