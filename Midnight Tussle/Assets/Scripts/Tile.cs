using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerClickHandler {
    #region Instance Variables

    // THESE ARE ZERO-INDEXED, with 0,0 being the bottom left
    private int xIndex;
    private int yIndex;

    public Vector3 unitOffset;

    [HideInInspector] public Tile Left;
    [HideInInspector] public Tile Right;
    [HideInInspector] public Tile Up;
    [HideInInspector] public Tile Down;

    private Unit myUnit;

    #endregion

    #region Getter
    public Unit Unit {
        get { return myUnit; }
        set { myUnit = value; }
    }
    #endregion

    #region Unit Functions
    public void PlaceUnit(Unit unit) {
        myUnit = unit;
        myUnit.transform.position = transform.position + unitOffset;
        myUnit.RecalculateDepth();
        myUnit.OccupiedTile = this;
    }

    public void ClearUnit() {
        myUnit = null;
    }
    #endregion

    #region Variable Functions
    public int GetXPosition() {
        return xIndex;
    }

    public int GetYPosition() {
        return yIndex;
    }
    #endregion

    #region Click
    public void OnPointerClick(PointerEventData eventData) {
        Debug.Log("Tile Click");
        // if (myUnit == null && UnitUI.chosenUnitUI != null) {
        //     Debug.Log("Conditions met");
        //     GameManager.singleton.PlaceCharacterOnTile(UnitUI.chosenUnitUI.unitData, xPosition, yPosition, GameManager.currentPlayer);
        //     Destroy(UnitUI.chosenUnitUI.gameObject);
        //     UnitUI.chosenUnitUI = null;
        // }
    }

    #endregion

}
