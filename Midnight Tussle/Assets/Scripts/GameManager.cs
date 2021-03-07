using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Variables
    public static GameManager singleton;

    public UnitDatabaseSO database;

    public static int currentPlayer;
    public static bool actionInProcess;
    public static int turn;

    private Tile[] tiles;

    private List<Unit> player1Units;
    private List<Unit> player2Units;

    [SerializeField]
    Unit testUnit;

    [SerializeField]
    private Button endButton;

    private Tile[,] mapArray;

    private const int m_xSize = 5;
    private const int m_ySize = 4;

    public const int PLAYER1 = 1;
    public const int PLAYER2 = 2;
    #endregion

    #region Initialization
    public void Awake() {
        // Singleton makes sure there is only one of this object
        if (singleton != null) {
            DestroyImmediate(gameObject);
            return;
        }
        singleton = this;

        player1Units = new List<Unit>();
        player2Units = new List<Unit>();

        currentPlayer = PLAYER1;
        turn = 1;

        SetTiles();
    }

    public void Update() {
    }
    #endregion

    #region Set Up
    public void SetTiles() {

        tiles = GetComponentsInChildren<Tile>();
        // Fill mapArray, which should be empty at first.
        mapArray = new Tile[m_xSize, m_ySize];

        // Nested for loop that creates mapYSize * mapXSize tiles.
        for (int y = 0; y < m_ySize; y++) {
            for (int x = 0; x < m_xSize; x++) {
                mapArray[x, y] = tiles[y * m_xSize + x];
                mapArray[x, y].xPosition = x;
                mapArray[x, y].yPosition = y;
            }
        }

        for (int y = 0; y < m_ySize; y++) {
            for (int x = 0; x < m_xSize; x++) {
                if (x - 1 >= 0) {
                    mapArray[x, y].Left = mapArray[x - 1, y];
                }
                if (x + 1 < m_xSize) {
                    mapArray[x, y].Right = mapArray[x + 1, y];
                }
                if (y + 1 < m_ySize) {
                    mapArray[x, y].Down = mapArray[x, y + 1];
                }
                if (y - 1 >= 0) {
                    mapArray[x, y].Up = mapArray[x, y - 1];
                }
            }
        }
    }

    public void PlaceCharacterOnTile(UnitDatabaseSO.UnitData data, int x, int y, int player) {
        // Instantiate an instance of the unit and place it on the given tile.
        Unit newUnit = Instantiate(data.unitPrefab).GetComponent<Unit>();
        newUnit.Setup(data, player);
        newUnit.OccupiedTile = mapArray[x, y];
        mapArray[x, y].transform.GetComponent<Tile>().PlaceUnit(newUnit);
        //Set Player
        if (player == PLAYER1) {
            player1Units.Add(newUnit);
        } else {
            player2Units.Add(newUnit);
        }
    }
    #endregion

    #region UI
    public void ShowCharacterUI(Unit selectedUnit) {
    }

    public void ClearUI() {

    }
    #endregion

    #region Turn
    private void NewTurn() {
        turn++;
        Mobalize();
    }

    public void EndTurn() {
        if (currentPlayer == PLAYER1) {
            currentPlayer = PLAYER2;
        }
        else {
            currentPlayer = PLAYER1;
        }
        Hand.singleton.ClearHand();
        NewTurn();
    }

    private void Mobalize() {
        List<Unit> units;
        if (currentPlayer == PLAYER1) {
            units = player1Units;
        }
        else {
            units = player2Units;
        }
        foreach (Unit unit in units) {
            unit.Movement();
        }
    }
    #endregion
}
