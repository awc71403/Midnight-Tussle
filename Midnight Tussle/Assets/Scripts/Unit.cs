using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Unit : MonoBehaviour {

    #region Variables

    [Header("Stats")]
    [Tooltip("Name of this specific unit")]
    public string characterName;
    [Tooltip("Amount of healtht he unit starts with")]
    public int initialHealth;
    [Tooltip("Amount of damage this unit deals to enemies")]
    public int attack;
    [Tooltip("Number of tiles the unit can move every turn")]
    public int movement;


    public Ability ability;

    [Header("References")]
    [SerializeField] private TextMeshPro HPText;
    [SerializeField] private TextMeshPro movementText;

    [HideInInspector] public PlayerType playertype;
    [HideInInspector] public int rarity;
    [Tooltip("A reference to the player object which controlls the units")]
    [HideInInspector] public Player player;

    [HideInInspector] public Unit killedBy;

    /*[HideInInspector]*/ public int health;
    [Tooltip("holds a reference of the tile that is currently occupied")]
    [HideInInspector] public Tile occupiedTile;

    [HideInInspector] public bool stuck;

    // Sprite Rendering
    private SpriteRenderer myRenderer;

    //Determins the GUI paramaters
    //private float box_width= 250;
    //private float box_height=25;
    //Get the UI object Info
    private InforUI InfoHolder;

    private Animator animator;

    [Header("Sound")]
    [SerializeField]
    private AudioClip[] stepSounds;
    private AudioSource audioSource;

    private const float stepDuration = 0.1f;

    // Movement Bounce Animation
    float totalStretch = 0.3f;
    float totalSquish = 0.3f;
    #endregion

    #region Turn Variables
    [HideInInspector] public Direction movingDirection;
    [HideInInspector] public int movementLeft;

    #endregion

    #region Initialization

    void Awake() {
        myRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        health = initialHealth;
        InfoHolder = FindObjectOfType<InforUI>();
        stuck = false;
    }

    void Start(){
        HPText.text = health.ToString();
        movementText.text = movement.ToString();

        movementLeft = movement;
        CheckAbilityCond(Ability.ActivationType.SUMMON);
    }

    #endregion

    #region Getter and Setter
    public string Name {
        get { return characterName; }
    }

    // public int GetHP {
    //     get { return currenthealth; }
    // }

    public void RecalculateDepth() {
        transform.position = new Vector3(occupiedTile.gameObject.transform.position.x, occupiedTile.gameObject.transform.position.y + .28f, occupiedTile.gameObject.transform.position.y);
    }

    #endregion

    #region Update

    void Update(){
        HPText.text = health.ToString();
        movementText.text = movementLeft.ToString();
    }

    #endregion

    #region Movement
    public void MovementState(bool state){
        movementLeft = movement;
        movementText.transform.parent.gameObject.SetActive(state);
    }

    public IEnumerator MoveUnitInDirection(Direction direction) {
        if (stuck) {
            stuck = false;
            yield break;
        }
        movingDirection = direction;
        Tile target = occupiedTile.directionMap[direction];
        if(target != null){
            // Going to another tile
            if(!target.HasUnit()){
                AudioManager.instance.PlaySFX("Move");
                occupiedTile.directionMap[direction].PlaceUnit(this);
                InfoHolder.ResetUnit();
            } else
            {
                Debug.Log("encountered enemy");
                if (playertype != target.Unit.playertype)
                {
                    Unit targetUnit = target.Unit;
                    CheckAbilityCond(Ability.ActivationType.ATTACK);

                    yield return targetUnit.TakeDamage(attack, this);
                    if(targetUnit.health > 0) yield return TakeDamage(targetUnit.attack, targetUnit);
                }
            }
        }
        else{
            if (playertype == PlayerType.DOG && direction == Direction.RIGHT){
                // Dog attacks Nexus
                yield return TussleManager.instance.AttackNexus(this, PlayerType.CAT);
            }
            else if (playertype == PlayerType.CAT && direction == Direction.LEFT){
                yield return TussleManager.instance.AttackNexus(this, PlayerType.DOG);
            }
        }
        movementLeft--;
        RecalculateDepth();
        // StartBounceAnimation();
        yield return new WaitForSeconds(stepDuration);


    }
    #endregion

    #region Attack

    public void TakeDamageBase(int damage, Unit from){
        StartCoroutine(TakeDamage(damage, from));
    }

    public IEnumerator TakeDamage(int damage, Unit from) {
        health -= damage;
        CheckAbilityCond(Ability.ActivationType.DAMAGE);
        if (health > 0) {
            yield return HurtAnimation(damage);
        }
        else {
            killedBy = from;
            CheckAbilityCond(Ability.ActivationType.DEATH);
            if(occupiedTile.Unit == this) occupiedTile.ClearUnit(); //condition needed for some abilities
            player.RemoveUnit(this);

            //Might need to change locations
            yield return DeathAnimation();
        }
    }

    public void Revenged(){
        player.RemoveUnit(this);
        StartCoroutine(DeathAnimation());
    }
    #endregion

    #region Ability
    public void CheckAbilityCond(Ability.ActivationType type) {
        Debug.Log(ability);
        if (ability.type == type) {
            ability.TriggerAbility(this);
        }
    }
    #endregion

    #region Animation
    IEnumerator HurtAnimation(int damage) {
        AudioManager.instance.PlaySFX("Move");

        // Shaking
        Vector3 defaultPosition = transform.position;
        System.Random r = new System.Random();
        for (int i = 0; i < 5; i++) {
            double horizontalOffset = r.NextDouble() * 0.2 - 0.1f;
            Vector3 vectorOffset = new Vector3((float)horizontalOffset, 0, 0);
            transform.position += vectorOffset;
            yield return new WaitForSeconds(0.025f);
            transform.position = defaultPosition;
        }

    }

    IEnumerator DeathAnimation() {
        AudioManager.instance.PlaySFX("Death");
        // loop over 0.5 second backwards
        for (float i = 0.25f; i >= 0; i -= Time.deltaTime) {
            // set color with i as alpha
            myRenderer.color = new Color(1, 1, 1, i);
            transform.localScale = new Vector3(1.5f - i, 1.5f - i, 1);
            yield return null;
        }

        // myUITracker.gameObject.SetActive(false);
        yield return null; //Just to make sure any logic that needed to run this frame gets run
        Destroy(gameObject);
        // Destroy(myUITracker.gameObject);
    }

    public void StartBounceAnimation() {
        StartCoroutine("BounceAnimation");
    }

    IEnumerator BounceAnimation() {
        int frames = 3;
        //Vector3 originalPosition = transform.position;
        float stretch = totalStretch;
        float squish = totalSquish;
        for (int i = frames; i > 0; i--) {
            transform.localScale = new Vector3(1 + stretch, 1 - squish, 1);
            yield return new WaitForSeconds(0.01f);
            stretch /= 2.5f;
            squish /= 2.5f;
        }
        transform.localScale = new Vector3(1, 1, 1);

        // Play random step sound
        System.Random r = new System.Random();
        int stepNum = r.Next(0, stepSounds.Length);
        //audioSource.clip = stepSounds[stepNum];
        //audioSource.Play();
    }
    #endregion

    #region Stats

    public void IncreaseHP(int amount) {
        health += amount;
        HPText.text = health.ToString();
    }
    #endregion


    public Sprite GetSprite(){
        //FIX ME
        return GetComponent<SpriteRenderer>().sprite;
    }

    #region GUI_abilities

    private void OnMouseOver()
    {
        InfoHolder.SetUnit(this);
    }

    private void OnMouseExit()
    {
        InfoHolder.ResetUnit();
    }
    #endregion
}
