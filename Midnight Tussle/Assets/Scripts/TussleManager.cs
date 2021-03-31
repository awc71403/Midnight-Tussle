using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerType{
    DOG,
    CAT
}

[RequireComponent(typeof(Gacha))]
public class TussleManager : MonoBehaviour
{
    #region Static Variables
    public const int MAX_LEVEL = 4; // This is NOT zero-indexed!!!!
    public static List<Sprite> raritySprites;

    #endregion 

    #region Variables
    public static TussleManager instance;

    // References
    [Header("References")]
    [SerializeField] private Transform tileParent;
    [SerializeField] private UIManager uiManager;
    [SerializeField] public Player dogPlayer;
    [SerializeField] public Player catPlayer;

    [Header("Settings")]
    [SerializeField] private List<int> rolledAtLevel = new List<int>(MAX_LEVEL);
    [SerializeField] private List<int> placedAtLevel = new List<int>(MAX_LEVEL);
    [SerializeField] private List<Sprite> temp_raritySprites = new List<Sprite>(MAX_LEVEL + 1); // None, Common, Rare, Epic, Legendary

    private Gacha gachaMachine;

    private PlayerType currentTurn;
    private int turnCount = 0;

    private Tile[,] mapArray = new Tile[XSIZE, YSIZE];

    private const int XSIZE = 5;
    private const int YSIZE = 4;

    
    private Player currentPlayer {
        get { return currentTurn == PlayerType.DOG ? dogPlayer : catPlayer; }

    }

    #endregion

    #region Set Up

    void Awake(){
        if (instance != null) {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;

        raritySprites = temp_raritySprites;
    }

    void Start(){
        gachaMachine = GetComponent<Gacha>();
    }

    // Called by the GameManager, used to set the Tussle in motion  and run setup for the match
    public void StartTussle() {
        currentTurn = PlayerType.DOG;

        // THESE SHOULD BE STORED WITH 0,0 AT BOTTOM LEFT in the hierarchy!!! And running horizontally in sequence!

        // Fill mapArray, which should be empty at first.

        // Nested for loop that creates mapYSize * mapXSize tiles.
        for(int y = 0; y < YSIZE; y++){
            for(int x = 0; x < XSIZE; x++){
                mapArray[x, y] = tileParent.GetChild(y * XSIZE + x).GetComponent<Tile>();
                mapArray[x, y].xIndex = x;
                mapArray[x, y].yIndex = y;
            }
        }


        for (int y = 0; y < YSIZE; y++) {
            for (int x = 0; x < XSIZE; x++) {
                //Error-check
                if(mapArray[x, y] == null){
                    Debug.LogError("The tile indexes aren't set up right!");
                }

                if (x - 1 >= 0) mapArray[x, y].directionMap[Direction.LEFT] = mapArray[x - 1, y];
                else mapArray[x, y].directionMap[Direction.LEFT] = null;
                
                if (x + 1 < XSIZE) mapArray[x, y].directionMap[Direction.RIGHT] = mapArray[x + 1, y];
                else mapArray[x, y].directionMap[Direction.RIGHT] = null;
                
                if (y - 1 >= 0) mapArray[x, y].directionMap[Direction.DOWN] = mapArray[x, y - 1];
                else mapArray[x, y].directionMap[Direction.DOWN] = null;
                
                if (y + 1 < YSIZE) mapArray[x, y].directionMap[Direction.UP] = mapArray[x, y + 1];
                else mapArray[x, y].directionMap[Direction.UP] = null;
                
            }
        }

        NewTurn();
    }

    public void PlaceUnitOnTile(Unit recruitPrefab, Tile tile) {
        // Instantiate an instance of the unit and place it on the given tile.
        Unit instantiated = Instantiate<Unit>(recruitPrefab, tile.transform.position, Quaternion.identity, transform);
        tile.PlaceUnit(instantiated);
        bool continueRecruit = currentPlayer.AddUnit(instantiated);
        
        if(!continueRecruit){
            // Move on to movement phase
            currentPlayer.ActivateMovement();
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
    
    // Called to begin a turn
    private void NewTurn() {
        turnCount++;
        int newRecruits = rolledAtLevel[currentPlayer.GetLevel() - 1];
        int toRecruit = placedAtLevel[currentPlayer.GetLevel() - 1];
        uiManager.StartTurn(currentTurn, newRecruits);
        List<Unit> rolled = gachaMachine.Roll(currentTurn, newRecruits, currentPlayer.GetLevel());
        currentPlayer.StartRecruiting(rolled, toRecruit);
    }

    // Called to end a turn
    public void EndTurn() {
        if (currentTurn == PlayerType.DOG) {
            currentTurn = PlayerType.CAT;
        }
        else {
            currentTurn = PlayerType.DOG;

        }
        NewTurn();
    }

    #endregion


}
