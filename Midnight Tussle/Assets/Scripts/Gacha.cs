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

    [Header("Settings")]
    [SerializeField] private float[] startDistribution = new float[TussleManager.MAX_LEVEL];
    [SerializeField] private float[] finalDistribution = new float[TussleManager.MAX_LEVEL];
    private float[] differenceDistribution = new float[TussleManager.MAX_LEVEL];

    List<Unit>[] catUnits;
    List<Unit>[] dogUnits;

    #endregion

    void Awake(){
        for(int i = 0; i < TussleManager.MAX_LEVEL; i++){
            differenceDistribution[i] = finalDistribution[i] - startDistribution[i];
        }

        // Set up cat units
        catUnits = new List<Unit>[4]{catNormalUnits, catRareUnits, catEpicUnits, catLegendaryUnits};

        // Set up dog units
        dogUnits = new List<Unit>[4]{dogNormalUnits, dogRareUnits, dogEpicUnits, dogLegendaryUnits};
    }


    #region Gacha

    // Will roll count units, 
    public List<Unit> Roll(PlayerType roller, int count, int level) {

        float[] currentDist = currentDistribution(level);

        List<Unit> recruits = new List<Unit>(count); 

        for(int rollIndex = 0; rollIndex < count; rollIndex++){

            float random = Random.value;

            float sumRate = 0;

            for(int rarity = 0; rarity < TussleManager.MAX_LEVEL; rarity++){
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

    // Lerps between the start and final distributions according to level (1 returns start, MAX_LEVEL returns final)
    private float[] currentDistribution(int level){
        
        if(level == 1) return startDistribution;
        else if(level == TussleManager.MAX_LEVEL) return finalDistribution;

        float[] distribution = new float[TussleManager.MAX_LEVEL];
        
        float differenceFactor = (level - 1) / (TussleManager.MAX_LEVEL - 1);

        for(int i = 0; i < TussleManager.MAX_LEVEL; i++){
            distribution[i] = startDistribution[i] + differenceDistribution[i] * differenceFactor;
        }

        return distribution;
    }

    #endregion
}
