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

    private List<Unit> units;

    // Add pity

    // Add XP
    
    // Add UI

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

    public void StartRecruiting(List<Unit> rolled){
        recruitManager.CreateRecruits(rolled);
    }

    #endregion

}
