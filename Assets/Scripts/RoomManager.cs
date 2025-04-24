using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

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

        SetNickName();



    }
    [PunRPC]
    public void SetNickName()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player != null)
            {
                if (player.IsMasterClient)
                {
                    hostPlayer.text = player.NickName;
                }
                else
                {
                    guestPlayer.text = player.NickName;
                }
            }
            
        }
    }
    
    public void ReadyButtonPressed()
    {
        photonView.RPC("ChangeReadyText", RpcTarget.All);//
    }

    [PunRPC]
    public void ChangeReadyText()
    {
        if (!isReady)
        {
            readyButtonText.text = "Ready!";
            isReady = true;
        }
        else
        {
            readyButtonText.text = "Not Ready";
            isReady = false;
        }
    }


    public void GameStartButtonPressed()
    {
        if(isReady)
        {
           PhotonNetwork.LoadLevel("InGameTestScene");
        }



    }
    public void QuitToTitleButtonPressed()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LeaveRoom();
            //PhotonNetwork.LoadLevel("Title");
        }
        else
        {
            PhotonNetwork.LeaveRoom();
            //SceneManager.LoadScene("Title");
        }

    }
    
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        photonView.RPC("SetNickName", RpcTarget.All);
    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        SceneManager.LoadScene("Title");
        photonView.RPC("SetNickName", RpcTarget.All);
    }

}
