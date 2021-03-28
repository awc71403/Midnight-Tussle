using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecruitManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform recruitBox;
    [SerializeField] private TextMeshProUGUI remainingText;
    [SerializeField] private GameObject RecruitUIPrefab;

    private Vector2 bottom;
    private float height;

    void Start(){
        Vector3[] fourCorners = new Vector3[4];
        recruitBox.GetWorldCorners(fourCorners);
        bottom.x = (fourCorners[0].x + fourCorners[3].x)/2;
        bottom.y = fourCorners[0].y;
        height = fourCorners[1].y - fourCorners[0].y;
    }

    public void SetRemaining(int remainCount){
        remainingText.text = string.Format("{0} more will join the cause!", remainCount);
    }

    public void CreateRecruits(List<Unit> recruits){
        for(int i = 0; i < recruits.Count; i++){
            CreateRecruit(i, recruits.Count, recruits[i]);
        }
    }

    // Makes a RecruitUI for the unit and places in the right position
    public void CreateRecruit(int index, int totalCount, Unit unit){
        //Place it at (index+1/totalCount+1)*height and centered horizontally
        
        Vector2 position = new Vector2(bottom.x, ( (float) (index+1)/(totalCount+1) ) * height + bottom.y);

        //Instantiate RecruitUI under the manager
        Recruited recruit = Instantiate(RecruitUIPrefab, position, Quaternion.identity, transform).GetComponent<Recruited>();
        recruit.Setup(unit);       
    }

    // Removes all remaining recruits
    public void ClearRecruits(){
        foreach(Transform child in transform){
            Destroy(child.gameObject);
        }
        remainingText.text = "";
    }
}
