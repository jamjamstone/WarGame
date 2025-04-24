using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text hostPlayer;
    [SerializeField] TMP_Text guestPlayer;
    [SerializeField] Button gameStartButton;
    [SerializeField] TMP_Text gameStopButtonText;
    [SerializeField] Button readyButton;
    [SerializeField] TMP_Text readyButtonText;
    private bool isReady = false;
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            gameStartButton.enabled = true;
            readyButton.enabled = false;
        }
        else
        {
            gameStartButton.enabled=false;
            readyButton.enabled=true;
        }
    }


    public void ReadyButtonPressed()
    {
        if (!isReady)
        {
            readyButtonText.text = "Ready!";
            isReady = true;
        }
        else
        {
            readyButtonText.text = "Not Ready";
            isReady=false;
        }
    }
    public void GameStartButtonPressed()
    {
        if(isReady)
        {
           PhotonNetwork.LoadLevel("InGame");
        }



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
