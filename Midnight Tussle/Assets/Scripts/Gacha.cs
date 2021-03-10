using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Gacha : MonoBehaviour, IPointerClickHandler
{
    #region Variables
    public int normalRate;
    public int rareRate;
    public int epicRate;
    public int legendaryRate;

    private int gachaRemaining;

    private UnitDatabaseSO unitDatabase;

    private const int gachaAmount = 5;
    #endregion

    private void Start() {
        unitDatabase = GameManager.singleton.database;
        gachaRemaining = gachaAmount;
    }

    #region Gacha
    private void GachaRoll() {
        gachaRemaining--;

        int random = Random.Range(1, 100);
        if (random <= normalRate) {
            if (GameManager.currentPlayer == GameManager.PLAYER1) {
                if (unitDatabase.dogNormalUnits.Count > 0) {
                    Hand.singleton.AddToHand(unitDatabase.dogNormalUnits[Random.Range(0, unitDatabase.dogNormalUnits.Count)]);
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
                if (unitDatabase.dogRareUnits.Count > 0) {
                    Hand.singleton.AddToHand(unitDatabase.dogRareUnits[Random.Range(0, unitDatabase.dogRareUnits.Count)]);
                }
            }
            else {
                if (unitDatabase.catRareUnits.Count > 0) {
                    Hand.singleton.AddToHand(unitDatabase.catRareUnits[Random.Range(0, unitDatabase.catRareUnits.Count)]);
                }
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
                if (unitDatabase.dogLegendaryUnits.Count > 0) {
                    Hand.singleton.AddToHand(unitDatabase.dogLegendaryUnits[Random.Range(0, unitDatabase.dogLegendaryUnits.Count)]);
                }
            }
            else {
                if (unitDatabase.catLegendaryUnits.Count > 0) {
                    Hand.singleton.AddToHand(unitDatabase.catLegendaryUnits[Random.Range(0, unitDatabase.catLegendaryUnits.Count)]);
                }
            }
        }

        if (gachaRemaining <= 0) {
            GetComponent<Image>().color = new Color(.5f, .5f, .5f);
        }
    }

    public void GachaReset() {
        gachaRemaining = gachaAmount;
        GetComponent<Image>().color = new Color(1f, 1f, 1f);

    }
    #endregion

    #region Click
    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            if (gachaRemaining > 0) {
                GachaRoll();
            }
        }
    }
    #endregion
}
