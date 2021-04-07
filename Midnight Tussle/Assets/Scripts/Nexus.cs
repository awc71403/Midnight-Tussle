using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Nexus : MonoBehaviour
{

    [SerializeField] private TextMeshPro HPText;

    SpriteRenderer myRenderer;

    public int health;

    void Start(){
        myRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damage){
        health = Mathf.Max(0, health - damage);
        HPText.text = health.ToString();
    }

    public IEnumerator HurtAnimation() {

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

    public IEnumerator DeathAnimation() {
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
}
