using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecruitManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform recruitBox;
    [SerializeField] private GameObject RecruitUIPrefab;

    private Vector2 bottomLeft;
    private float heightFourth;
    private float widthSixth;

    void Start(){
        Vector3[] fourCorners = new Vector3[4];
        recruitBox.GetWorldCorners(fourCorners);
        bottomLeft = fourCorners[0];
        heightFourth = Mathf.Abs(fourCorners[1].y - fourCorners[0].y) / 4;
        widthSixth = Mathf.Abs(fourCorners[3].x - fourCorners[0].x) / 6;
    }

    public void CreateRecruits(List<Unit> recruits){
        for(int i = 0; i < recruits.Count; i++){
            CreateRecruit(i, recruits.Count, recruits[i]);
        }
    }

    // Makes a RecruitUI for the unit and places in the right position
    public void CreateRecruit(int index, int totalCount, Unit unit){
        //Place it at (index+1/totalCount+1)*height and centered horizontally
        Vector2 position = GetPosition(index);

        //Instantiate RecruitUI under the manager
        Recruited recruit = Instantiate(RecruitUIPrefab, position, Quaternion.identity, transform).GetComponent<Recruited>();
        recruit.Setup(unit);       
    }

    // Removes all remaining recruits
    public void ClearRecruits(){
        foreach(Transform child in transform){
            Destroy(child.gameObject);
        }
    }

    private Vector2 GetPosition(int index){
        int x = index % 3; // Get the row
        int y = index / 3; // Get the column
        return bottomLeft + Vector2.up * y * heightFourth + Vector2.right * (x * 2 + 1) * widthSixth;
    }
}
