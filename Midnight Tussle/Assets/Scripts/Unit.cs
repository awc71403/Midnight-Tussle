using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Unit : MonoBehaviour {

    #region Variables

    [Header("Stats")]
    public string characterName;
    public int maxHealth;
    public int movement;
    public int attack;

    public Ability ability;

    [Header("References")]
    [SerializeField] private TextMeshPro HPText;
    [SerializeField] private TextMeshPro attackText;
    [SerializeField] private TextMeshPro movementText;
    
    [HideInInspector] public PlayerType player;
    [HideInInspector] public int rarity;
    
    private int health;
    private void Updatehealth(int value) {
        health = value;
        HPText.text = value.ToString();
    }
    
    public Tile occupiedTile;

    // Sprite Rendering
    private SpriteRenderer myRenderer;

    [SerializeField]
    private Text DamageTextPrefab;
    public Text damageText;

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

    public int movementLeft;
    public void UpdateMovementLeft(int value){
        movementLeft = value;
        movementText.text = value.ToString();
    }

    #endregion

    #region Initialization
    
    void Awake() {
        myRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        health = maxHealth;

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
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }

    #endregion

    #region Update

    void Update(){

    }
    
    #endregion

    #region Movement
    public void MovementState(bool state){
        UpdateMovementLeft(movement);
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
        Tile target = occupiedTile.directionMap[direction];
        if(target != null){
            // Going to another tile
            if(!target.HasUnit()){
                occupiedTile.directionMap[direction].PlaceUnit(this);
                UpdateMovementLeft(movementLeft - 1);
            }
        }
        else{
            // Hitting edge
            UpdateMovementLeft(movementLeft - 1);
        }
        // RecalculateDepth();
        // StartBounceAnimation();
        yield return new WaitForSeconds(stepDuration);
        

    }
    #endregion

    #region Attack

    public void TakeDamage(int damage) {
        health -= damage;
        if (health > 0) {
            StartCoroutine("HurtAnimation", damage);
        }
        else {
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
        //Create Damage Text
        print("damage text created");
        damageText = Instantiate(DamageTextPrefab);
        Vector3 textPositionOffset = new Vector3(0, 1.25f, 0);
        damageText.transform.position = Camera.main.WorldToScreenPoint(transform.position + textPositionOffset);
        //damageText.GetComponent<DamageTextBehavior>().SetDamage(damage);

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

        myRenderer.color = new Color(1, 1, 1, 1);
        transform.localScale = new Vector3(1, 1, 1);
        gameObject.SetActive(false);
        // myUITracker.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
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

    public void HPDamage(int damage) {
        health = Mathf.Max(health - damage, 0);
        if (health != 0) {
            StartCoroutine("HurtAnimation", damage);
        }
        else {
            StartCoroutine("DeathAnimation");
        }
    }

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
