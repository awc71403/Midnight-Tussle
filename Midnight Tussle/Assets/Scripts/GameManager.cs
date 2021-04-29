using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    #region Initialization
    public void Awake() {
        // Singleton makes sure there is only one of this object
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }


    #endregion

    // The function used to begin a tussle in the actual "Tussle" scene
    public void LoadTussle() {
        AudioManager.instance.PlayMusic("Battle Theme");
        SceneManager.LoadScene("SampleScene");
    }
}
