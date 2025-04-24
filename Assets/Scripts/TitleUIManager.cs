using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class TitleUIManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Button createRoom;
    [SerializeField] Button joinRoom;
    [SerializeField] Button joinRandomRoom;
    [SerializeField] Button quitGame;

    [SerializeField] TMP_InputField roomNameInput;
    [SerializeField] TMP_Text roomNameLogo;
    [SerializeField] Button joinOrCreateRoomButton;

    private string roomName;
    private bool isCreateRoom = false;
    private bool isJoinRoom = false;
    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    // Start is called before the first frame update
    void Start()
    {
        
        PhotonNetwork.JoinLobby();
        roomNameInput.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateRoomButtonPressed()
    {
        roomNameInput.gameObject.SetActive(true);
        isCreateRoom = true;
    }
    public void JoinRoomButtonPressed()
    {
        roomNameInput.gameObject.SetActive(true);
        isJoinRoom = true;
    }
    public void JoinRandomRoomButtonPressed()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public void QuitGameButtonPressed()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit(); // 어플리케이션 종료
        #endif
    }



    public void JOCRButtonClick()
    {
        roomName = roomNameInput.text;
        if (isJoinRoom)
        {
            PhotonNetwork.JoinRoom(roomName);
            isJoinRoom=false;

        }

        if (isCreateRoom)
        {
            PhotonNetwork.CreateRoom(roomName);
            isCreateRoom=false;
        }
        roomNameInput.gameObject.SetActive(false);
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

    }
}
