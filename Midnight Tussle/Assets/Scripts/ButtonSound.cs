using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{

    public void Hover(){
        AudioManager.instance.PlaySFX("Hover");
    }

    public void Select(bool cancel){
        if(cancel) AudioManager.instance.PlaySFX("Cancel");
        else AudioManager.instance.PlaySFX("Select");
    }
}
