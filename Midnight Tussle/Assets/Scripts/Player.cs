using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;


[RequireComponent(typeof(RecruitManager))]
public class Player : NetworkBehaviour
{

    #region Variables 
    
    [Header("References")]
    [SerializeField] private PlayerUI playerUI;
    public Nexus nexus;

    private RecruitManager recruitManager;

    private int treatCount;

    #endregion

    #region Turn Variables

    private int countToRecruit;

    public bool movingPhase = false;
    public bool moveInProcess = false;

    #endregion

    #region Network Variables

    private SyncList<Unit> units = new SyncList<Unit>();

    #endregion

    #region Init

    void Start(){
        recruitManager = GetComponent<RecruitManager>();
        UpdateTreats(TussleManager.instance.startingTreats);
    }

    #endregion

    #region Getter
    public List<Unit> GetUnits {
        get { return units.ToList(); }
    }
    #endregion

    #region Functions

    void Update(){
        if(movingPhase && !moveInProcess){
            if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                StartCoroutine("Movement", Direction.LEFT);
            }
            else if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                StartCoroutine("Movement", Direction.RIGHT);
            }
            else if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                StartCoroutine("Movement", Direction.UP);
            }
            else if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                StartCoroutine("Movement", Direction.DOWN);
            }
            
        }
    }

    public void StartRecruiting(List<Unit> rolled){
        //Check if there is a place to put units
        playerUI.HandState(true);
        recruitManager.CreateRecruits(rolled);
    
        
    }

    public void ChargeAbility(Direction direction, Unit abilityUser) {
        StartCoroutine(ChargeMove(direction, abilityUser));
    }

    private IEnumerator ChargeMove(Direction direction, Unit abilityUser){
        moveInProcess = true;

        Queue<Unit> movementQueue = new Queue<Unit>(units);
        switch(direction){
            case Direction.LEFT:
                movementQueue.OrderBy(u => u.occupiedTile.xIndex);
                break;
            case Direction.RIGHT:
                movementQueue.OrderByDescending(u => u.occupiedTile.xIndex);
                break;
            case Direction.UP:
                movementQueue.OrderByDescending(u => u.occupiedTile.yIndex);
                break;
            case Direction.DOWN:
                movementQueue.OrderBy(u => u.occupiedTile.yIndex);
                break;
        }

        while(movementQueue.Count != 0){
            Unit unit = movementQueue.Dequeue();

            if(unit != abilityUser){
                // Try moving in direction
                yield return unit.MoveUnitInDirection(direction);
            }
            
        }

        moveInProcess = false;
    }

    // Returns true if there are still more to place
    public void AddUnit(Unit unit){
        units.Add(unit);
        unit.player = this;
    }

    public void RemoveUnit(Unit unit)
    {
        units.Remove(unit);
    }

    public void ActivateMovement(){
        // Recruting Over
        recruitManager.ClearRecruits();

        // UI
        playerUI.HandState(false);
        playerUI.MoveState(true);

        foreach(Unit unit in units){
            unit.MovementState(true);
        }

        TussleManager.instance.ResetTileColor();

        if(!MovesLeft()) EndMovement();
        else movingPhase = true;
    }

    IEnumerator Movement(Direction direction) {
       
        moveInProcess = true;

        // Arrow UI
        playerUI.SetArrowState(direction, true);

        // Order based on direction
        Queue<Unit> movementQueue = new Queue<Unit>(units.Where(u => u.movementLeft > 0));
        switch(direction){
            case Direction.LEFT:
                movementQueue.OrderBy(u => u.occupiedTile.xIndex);
                break;
            case Direction.RIGHT:
                movementQueue.OrderByDescending(u => u.occupiedTile.xIndex);
                break;
            case Direction.UP:
                movementQueue.OrderByDescending(u => u.occupiedTile.yIndex);
                break;
            case Direction.DOWN:
                movementQueue.OrderBy(u => u.occupiedTile.yIndex);
                break;
        }

        // Process one-by-one
        while(movementQueue.Count != 0){
            Unit unit = movementQueue.Dequeue();

            // Try moving in direction
            yield return unit.MoveUnitInDirection(direction);
        }

        // This movement is over
        playerUI.SetArrowState(direction, false);
        moveInProcess = false;

        // Stop if no more moves left
        if(!MovesLeft()) EndMovement();
    }

    private void EndMovement(){
        movingPhase = false;
        playerUI.MoveState(false);
        foreach(Unit unit in units){
            unit.MovementState(false);
        }
        if(!TussleManager.instance.gameOver) TussleManager.instance.EndTurn(); // Don't need to end turn if game over
    }

    private bool MovesLeft(){
        foreach(Unit unit in units){
            if(unit.movementLeft != 0) return true;
        }

        return false;
    }

    #region Treats

    public int GetTreats(){
        return treatCount;
    }

    public void UpdateTreats(int count){
        treatCount = count;
        playerUI.UpdateTreatCount(treatCount);
    }

    #endregion

    #endregion



    

}
