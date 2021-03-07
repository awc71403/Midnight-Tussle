using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitUI : MonoBehaviour, IPointerClickHandler {
    #region Variables
    public static UnitUI chosenUnitUI;

    [SerializeField]
    public UnitDatabaseSO.UnitData unitData;
    #endregion

    #region Initialization
    public void Setup(UnitDatabaseSO.UnitData data) {
        unitData = data;
        GetComponent<Image>().sprite = data.unitSprite;
        //Tooltip later
    }
    #endregion

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            if (chosenUnitUI != null) {
                chosenUnitUI.GetComponent<Image>().color = new Color(1, 1, 1);
            }
            Debug.Log("Chosen");
            GetComponent<Image>().color = new Color(.5f, .5f, .5f);
            chosenUnitUI = this;
        }
    }
}
