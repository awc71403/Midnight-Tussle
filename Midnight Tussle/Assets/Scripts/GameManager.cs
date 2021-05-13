using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
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
        if(isServer) NetworkManager.singleton.ServerChangeScene("Networked");
    }
}
