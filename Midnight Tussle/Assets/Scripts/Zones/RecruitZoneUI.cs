using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class RecruitZoneUI : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private RecruitZone zoneData;

    [Header("Assets")]
    [SerializeField] private Sprite dogTreat;
    [SerializeField] private Sprite catTreat;

    [Header("References")]
    [SerializeField] private Image treatImage;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI summonText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI[] distributions = new TextMeshProUGUI[4]; 


    void Start(){
        costText.text = zoneData.cost.ToString();
        summonText.text = "Summon Count: " + zoneData.summonCount.ToString();
        nameText.text = zoneData.zoneName;
        for(int i = 0; i < 4; i++){
            distributions[i].text = string.Format("{0:0%}", zoneData.dist[i]);
        }
    }



    public void SetSpecies(PlayerType species){
        treatImage.sprite = species == PlayerType.DOG ? dogTreat : catTreat;
    }


    // public void SetState(bool state){
    //     gameObject.SetActive(state);
    // }

    public void SelectZone(){
        TussleManager.instance.AttemptBuyZone(zoneData);
    }

}
