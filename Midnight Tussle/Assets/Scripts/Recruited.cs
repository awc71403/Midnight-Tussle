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

    [HideInInspector] public Unit recruit;

    [SerializeField] private float dragSpeed;
    private Rigidbody2D rb;
    private Camera main;

    private Vector2 origin;

    

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

    private void OnMouseDrag()
    {
        Vector2 mousePos = (Vector2)main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.zero, 1f);
        Vector2 direction = (mousePos - (Vector2)this.transform.position);
        rb.velocity = direction.normalized * dragSpeed * direction.magnitude;

    }

    private void OnMouseDown()
    {
        // AudioManager.instance.Play("pickup");
        
    }
    private void OnMouseUp()
    {
        
    }
}
