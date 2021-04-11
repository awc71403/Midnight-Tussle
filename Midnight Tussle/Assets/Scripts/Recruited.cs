using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Recruited : MonoBehaviour {

    [Header("References")]
    [SerializeField] private SpriteRenderer rarityImage;
    [SerializeField] private SpriteRenderer spriteImage;
    [SerializeField] private TextMeshPro HPText;
    [SerializeField] private TextMeshPro movementText;
    [SerializeField] private TextMeshPro attackText;

    [Header("Layer Mask")]
    [SerializeField] private LayerMask tileMask;

    [HideInInspector] public Unit recruit;

    [SerializeField] private float dragSpeed;
    private Rigidbody2D rb;
    private Camera main;

    private bool dragging = false;
    private Vector2 origin;

    //Checks if mouse is hovering over unti
    private bool Mouse_over;

    //Determins the GUI paramaters
    private float box_width = 150;
    private float box_height = 75;
    private float SCheight = Screen.height;



    #region Initialization
    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        main = Camera.main;
    }
    
    public void Setup(Unit recruit) {
        this.recruit = recruit;
        spriteImage.sprite = recruit.GetSprite();
        rarityImage.sprite = TussleManager.raritySprites[recruit.rarity + 1]; // +1 beecause of None sprite
        HPText.text = recruit.maxHealth.ToString();
        movementText.text = recruit.movement.ToString();
        attackText.text = recruit.attack.ToString();
        
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
            if(TussleManager.instance.ColumnInRange(tile.xIndex)){
                TussleManager.instance.PlaceUnitOnTile(recruit, tile);
                Destroy(this.gameObject);
            }
            

        }
    }

    #region GUI_abilities

    private void OnGUI()
    {

        if (!Mouse_over)
        {
            return;
        }
        //Debug.Log(transform.position);
        //Debug.Log(Input.mousePosition);
        //Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Vector2 worldPoint2d = new Vector2(worldPoint.x, worldPoint.y);
        GUI.Box(new Rect(Input.mousePosition.x, -Input.mousePosition.y+ SCheight, box_width, box_height), recruit.ability.aDesc);
    }
    private void OnMouseOver()
    {
        Mouse_over = true;
    }

    private void OnMouseExit()
    {
        Mouse_over = false;
    }
    #endregion
}
