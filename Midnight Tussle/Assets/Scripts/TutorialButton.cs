using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialButton : MonoBehaviour
{

    [SerializeField]
    private GameObject holder;
    [SerializeField]
    private Sprite[] sprites;
    [SerializeField]
    private string[] instructions;
    [SerializeField]
    private Image slideshow;
    [SerializeField]
    private TextMeshProUGUI showntext;
    private int currentimage;

    private void Start()
    {
        currentimage = -1;
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                prevImage();
                AudioManager.instance.PlaySFX("Cancel");
            }
            else if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                nextImage();
                AudioManager.instance.PlaySFX("Select");
            }
    }

    public void nextImage()
    {
        
        currentimage++;
        if( currentimage>= instructions.Length)
        {
            currentimage = -1;
            holder.SetActive(false);
        }
        else
        {
            slideshow.sprite = sprites[currentimage];
            showntext.text = instructions[currentimage];
        }
    }

    public void prevImage(){
        
        if( currentimage != 0)
        {
            currentimage--;
            slideshow.sprite = sprites[currentimage];
            showntext.text = instructions[currentimage];
        }
    }

    public void startslideshow()
    {
        holder.SetActive(true);
        nextImage();
    }
}
