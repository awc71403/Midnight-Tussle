using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InforUI : MonoBehaviour
{
    #region TrackingVars
    private bool over;
    private RectTransform myrect;
    private Vector3 startingtransform;
    private Unit current = null;
    #endregion

    #region UI
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI abilityText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI atkText;
    [SerializeField] private TextMeshProUGUI moveText;
    [SerializeField] private Image rarity;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        over = false;
        myrect = GetComponent<RectTransform>();
        startingtransform = transform.position - Vector3.one * 1000;
    }

    // Update is called once per frame
    void Update()
    {
        if(over){
            Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(mousepos.x, mousepos.y);
        }

        if (over && (transform.position.x < Screen.width/2 && transform.position.x > 0) )
        {
            myrect.pivot = new Vector2(1, 1);
        }
        else
        {
            myrect.pivot = new Vector2(0, 1);
        }

        if (!over)
        {
            transform.position =startingtransform;
            myrect.pivot = new Vector2(1, 1);
        }
    }

    public void SetUnit(Unit unit){
        over = true;
        if(unit == current) return;

        current = unit;
        nameText.text = unit.characterName;
        abilityText.text = string.Format("Ability: <color=red>{0}</color>\n{1}", unit.ability.aName, unit.ability.aDesc);
        hpText.text = unit.initialHealth.ToString();
        atkText.text = unit.attack.ToString();
        moveText.text = unit.movement.ToString();
        rarity.sprite = TussleManager.raritySprites[unit.rarity];

    }

    public void ResetUnit(){
        current = null;
        over = false;

    }

}
