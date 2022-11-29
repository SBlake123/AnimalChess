using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyScene : MonoBehaviourPunCallbacks
{
    [Header("Button")]
    public Button nickNameBtn;
    public Button makeARoomBtn;
    public Button leaveBtn;
    public Button bToLobbyBtn;
    public Button playBtn;

    [Header("RoomList")]
    public RoomList roomList;

    [Header("Input Field")]
    public TMP_InputField nickNameInput;
    public TMP_InputField roomNameInput;

    [Header("Room Lobby Text")]
    public TextMeshProUGUI greetingText;
    public TextMeshProUGUI roomTitleText;

    [Header("InRoom Player Text")]
    public TextMeshProUGUI masterName;
    public TextMeshProUGUI nonMasterName;

    [Header("InRoom Avatar")]
    public GameObject masterAvatar;
    public GameObject nonMasterAvatar;

    [Header("Canvas")]
    public Canvas loginCanvas;
    public Canvas matchStartCanvas;
    public Canvas loadingCanvas;
    public Canvas roomListCanvas;
    public Canvas inRoomCanvas;

    [Header("Animal Array")]
    public string[] animalArray = new string[] { "COW", "DOG", "PAPAGO", "MOMO", "DIDI", "KIKI" };
    
    void Start()
    {
        loadingCanvas.enabled = false;
        roomListCanvas.enabled = false;
        inRoomCanvas.enabled = false;

        masterAvatar.SetActive(false);
        nonMasterAvatar.SetActive(false);

        nickNameBtn.onClick.AddListener(JoinLobby);
        makeARoomBtn.onClick.AddListener(MakeARoom);
        leaveBtn.onClick.AddListener(LeaveLobby);
        bToLobbyBtn.onClick.AddListener(BackToLobby);
        playBtn.onClick.AddListener(StartGame);

        if(PhotonManager.instance.backToLfromIn)
        {
            inRoomCanvas.enabled = true;
            RoomRenewal();
            PhotonManager.instance.backToLfromIn = false;
        }
    }
    #region SERVER

    private void JoinLobby()
    {
        PhotonNetwork.JoinLobby();
        loginCanvas.enabled = false;
        roomListCanvas.enabled = true;

        if (nickNameInput.text.Length > 6 || nickNameInput.text == "")
        {
            nickNameInput.text = animalArray[UnityEngine.Random.Range(0, animalArray.Length)];
            PhotonNetwork.LocalPlayer.NickName = nickNameInput.text;
        }
        else
            PhotonNetwork.LocalPlayer.NickName = nickNameInput.text;

        greetingText.text = $"Hello! {PhotonNetwork.LocalPlayer.NickName}!";
    }

    public override void OnJoinedLobby()
    {
        roomList.roomList.Clear();
    }

    private void LeaveLobby()
    {
        roomListCanvas.enabled = false;
        loginCanvas.enabled = true;
        PhotonNetwork.LeaveLobby();
    }

    private void MakeARoom()
    {
        OnCreatedRoom(roomNameInput.text);
        if (roomNameInput.text == "")
            roomNameInput.text = $"{nickNameInput.text}'s Room";

        roomListCanvas.enabled = false;
        inRoomCanvas.enabled = true;

        roomTitleText.text = roomNameInput.text;
        masterName.text = nickNameInput.text;
        masterAvatar.SetActive(true);
    }

    public void OnCreatedRoom(string roomName)
    {
        PhotonNetwork.CreateRoom(roomName == "" ? $"{PhotonNetwork.LocalPlayer.NickName}'s Room" : roomName, new RoomOptions { MaxPlayers = 2, IsVisible = true });
        print("LobbyScene. 방 생성 완료");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        roomListCanvas.enabled = false;
        inRoomCanvas.enabled = true;
        RoomRenewal();
        print("LobbyScene. 방 참가 완료");
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        JoinLobby();
        print("LobbyScene. 방을 떠나 로비로 돌아옴");
    }

    private void StartGame()
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount == 2 && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel(SSceneName.MAIN_SCENE);
        }
    }

    private void BackToLobby()
    {
        inRoomCanvas.enabled = false;
        roomListCanvas.enabled = true;
        PhotonNetwork.LeaveRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("방 생성 실패 :\n" +
            $"코드 : {returnCode}\n" +
            $"메세지 : {message}");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.Log("방 참가 실패 :\n" +
            $"코드 : {returnCode}\n" +
            $"메세지 : {message}");
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("OnPlayerEnteredRoom LobbyScene");
        RoomRenewal();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("OnPlayerLeftRoom LobbyScene");
        RoomRenewal();
    }

    private void RoomRenewal()
    {
        Debug.Log("RoomRenewal");
        masterAvatar.SetActive(true);
        masterName.text = PhotonNetwork.MasterClient.NickName;
        roomTitleText.text = PhotonNetwork.CurrentRoom.Name;

        for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if(PhotonNetwork.PlayerList.Length == 2)
            {
                nonMasterName.text = PhotonNetwork.PlayerList[1].NickName;
                nonMasterAvatar.SetActive(true);
            }
            else
            {
                nonMasterName.text = "";
                nonMasterAvatar.SetActive(false);
            }
        }
    }
    #endregion
}
