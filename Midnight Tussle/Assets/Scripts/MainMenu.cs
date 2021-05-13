using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Mirror;

public class MainMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform JoinGroup;
    [SerializeField] private Transform HostGroup;
    [SerializeField] private Transform ButtonGroup;
    [SerializeField] private Transform WaitGroup;
    [SerializeField] private TMP_InputField addressField;

    private bool waitingForConnection = false;


    private void Start(){
        AudioManager.instance.PlayMusic("Title Theme");
    }

    void Update(){
        if(waitingForConnection && !NetworkClient.active){
            ShowWait(false);
        }
    }
    
    public void HostGame()
    {
        ButtonGroup.gameObject.SetActive(false);
        HostGroup.gameObject.SetActive(true);
        NetworkManager.singleton.StartHost();
    }


    public void ShowJoin(){
        ButtonGroup.gameObject.SetActive(false);
        JoinGroup.gameObject.SetActive(true);
    }

    private void ShowWait(bool state){
        WaitGroup.gameObject.SetActive(state);
        JoinGroup.gameObject.SetActive(!state);
    }

    public void JoinGame(){
        waitingForConnection = true;
        NetworkManager.singleton.StartClient();
        print(NetworkManager.singleton.networkAddress);
        ShowWait(true);
    }

    public void UpdateAddress(){
        NetworkManager.singleton.networkAddress = addressField.text;
    }


}
