using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private TussleManager tussleManager;

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

    void Start(){
        StartTussle();
    }

    #endregion

    // The function used to begin a tussle in the actual "Tussle" scene
    public void StartTussle() {
        if(tussleManager == null){
            tussleManager = FindObjectOfType<TussleManager>();
        }

        tussleManager.StartTussle();
    }
}
