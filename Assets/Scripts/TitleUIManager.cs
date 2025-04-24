using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

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
    private void Awake()
    {
        roomNameInput.gameObject.SetActive(false);
        buttonsPanel.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        EnterNickName();
        PhotonNetwork.AutomaticallySyncScene = true;

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
