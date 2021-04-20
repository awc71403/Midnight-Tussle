using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region Variables
    [Header("References")]
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private TextMeshProUGUI recruitText;
    
    private Animator animator;

    #endregion
    
    #region Init

    void Start(){
        animator = GetComponent<Animator>();
    }

    #endregion

    #region Functions

    public void StartTurn(PlayerType turn, int recruitCount){
        turnText.text = turn == PlayerType.DOG ? "Dog's Turn" : "Cat's Turn";
        recruitText.text = string.Format("{0} new recruits on their way", recruitCount);
        animator.Play("TurnStart");
    }

    public void End(PlayerType winner){
        turnText.text = winner == PlayerType.DOG ? "DOG team has taken the park!" : "CAT team has taken the park!";
        recruitText.text = "Thanks for playing!";
        animator.Play("GameOver");
    }


    #endregion
}
