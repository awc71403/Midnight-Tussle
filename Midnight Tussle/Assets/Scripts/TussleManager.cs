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

    public const int XSIZE = 6;
    public const int YSIZE = 5;
    
    public Player currentPlayer {
        get { return currentTurn == PlayerType.DOG ? dogPlayer : catPlayer; }

    }

    [SerializeField]
    private int furthestColumn;

    [HideInInspector] public bool gameOver = false;

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
        UpdateFurthestColumnCanSpawn();
        if(!continueRecruit || ColumnAvailable()){
            // Move on to movement phase
            currentPlayer.ActivateMovement();
        }
               
    }

    public void PlaceMinion(Unit minion, Tile tile, Player player){
        tile.PlaceUnit(minion);
        player.AddUnit(minion);
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

        UpdateFurthestColumnCanSpawn();

        if(ColumnAvailable()){
            currentPlayer.StartRecruiting(rolled, toRecruit);
        }
        else{
            currentPlayer.ActivateMovement();
        }

        
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

    public IEnumerator AttackNexus(Unit unit, PlayerType playerType){
        if(playerType == PlayerType.DOG){
            dogPlayer.nexus.TakeDamage(unit.attack);
            if(dogPlayer.nexus.health == 0){
                yield return dogPlayer.nexus.DeathAnimation();
                EndTussle(PlayerType.CAT);
            }
            else{
                yield return dogPlayer.nexus.HurtAnimation();
            }
        }
        else{
            catPlayer.nexus.TakeDamage(unit.attack);
            if(catPlayer.nexus.health == 0){
                yield return catPlayer.nexus.DeathAnimation();
                EndTussle(PlayerType.DOG);
            }
            else{
                yield return catPlayer.nexus.HurtAnimation();
            }
        }
        unit.TakeDamage(unit.health, null); // Kill the unit
    }

    private void UpdateFurthestColumnCanSpawn(){
        int first = currentTurn == PlayerType.DOG ? 0 : XSIZE - 1; 
        int direction = currentTurn == PlayerType.DOG ? 1 : -1; 
        
        int max = first - direction;

        for(int i = 0; i < XSIZE; i++){
            for(int j = 0; j < YSIZE; j++){
                if(mapArray[i,j].HasUnit() && mapArray[i,j].Unit.playertype == currentTurn){
                    if(currentTurn == PlayerType.DOG){
                        max = Mathf.Max(max, i);
                    }
                    else{
                        max = Mathf.Min(max, i);
                    }
                }
                
            }
        }
        bool firstOpen = false;
        for(int row = 0; row < YSIZE; row++){
            if(!mapArray[first,row].HasUnit()){
                firstOpen = true;
                break;
            }
        }

        if((max < 0 || max >= XSIZE) && firstOpen){
            furthestColumn = first;
        }
        else{
            if (currentTurn == PlayerType.DOG)
            {
                furthestColumn = Mathf.Min(max, XSIZE - 2);
            }
            else {
                furthestColumn = Mathf.Max(max, 1);
            }
        }

        foreach (Tile tile in mapArray) {
            if (currentTurn == PlayerType.DOG)
            {
                if (tile.xIndex <= furthestColumn && !tile.HasUnit())
                {
                    tile.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, .5f);
                }
                else
                {
                    tile.GetComponent<SpriteRenderer>().color = Color.clear;
                }
            }
            else
            {
                if (tile.xIndex >= furthestColumn && !tile.HasUnit())
                {
                    tile.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, .5f);
                }
                else
                {
                    tile.GetComponent<SpriteRenderer>().color = Color.clear;
                }
            }
        }

    }

    public void ResetTileColor() {
        foreach (Tile tile in mapArray)
        {
            tile.GetComponent<SpriteRenderer>().color = Color.clear;
        }
    }

    private bool ColumnAvailable(){
        return furthestColumn >= 0 && furthestColumn < XSIZE;
    }

    public bool ColumnInRange(int column){
        if(currentTurn == PlayerType.DOG){
            return column <= furthestColumn;
        }
        else{
            return column >= furthestColumn;
        }
    }


    private void EndTussle(PlayerType winner){
        gameOver = true;
        uiManager.End(winner);
    }

}
