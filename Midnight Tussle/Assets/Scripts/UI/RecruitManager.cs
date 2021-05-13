using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class RecruitManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject RecruitUIPrefab;
    [SerializeField] private Transform spawnParent;

    private Vector2 bottomLeft;
    private float heightFourth;
    private float widthSixth;

    void Awake(){
        // Register the prefab for the network spawn
        // NetworkManager.singleton.spawnPrefabs.Add(RecruitUIPrefab);
    }

    public void CreateRecruits(List<Unit> recruits){
        for(int i = 0; i < recruits.Count; i++){
            CreateRecruit(i, recruits.Count, recruits[i]);
        }
    }

    // Makes a RecruitUI for the unit and places in the right position
    public void CreateRecruit(int index, int totalCount, Unit unit){
        // Find the position
        Vector3 position = spawnParent.GetChild(index).position;

        //Instantiate RecruitUI under the manager
        Recruited recruit = Instantiate(RecruitUIPrefab, position, Quaternion.identity, transform).GetComponent<Recruited>();
        // NetworkServer.Spawn(recruit.gameObject);
        recruit.Setup(unit);       
    }

    // Removes all remaining recruits
    public void ClearRecruits(){
        foreach(Transform child in transform){
            Destroy(child.gameObject);
        }
    }

}
