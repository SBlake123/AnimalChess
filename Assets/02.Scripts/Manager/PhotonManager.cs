using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        Screen.SetResolution(960, 540, false);
        DontDestroyOnLoad(this);
        // 호스트가 씬을 이동할 때, 다른 클라이언트들도 씬을 이동하게 하면서 동시에, 씬을 동기화시켜줌.
        // (서로 씬이 달라서 같은 포톤 뷰 개체를 못 찾아서 RPC함수 호출이 씹히는 문제를 막을 수 있음[RPC 손실 방지])
        PhotonNetwork.AutomaticallySyncScene = true;

        // 동기화 속도 늘려서 유저 이동이 끊어져 보이지 않게 함
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
        ConnectMasterServer();
    }

    
    #region SERVER
    public void ConnectMasterServer()
    {
        print("마스터 서버 접속 시도");
        
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print("마스터 서버에 연결 완료");

        if(!(SceneManager.GetActiveScene().name == SSceneName.LOBBY_SCENE))
        PhotonNetwork.LoadLevel(SSceneName.LOBBY_SCENE);
    }

    public void OnJoinedLobby(string text)
    {
        base.OnJoinedLobby();
        PhotonNetwork.LocalPlayer.NickName = text;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        print("연결 끊김. 로비로 이동.\n" +
            $"이유 [{cause}]");

    }

    public void OnJoinRandomRoomOrCreateRoom()
    {
        print("방 참가 혹은 생성");

        // 2명 참가 가능한 방
        PhotonNetwork.JoinRandomOrCreateRoom(null, 0, MatchmakingMode.FillRoom,
            null, null, null, new RoomOptions { MaxPlayers = 2 }, null);
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

    public void OnCreatedRoom(string roomName)
    {
        PhotonNetwork.CreateRoom(roomName == "" ? $"{PhotonNetwork.LocalPlayer.NickName}'s Room" : roomName , new RoomOptions { MaxPlayers = 2 });
        print("방 생성 완료");
    }

    // OnCreateRoom 함수 호출 뒤 이곳으로 들어옴
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("방 참가 완료");
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        print("방 떠남, 자동으로 게임 서버 연결 해제 후 마스터 서버 접속 시도..");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        print($"{otherPlayer.NickName}가 떠남");
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        print($"{newPlayer.NickName}가 방에 참가");

        // 게임 시작 후에는 유저가 못 들어옴 (게임 종료 후, 유저 나가면 다른 유저 들어오는 것 방지)
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(SSceneName.MAIN_SCENE);

        print("게임 씬으로 이동");
    }
    #endregion

    #region RPC
    public void AnimalMove(int parentCellx, int parentCelly, int nextCellx, int nextCelly)
    {
        photonView.RPC(nameof(AnimalMoveRPC), RpcTarget.OthersBuffered, parentCellx, parentCelly, nextCellx, nextCelly);
    }

    [PunRPC]
    private void AnimalMoveRPC(int parentCellx, int parentCelly, int nextCellx, int nextCelly)
    {
        FindObjectOfType<TouchManager>().AnimalMove(parentCellx, parentCelly, nextCellx, nextCelly);
    }

    public void AnimalToInven(int parentCellx, int parentCelly)
    {
        photonView.RPC(nameof(AnimalToInvenRPC), RpcTarget.OthersBuffered , parentCellx, parentCelly);
    }

    [PunRPC] private void AnimalToInvenRPC(int parentCellx, int parentCelly)
    {
        FindObjectOfType<TouchManager>().AnimalToInven(parentCellx, parentCelly);
    }

    public void AnimalComeBack(int invenCellx, int invenCelly, int parentCellx, int parentCelly)
    { 
        photonView.RPC(nameof(AnimalComeBackRPC), RpcTarget.OthersBuffered, invenCellx, invenCelly, parentCellx, parentCelly); 
    }

    [PunRPC] private void AnimalComeBackRPC(int invenCellx, int invenCelly, int parentCellx, int parentCelly)
    {
        FindObjectOfType<TouchManager>().AnimalComeBack(invenCellx, invenCelly, parentCellx, parentCelly);
    }

    public void DecidePlayer(string player)
    {
        photonView.RPC(nameof(DecidePlayerRPC), RpcTarget.OthersBuffered, player);
    }

    [PunRPC] private void DecidePlayerRPC(string player)
    {
        FindObjectOfType<TurnManager>().DecidePlayer(player);
    }

    public void DecideTurn(string player)
    {
        photonView.RPC(nameof(DecideTurnRPC), RpcTarget.OthersBuffered, player);
    }

    [PunRPC] private void DecideTurnRPC(string player)
    {
        FindObjectOfType<TurnManager>().DecideTurn(player);
    }

    public void LionDie(string player)
    {
        photonView.RPC(nameof(LionDieRPC), RpcTarget.OthersBuffered, player);
    }

    [PunRPC] private void LionDieRPC(string player)
    {
        FindObjectOfType<WinManager>().LionDie(player);
    }

    public void Evolve(string player)
    {
        photonView.RPC(nameof(EvolveRPC), RpcTarget.OthersBuffered, player);
    }

    [PunRPC] private void EvolveRPC(string player)
    {
        Chick[] chicklist = FindObjectsOfType<Chick>();
        foreach (Chick item in chicklist)
        {
            item.DecideEvolve(player);
        }
    }

    public void InvadeSuccess(string player)
    {
        photonView.RPC(nameof(InvadeSuccessRPC), RpcTarget.OthersBuffered, player);
    }

    [PunRPC] private void InvadeSuccessRPC(string player)
    {
        FindObjectOfType<WinManager>().InvadeSuccess(player);
    }
    #endregion
}
