using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    #region Initialization
    public static Hand singleton;

    private List<UnitUI> unitUIs;

    public GameObject templatePrefab;
    #endregion

    #region Initialization
    public void Awake() {
        singleton = this;

        unitUIs = new List<UnitUI>();
    }
    #endregion

    #region Hand
    public void AddToHand(UnitDatabaseSO.UnitData unitData) {
        UnitUI unitUI = Instantiate(templatePrefab).GetComponent<UnitUI>();
        unitUI.Setup(unitData);
        unitUIs.Add(unitUI);
        unitUI.transform.SetParent(transform);
    }

    public void ClearHand() {
        foreach (UnitUI unit in unitUIs) {
            if (unit != null) {
                Destroy(unit.gameObject);
            }
        }
        unitUIs.Clear();
    }
    #endregion
}
