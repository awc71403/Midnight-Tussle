using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Gacha : MonoBehaviour, IPointerClickHandler
{
    #region Variables
    public int normalRate;
    public int rareRate;
    public int epicRate;
    public int legendaryRate;

    private UnitDatabaseSO unitDatabase;
    #endregion

    private void Start() {
        unitDatabase = GameManager.singleton.database;
    }

    #region Gacha
    private void GachaRoll() {
        int random = Random.Range(1, 100);
        if (random <= normalRate) {
            if (GameManager.currentPlayer == GameManager.PLAYER1) {
                //Change to dogs
                if (unitDatabase.catNormalUnits.Count > 0) {
                    Hand.singleton.AddToHand(unitDatabase.catNormalUnits[Random.Range(0, unitDatabase.catNormalUnits.Count)]);
                }
            }
            else {
                if (unitDatabase.catNormalUnits.Count > 0) {
                    Hand.singleton.AddToHand(unitDatabase.catNormalUnits[Random.Range(0, unitDatabase.catNormalUnits.Count)]);
                }
            }
        }
        else if (random <= normalRate + rareRate) {
            if (GameManager.currentPlayer == GameManager.PLAYER1) {

            }
            else {

            }
        }
        else if (random <= normalRate + rareRate + epicRate) {
            if (GameManager.currentPlayer == GameManager.PLAYER1) {

            }
            else {

            }
        }
        else {
            if (GameManager.currentPlayer == GameManager.PLAYER1) {

            }
            else {

            }
        }
    }
    #endregion

    #region Click
    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            GachaRoll();
        }
    }
    #endregion
}
