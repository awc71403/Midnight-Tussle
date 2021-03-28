using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    #region Variables

    [Header("Group References")]
    [SerializeField] private RectTransform handGroup;
    [SerializeField] private RectTransform moveGroup;

    [Header("Arrow References")]
    [SerializeField] private Image arrowLeft;
    [SerializeField] private Image arrowRight;
    [SerializeField] private Image arrowUp;
    [SerializeField] private Image arrowDown;

    [Header("Arrow Sprites")]
    [SerializeField] private Sprite arrowLeft_Inactive;
    [SerializeField] private Sprite arrowLeft_Active;
    [SerializeField] private Sprite arrowRight_Inactive;
    [SerializeField] private Sprite arrowRight_Active;
    [SerializeField] private Sprite arrowUp_Inactive;
    [SerializeField] private Sprite arrowUp_Active;
    [SerializeField] private Sprite arrowDown_Inactive;
    [SerializeField] private Sprite arrowDown_Active;


    #endregion

    #region Functions

    public void HandState(bool state){
        handGroup.gameObject.SetActive(state);
    }

    public void MoveState(bool state){
        moveGroup.gameObject.SetActive(state);
    }

    public void SetArrowState(Direction direction, bool state){
        switch(direction){
            case Direction.LEFT:
                arrowLeft.sprite = state ? arrowLeft_Active : arrowLeft_Inactive;
                break;
            case Direction.RIGHT:
                arrowRight.sprite = state ? arrowRight_Active : arrowRight_Inactive;
                break;
            case Direction.UP:
                arrowUp.sprite = state ? arrowUp_Active : arrowUp_Inactive;
                break;
            case Direction.DOWN:
                arrowDown.sprite = state ? arrowDown_Active : arrowDown_Inactive;
                break;
        }
    }

    public void ResetArrows(){
        SetArrowState(Direction.LEFT, false);
        SetArrowState(Direction.RIGHT, false);
        SetArrowState(Direction.DOWN, false);
        SetArrowState(Direction.UP, false);
    }


    #endregion
}
