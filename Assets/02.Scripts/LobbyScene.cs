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
    public Button joinBtn;
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
    // Start is called before the first frame update
    void Start()
    {
        loadingCanvas.enabled = false;
        roomListCanvas.enabled = false;
        inRoomCanvas.enabled = false;

        masterAvatar.SetActive(false);
        nonMasterAvatar.SetActive(false);

        nickNameBtn.onClick.AddListener(JoinLobby);
        makeARoomBtn.onClick.AddListener(MakeARoom);
        joinBtn.onClick.AddListener(JoinARoom);
        bToLobbyBtn.onClick.AddListener(BackToLobby);
        playBtn.onClick.AddListener(StartGame);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void JoinLobby()
    {
        loginCanvas.enabled = false;
        roomListCanvas.enabled = true;

        if (nickNameInput.text.Length > 6)
        {
            nickNameInput.text = animalArray[UnityEngine.Random.Range(0, animalArray.Length)];
            PhotonManager.instance.OnJoinedLobby(nickNameInput.text);
        }
        else
            PhotonManager.instance.OnJoinedLobby(nickNameInput.text);

        greetingText.text = $"Hello! {PhotonNetwork.LocalPlayer.NickName}!";
    }

    private void JoinARoom()
    {
        roomListCanvas.enabled = false;
        inRoomCanvas.enabled = true;
        roomList.RoomListRenewal();
        PhotonManager.instance.OnJoinedRoom();
    }

    private void MakeARoom()
    {
        PhotonManager.instance.OnCreatedRoom(roomNameInput.text);
        roomListCanvas.enabled = false;
        inRoomCanvas.enabled = true;
        roomTitleText.text = roomNameInput.text;
        masterName.text = nickNameInput.text;
        masterAvatar.SetActive(true);
        roomList.RoomListRenewal();
    }

    private void StartGame()
    {
        
    }

    private void BackToLobby()
    {
        inRoomCanvas.enabled = false;
        roomListCanvas.enabled = true;
        PhotonNetwork.LeaveRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        photonView.RPC(nameof(RoomRenewalRPC), RpcTarget.AllBuffered);
        photonView.RPC(nameof(AvatarActive), RpcTarget.AllBuffered, true);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        photonView.RPC(nameof(RoomRenewalRPC), RpcTarget.AllBuffered);
        photonView.RPC(nameof(AvatarActive), RpcTarget.AllBuffered, false);
    }

    [PunRPC] private void RoomRenewalRPC()
    {
        nonMasterName.text = "";
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            nonMasterName.text = PhotonNetwork.PlayerList[i].NickName;
        masterName.text = PhotonNetwork.MasterClient.NickName;
    }

    [PunRPC] private void AvatarActive(bool selected)
    {
        nonMasterAvatar.SetActive(selected);
    }
}
