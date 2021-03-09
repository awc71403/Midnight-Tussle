using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Unit : MonoBehaviour {

    protected string characterName;
    public int initialHealth;
    public int currentHealth;
    public int initialMovement;
    public int player;

    public Tile occupiedTile;

    // Sprite Rendering
    private SpriteRenderer myRenderer;
    private Shader shaderGUItext;
    private Shader shaderSpritesDefault;

    private UITracker myUITracker;

    [SerializeField]
    private Text DamageTextPrefab;
    public Text damageText;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private AudioClip[] stepSounds;
    private AudioSource audioSource;

    private const float stepDuration = 0.1f;

    // Movement Bounce Animation
    float totalStretch = 0.3f;
    float totalSquish = 0.3f;

    #region Abstract
    public abstract void Ability();
    #endregion

    #region Initialization
    void Awake() {
        myUITracker = Instantiate(GameManager.singleton.uiTrackerPrefab).GetComponent<UITracker>();
        myUITracker.TrackObject = gameObject;
    }

    void Start() {
        myRenderer = gameObject.GetComponent<SpriteRenderer>();
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = Shader.Find("Sprites/Default");
        audioSource = GetComponent<AudioSource>();
        SetHPFull();
    }

    public void Setup(UnitDatabaseSO.UnitData data, int team) {
        initialHealth = data.health;
        characterName = data.name;
        initialMovement = data.initialMovement;
        player = team;
        SetHPFull();
    }
    #endregion

    #region Getter and Setter
    public string Name {
        get { return characterName; }
    }

    public int GetHP {
        get { return currentHealth; }
    }

    public void SetHPFull() {
        currentHealth = initialHealth;
        UpdateUI();
    }

    public void RecalculateDepth() {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }

    public Tile OccupiedTile {
        get { return occupiedTile; }
        set { occupiedTile = value; }
    }
    #endregion

    #region Update
    private void UpdateUI() {
        myUITracker.GetComponentInChildren<TextMeshProUGUI>().text = currentHealth.ToString();
        myUITracker.transform.SetParent(GameManager.singleton.canvas.transform);
    }
    #endregion

    #region Movement
    public void InitialMovement() {

    }

    public void Movement() {
        if (player == GameManager.PLAYER1) {
            if (occupiedTile.Right != null && occupiedTile.Right.Unit == null) {
                StartCoroutine(MoveUnitInDirection(player));
            }
            else if (occupiedTile.Right.Unit.player != GameManager.PLAYER1) {
                //Fight
                int enemyHP = occupiedTile.Right.Unit.GetHP;
                occupiedTile.Right.Unit.TakeDamage(currentHealth);
                TakeDamage(enemyHP);
                if (currentHealth > 0) {
                    StartCoroutine(MoveUnitInDirection(player));
                }
            }
        }
        else {
            if (occupiedTile.Left != null && occupiedTile.Left.Unit == null) {
                StartCoroutine(MoveUnitInDirection(player));
            }
            else if (occupiedTile.Left.Unit.player != GameManager.PLAYER2) {
                //Fight
                int enemyHP = occupiedTile.Left.Unit.GetHP;
                occupiedTile.Left.Unit.TakeDamage(currentHealth);
                TakeDamage(enemyHP);
                if (currentHealth > 0) {
                    StartCoroutine(MoveUnitInDirection(player));
                }
            }
        }
    }

    IEnumerator MoveUnitInDirection(int player) {
        // Action in process!
        GameManager.actionInProcess = true;

        float tileSize = GetComponent<SpriteRenderer>().sprite.bounds.size.x;

        // Calculate the steps you need to take

        //Take that step!
        if (player == GameManager.PLAYER1) {
            transform.position += new Vector3(tileSize, 0);
            occupiedTile = occupiedTile.Right;
        }
        else {
            transform.position -= new Vector3(tileSize, 0);
            occupiedTile = occupiedTile.Left;
        }
        RecalculateDepth();
        StartBounceAnimation();
        yield return new WaitForSeconds(stepDuration);
        occupiedTile.PlaceUnit(this);

        // Action over!
        GameManager.actionInProcess = false;
    }
    #endregion

    #region Attack

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        if (currentHealth > 0) {
            StartCoroutine("HurtAnimation", damage);
        }
        else {
            StartCoroutine("DeathAnimation");
        }
        UpdateUI();
    }
    #endregion

    #region Sprite
    void WhiteSprite() {
        myRenderer.material.shader = shaderGUItext;
        myRenderer.color = Color.white;
    }

    void NormalSprite() {
        myRenderer.material.shader = shaderSpritesDefault;
        myRenderer.color = Color.white;
    }
    #endregion

    #region Animation
    IEnumerator HurtAnimation(int damage) {
        // Go white
        WhiteSprite();

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

        // Go normal
        NormalSprite();
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
        myUITracker.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        Destroy(myUITracker.gameObject);
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
    public void ResetStats() {
        currentHealth = initialHealth;
    }

    public void HPDamage(int damage) {
        currentHealth -= damage;
        if (currentHealth > 0) {
            StartCoroutine("HurtAnimation", damage);
        }
        else {
            StartCoroutine("DeathAnimation");
        }
    }
    #endregion
}
