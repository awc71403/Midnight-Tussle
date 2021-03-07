using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit Database", menuName = "CustomSO/TileDatabase")]
public class UnitDatabaseSO : ScriptableObject {

    [System.Serializable]
    public struct UnitData {
        public Unit unitPrefab;
        public int ID;
        public string name;

        public int health;
        public int initialMovement;

        public string abilityDesc;

        public Sprite unitSprite;
    }

    [SerializeField]
    public List<UnitData> catNormalUnits;
    [SerializeField]
    public List<UnitData> catRareUnits;
    [SerializeField]
    public List<UnitData> catEpicUnits;
    [SerializeField]
    public List<UnitData> catLegendaryUnits;

    [SerializeField]
    public List<UnitData> dogNormalUnits;
    [SerializeField]
    public List<UnitData> dogRareUnits;
    [SerializeField]
    public List<UnitData> dogEpicUnits;
    [SerializeField]
    public List<UnitData> dogLegendaryUnits;
}
