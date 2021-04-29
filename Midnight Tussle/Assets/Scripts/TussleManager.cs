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
    [SerializeField] private List<Sprite> temp_raritySprites = new List<Sprite>(4); // None, Common, Rare, Epic, Legendary
    public int startingTreats;
    public int[] treatsCostByRarity = new int[4];
    public int[] treatsRewardByRarity = new int[4];
    public int treatsPerTurn;

    private Gacha gachaMachine;

    private PlayerType currentTurn;
    private int turnCount = 0;

    private Tile[,] mapArray = new Tile[XSIZE, YSIZE];

    public const int XSIZE = 7;
    public const int YSIZE = 5;
    
    public Player currentPlayer {
        get { return currentTurn == PlayerType.DOG ? dogPlayer : catPlayer; }

    }

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
        StartTussle();
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

        // Balance - no new treat on first turn
        currentPlayer.UpdateTreats(currentPlayer.GetTreats() - treatsPerTurn);

        NewTurn();
    }

    public void PlaceUnitOnTile(Unit recruitPrefab, Tile tile) {
        // Instantiate an instance of the unit and place it on the given tile.
        Unit instantiated = Instantiate<Unit>(recruitPrefab, tile.transform.position, Quaternion.identity, transform); 
        AudioManager.instance.PlaySFX("Summon");
        tile.PlaceUnit(instantiated);
        currentPlayer.AddUnit(instantiated);
        UpdateFurthestColumnCanSpawn();
               
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
        currentPlayer.UpdateTreats(currentPlayer.GetTreats() + treatsPerTurn);
        uiManager.StartZone(currentTurn);
       
    }

    public void StartPlacement(RecruitZone zoneData){
        uiManager.StartPlacement();

        List<Unit> rolled = gachaMachine.Roll(currentTurn, zoneData);

        UpdateFurthestColumnCanSpawn();

        currentPlayer.StartRecruiting(rolled);

    }

    public void StartAttack(){
        currentPlayer.ActivateMovement();
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

    public void AttemptBuyZone(RecruitZone zoneData){
        if(currentPlayer.GetTreats() >= zoneData.cost){
            currentPlayer.UpdateTreats(currentPlayer.GetTreats() - zoneData.cost);
            StartPlacement(zoneData);
        }
    }

    public void AttemptBuyRecruit(Recruited recruited, Tile tile){
        int cost = treatsCostByRarity[recruited.recruit.rarity];
        if(currentPlayer.GetTreats() >= cost){
            currentPlayer.UpdateTreats(currentPlayer.GetTreats() - cost);
            PlaceUnitOnTile(recruited.recruit, tile);
            Destroy(recruited.gameObject);
        }
    }

    public void GiveTreatFromDeath(PlayerType killedType, int rarity){
        Player killer = killedType == PlayerType.DOG ? catPlayer : dogPlayer;
        killer.UpdateTreats(killer.GetTreats() + treatsRewardByRarity[rarity]);
    }

    public IEnumerator AttackNexus(Unit unit, PlayerType playerType){
        if(playerType == PlayerType.DOG){
            dogPlayer.nexus.TakeDamage(unit.attack + unit.health);
            if(dogPlayer.nexus.health == 0){
                yield return dogPlayer.nexus.DeathAnimation();
                EndTussle(PlayerType.CAT);
            }
            else{
                yield return dogPlayer.nexus.HurtAnimation();
            }
        }
        else{
            catPlayer.nexus.TakeDamage(unit.attack + unit.health);
            if(catPlayer.nexus.health == 0){
                yield return catPlayer.nexus.DeathAnimation();
                EndTussle(PlayerType.DOG);
            }
            else{
                yield return catPlayer.nexus.HurtAnimation();
            }
        }
        unit.occupiedTile.ClearUnit();
        unit.player.RemoveUnit(unit);
        Destroy(unit.gameObject);
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
                furthestColumn = Mathf.Min(max, XSIZE / 2);
            }
            else {
                furthestColumn = Mathf.Max(max, XSIZE / 2);
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
        AudioManager.instance.PlayMusic("Game Over");
        uiManager.End(winner);
    }

}
