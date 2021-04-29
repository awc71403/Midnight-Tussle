using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region Variables
    [Header("References")]
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private Transform recruitGroup;
    
    private Animator animator;
    private RecruitZoneUI[] zoneUI;

    #endregion
    
    #region Init

    void Awake(){
        animator = GetComponent<Animator>();
        zoneUI = recruitGroup.GetComponentsInChildren<RecruitZoneUI>();
    }

    #endregion

    #region Functions

    public void StartZone(PlayerType turn){
        turnText.text = turn == PlayerType.DOG ? "Dog's Turn" : "Cat's Turn";
        animator.Play("TurnStart");

        foreach(RecruitZoneUI zone in zoneUI){
            zone.SetSpecies(turn);
        }
         
    }

    public void StartPlacement(){
        animator.Play("StartPlacement");
    }

    public void End(PlayerType winner){
        turnText.text = winner == PlayerType.DOG ? "DOG team has taken the park!" : "CAT team has taken the park!";
        animator.Play("GameOver");
    }


    #endregion
}
