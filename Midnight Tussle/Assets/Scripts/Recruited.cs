using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Recruited : MonoBehaviour {

    [Header("References")]
    [SerializeField] private SpriteRenderer rarityImage;
    [SerializeField] private SpriteRenderer spriteImage;
    [SerializeField] private SpriteRenderer treatImage;
    [SerializeField] private TextMeshPro costText;

    [Header("Assets")]
    [SerializeField] private Sprite dogTreat;
    [SerializeField] private Sprite catTreat;

    [Header("Layer Mask")]
    [SerializeField] private LayerMask tileMask;

    [HideInInspector] public Unit recruit;

    [SerializeField] private float dragSpeed;
    private Rigidbody2D rb;
    private Camera main;

    private bool dragging = false;
    private Vector2 origin;

    //Determins the GUI paramaters
    //private float box_width = 250;
    //private float box_height = 25;
    private float SCheight = Screen.height;
    //Get the UI object Info
    private InforUI InfoHolder;


    #region Initialization
    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        main = Camera.main;
        InfoHolder = FindObjectOfType<InforUI>();
    }
    
    public void Setup(Unit recruit) {
        this.recruit = recruit;
        spriteImage.sprite = recruit.GetSprite();
        rarityImage.sprite = TussleManager.raritySprites[recruit.rarity];
        costText.text = TussleManager.instance.treatsCostByRarity[recruit.rarity].ToString();
        treatImage.sprite = recruit.playertype == PlayerType.DOG ? dogTreat : catTreat;

        origin = transform.position;
    }
    #endregion

    void Update(){
        if(!dragging && rb.position != origin){
            Vector2 direction = (origin - (Vector2)this.transform.position);
            rb.velocity = direction.normalized * dragSpeed * direction.magnitude;
        }
    }

    private void OnMouseDrag()
    {
        Vector2 mousePos = (Vector2)main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - (Vector2)this.transform.position);
        rb.velocity = direction.normalized * dragSpeed * direction.magnitude;

        Collider2D collider = Physics2D.OverlapPoint(mousePos, tileMask);
        if (collider)
        {
            TussleManager tussle = TussleManager.instance;
            Tile hoveredTile = Tile.hoveredTile;
            if (Tile.hoveredTile != null) {
                if (hoveredTile.HasUnit()) {
                    Tile.hoveredTile.GetComponent<SpriteRenderer>().color = Color.clear;
                }
                else if (tussle.ColumnInRange(hoveredTile.xIndex)) {
                    Tile.hoveredTile.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, .5f);
                }
            }
            Tile tile = collider.GetComponent<Tile>();
            if (tussle.ColumnInRange(tile.xIndex))
            {
                if (!tile.HasUnit())
                {
                    tile.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, .25f);
                    Tile.hoveredTile = tile;
                }
            }
        }
    }

    private void OnMouseDown()
    {
        dragging = true;
    }

    private void OnMouseUp()
    {
        Vector2 mousePos = (Vector2)main.ScreenToWorldPoint(Input.mousePosition);
        dragging = false;
        Collider2D collider = Physics2D.OverlapPoint(mousePos, tileMask);
        if(collider){
            // Place the recruit onto the map
            Tile tile = collider.GetComponent<Tile>();
            TussleManager tussle = TussleManager.instance;
            if(tussle.ColumnInRange(tile.xIndex)){
                if (!tile.HasUnit()) {
                    tussle.AttemptBuyRecruit(this, tile);
                }
            }           
        }
    }

    #region GUI_abilities

    private void OnMouseOver()
    {
        InfoHolder.SetUnit(recruit);
    }

    private void OnMouseExit()
    {
        InfoHolder.ResetUnit();
    }
    #endregion
}
