using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Unit : MonoBehaviour {

    #region Variables

    [Header("Stats")]
    public string characterName;
    public int initialHealth;
    public int movement;
    public int attack;

    public Ability ability;

    [Header("References")]
    [SerializeField] private TextMeshPro HPText;
    [SerializeField] private TextMeshPro attackText;
    [SerializeField] private TextMeshPro movementText;
    
    [HideInInspector] public PlayerType playertype;
    [HideInInspector] public int rarity;
    public Player player;

    public Unit killedBy;

    public int health;
    
    public Tile occupiedTile;

    // Sprite Rendering
    private SpriteRenderer myRenderer;


    private Animator animator;

    [SerializeField]
    private AudioClip[] stepSounds;
    private AudioSource audioSource;

    private const float stepDuration = 0.1f;

    // Movement Bounce Animation
    float totalStretch = 0.3f;
    float totalSquish = 0.3f;
    #endregion

    #region Turn Variables
    public Direction movingDirection;
    public int movementLeft;

    #endregion

    #region Initialization
    
    void Awake() {
        myRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        health = initialHealth;

    }

    void Start(){
        HPText.text = health.ToString();
        attackText.text = attack.ToString();
        movementText.text = movement.ToString();

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
        transform.position = new Vector3(occupiedTile.gameObject.transform.position.x, occupiedTile.gameObject.transform.position.y + .5f, occupiedTile.gameObject.transform.position.y);
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
        movementText.gameObject.SetActive(state);
    }

    public void Movementr() {
        // if (player == GameManager.PLAYER1) {
        //     if (occupiedTile.Right != null && occupiedTile.Right.Unit == null) {
        //         StartCoroutine(MoveUnitInDirection(player));
        //     }
        //     else if (occupiedTile.Right.Unit.player != GameManager.PLAYER1) {
        //         //Fight
        //         int enemyHP = occupiedTile.Right.Unit.GetHP;
        //         occupiedTile.Right.Unit.TakeDamage(currenthealth);
        //         TakeDamage(enemyHP);
        //         if (currenthealth > 0) {
        //             StartCoroutine(MoveUnitInDirection(player));
        //         }
        //     }
        // }
        // else {
        //     if (occupiedTile.Left != null && occupiedTile.Left.Unit == null) {
        //         StartCoroutine(MoveUnitInDirection(player));
        //     }
        //     else if (occupiedTile.Left.Unit.player != GameManager.PLAYER2) {
        //         //Fight
        //         int enemyHP = occupiedTile.Left.Unit.GetHP;
        //         occupiedTile.Left.Unit.TakeDamage(currenthealth);
        //         TakeDamage(enemyHP);
        //         if (currenthealth > 0) {
        //             StartCoroutine(MoveUnitInDirection(player));
        //         }
        //     }
        // }
    }

    public IEnumerator MoveUnitInDirection(Direction direction) {
        movingDirection = direction;
        Tile target = occupiedTile.directionMap[direction];
        if(target != null){
            // Going to another tile
            if(!target.HasUnit()){
                occupiedTile.directionMap[direction].PlaceUnit(this);
            } else
            {
                Debug.Log("encountered enemy");
                if (playertype != target.Unit.playertype)
                {
                    CheckAbilityCond(Ability.ActivationType.ATTACK);
                    Unit targetUnit = target.Unit;
                    targetUnit.TakeDamage(attack, this);
                    if(targetUnit.health > 0){
                        TakeDamage(targetUnit.attack, targetUnit);
                    }
                    else{
                        
                    }
                }
            }
        }
        else{
            if (playertype == PlayerType.DOG && direction == Direction.RIGHT){
                // Dog attacks Nexus
                yield return TussleManager.instance.AttackNexus(attack, PlayerType.CAT);
            }
            else if (playertype == PlayerType.CAT && direction == Direction.LEFT){
                yield return TussleManager.instance.AttackNexus(attack, PlayerType.DOG);
            }
        }
        movementLeft--;
        RecalculateDepth();
        // StartBounceAnimation();
        yield return new WaitForSeconds(stepDuration);
        

    }
    #endregion

    #region Attack

    public void TakeDamage(int damage, Unit from) {
        health -= damage;
        CheckAbilityCond(Ability.ActivationType.DAMAGE);
        if (health > 0) {
            StartCoroutine("HurtAnimation", damage);
        }
        else {
            killedBy = from;
            CheckAbilityCond(Ability.ActivationType.DEATH);
            occupiedTile.ClearUnit();
            player.RemoveUnit(this);
            //Might need to change locations
            StartCoroutine("DeathAnimation");
        }
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
        // loop over 0.5 second backwards
        print("death time");
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
}
