using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class TitleUIManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField nickNameInput;


    [SerializeField] GameObject buttonsPanel;
    [SerializeField] Button createRoom;
    [SerializeField] Button joinRoom;
    [SerializeField] Button joinRandomRoom;
    [SerializeField] Button quitGame;

    [SerializeField] TMP_InputField roomNameInput;
    
    [SerializeField] TMP_Text roomNameLogo;
    
    [SerializeField] Button joinOrCreateRoomButton;

    [SerializeField] TMP_Text jocrButtonText;
    private string roomName;
    private bool isCreateRoom = false;
    private bool isJoinRoom = false;

    private RoomOptions roomOptions = new RoomOptions();

    private void Awake()
    {
        roomNameInput.gameObject.SetActive(false);
        buttonsPanel.SetActive(false);
        roomOptions.MaxPlayers = 2;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            nickNameInput.gameObject.SetActive(false);
        }
        else
        {
            EnterNickName();
            PhotonNetwork.AutomaticallySyncScene = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void EnterNickName()
    {
        nickNameInput.gameObject.SetActive(true); 
    }

    public void EnterNickNameButtonPressed()
    {

        PhotonNetwork.NickName = nickNameInput.text;

        PhotonNetwork.ConnectUsingSettings();
        
        
        nickNameInput.gameObject.SetActive(false);
        //roomNameInput.gameObject.SetActive(true);
        
    }
    public void CreateRoomButtonPressed()
    {
        roomNameInput.gameObject.SetActive(true);
        jocrButtonText.text = "Create Room";
        isCreateRoom = true;
    }
    public void JoinRoomButtonPressed()
    {
        roomNameInput.gameObject.SetActive(true);
        jocrButtonText.text = "Join Room";
        isJoinRoom = true;
    }
    public void JoinRandomRoomButtonPressed()
    {
        string randomeName = "room_" + Random.Range(1000, 9999);
        //PhotonNetwork.JoinRandomOrCreateRoom(null, 0, MatchmakingMode.FillRoom, null, null, null,roomOptions);
        PhotonNetwork.JoinRandomOrCreateRoom(roomOptions: roomOptions);
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
            
            PhotonNetwork.CreateRoom(roomName,roomOptions);
            isCreateRoom=false;
            
        }
        //roomNameInput.gameObject.SetActive(false);

    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        //Debug.Log("connect");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        //Debug.Log("join lob");
        base.OnJoinedLobby();
        buttonsPanel.SetActive(true);
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel("Room");
    }


}
