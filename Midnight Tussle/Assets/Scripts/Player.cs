using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[RequireComponent(typeof(RecruitManager))]
public class Player : MonoBehaviour
{

    #region Variables 
    
    [Header("References")]
    [SerializeField] private PlayerUI playerUI;
    public Nexus nexus;


    private RecruitManager recruitManager;

    private int nexusHealth;

    private List<Unit> units = new List<Unit>();

    // Add pity

    // Add XP
    
    // Add UI

    #endregion

    #region Turn Variables

    private int countToRecruit;

    public bool movingPhase = false;
    private bool moveInProcess = false;

    #endregion

    #region Init

    void Start(){
        recruitManager = GetComponent<RecruitManager>();
    }

    #endregion

    #region Getter
    public List<Unit> GetUnits {
        get { return units; }
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

    // NOT ZERO-INDEXED!!
    public int GetLevel(){
        return 1;
    }

    public void StartRecruiting(List<Unit> rolled, int countToRecruit){
        //Check if there is a place to put units
        playerUI.HandState(true);
        this.countToRecruit = countToRecruit;
        recruitManager.SetRemaining(countToRecruit);
        recruitManager.CreateRecruits(rolled);
    
        
    }

    public void CallMovement(Direction direction) {
        StartCoroutine("Movement", direction);
    }

    // Returns true if there are still more to place
    public bool AddUnit(Unit unit){
        recruitManager.SetRemaining(--countToRecruit);
        units.Add(unit);
        unit.player = this;
        return countToRecruit != 0; // FIX ME add condition about no more placement spots available
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
        movingPhase = true;
    }

    IEnumerator Movement(Direction direction){
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
        if(!MovesLeft()){
            movingPhase = false;
            playerUI.MoveState(false);
            foreach(Unit unit in units){
                unit.MovementState(false);
            }
            TussleManager.instance.EndTurn();
        }        
    }

    private bool MovesLeft(){
        foreach(Unit unit in units){
            if(unit.movementLeft != 0) return true;
        }

        return false;
    }

    #endregion

    

}
