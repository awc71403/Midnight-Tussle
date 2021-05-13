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

    public void previousImage()
    {
        currentimage--;
        if (currentimage < 0)
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

    public void startslideshow()
    {
        holder.SetActive(true);
        nextImage();
    }
}
