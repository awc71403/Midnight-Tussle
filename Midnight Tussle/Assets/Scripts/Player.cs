using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RecruitManager))]
public class Player : MonoBehaviour
{

    #region Variables 
    
    [Header("References")]
    [SerializeField] private PlayerUI playerUI;

    private RecruitManager recruitManager;

    private int nexusHealth;

    private List<Unit> units = new List<Unit>();

    // Add pity

    // Add XP
    
    // Add UI

    #endregion

    #region Turn Variables

    private int countToRecruit;

    #endregion

    #region Init

    void Start(){
        recruitManager = GetComponent<RecruitManager>();
    }

    #endregion


    #region Functions

    // NOT ZERO-INDEXED!!
    public int GetLevel(){
        return 1;
    }

    public void StartRecruiting(List<Unit> rolled, int countToRecruit){
        this.countToRecruit = countToRecruit;
        recruitManager.SetRemaining(countToRecruit);
        recruitManager.CreateRecruits(rolled);
    }

    // Returns true if there are still more to place
    public bool AddUnit(Unit unit){
        recruitManager.SetRemaining(--countToRecruit);
        units.Add(unit);
        return countToRecruit != 0; // FIX ME add condition about no more placement spots available
    }
    #endregion

}
