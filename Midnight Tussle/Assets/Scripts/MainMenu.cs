using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance = null;


    private void Start(){
        AudioManager.instance.PlayMusic("Title Theme");
    }
    
    #region Scene_funcs
    public void SampleScene()
    {
        GameManager.instance.LoadTussle();
    }
    #endregion
}
