using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerClickHandler {
    #region Instance Variables

    public Unit myUnit;
    public int xPosition;
    public int yPosition;
    public string tileType;

    public Tile Left;
    public Tile Right;
    public Tile Up;
    public Tile Down;

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
        myUnit.transform.position = transform.position - new Vector3(0, 0, 0);
        myUnit.RecalculateDepth();
        myUnit.OccupiedTile = this;
    }

    public void ClearUnit() {
        myUnit = null;
    }
    #endregion

    #region Variable Functions
    public int GetXPosition() {
        return xPosition;
    }

    public int GetYPosition() {
        return yPosition;
    }
    #endregion

    #region Click
    public void OnPointerClick(PointerEventData eventData) {
        Debug.Log("Tile Click");
        if (myUnit == null && UnitUI.chosenUnitUI != null) {
            Debug.Log("Conditions met");
            GameManager.singleton.PlaceCharacterOnTile(UnitUI.chosenUnitUI.unitData, xPosition, yPosition, GameManager.currentPlayer);
        }
    }
    #endregion

}
