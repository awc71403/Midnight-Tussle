using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Gacha : MonoBehaviour
{
    #region Variables

    [Header("Units")]
    [SerializeField] private List<Unit> catNormalUnits;
    [SerializeField] private List<Unit> catRareUnits;
    [SerializeField] private List<Unit> catEpicUnits;
    [SerializeField] private List<Unit> catLegendaryUnits;

    [SerializeField] private List<Unit> dogNormalUnits;
    [SerializeField] private List<Unit> dogRareUnits;
    [SerializeField] private List<Unit> dogEpicUnits;
    [SerializeField] private List<Unit> dogLegendaryUnits;

    List<Unit>[] catUnits;
    List<Unit>[] dogUnits;

    #endregion

    void Awake(){
        // Set up cat units
        catUnits = new List<Unit>[4]{catNormalUnits, catRareUnits, catEpicUnits, catLegendaryUnits};

        // Set up dog units
        dogUnits = new List<Unit>[4]{dogNormalUnits, dogRareUnits, dogEpicUnits, dogLegendaryUnits};
    }


    #region Gacha

    // Will roll count units, 
    public List<Unit> Roll(PlayerType roller, RecruitZone zoneData) {

        float[] currentDist = zoneData.dist;
        int count = zoneData.summonCount;

        List<Unit> recruits = new List<Unit>(count); 

        for(int rollIndex = 0; rollIndex < count; rollIndex++){

            float random = Random.value;

            float sumRate = 0;

            for(int rarity = 0; rarity < 4; rarity++){
                sumRate += currentDist[rarity];
                
                if(random <= sumRate){
                    Unit recruit;
                    if(roller == PlayerType.DOG){
                        recruit = Utils.RandomFromList(dogUnits[rarity]);
                        recruit.playertype = PlayerType.DOG;
                    }
                    else{
                        recruit = Utils.RandomFromList(catUnits[rarity]);
                        recruit.playertype = PlayerType.CAT;
                    }
                    recruit.rarity = rarity;
                    recruits.Add(recruit);

                    break;
                }
                
            }
            
        }

        return recruits;
    }


    #endregion
}
