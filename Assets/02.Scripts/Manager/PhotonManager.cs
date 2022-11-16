using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
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
        
        DontDestroyOnLoad(this);
        // 호스트가 씬을 이동할 때, 다른 클라이언트들도 씬을 이동하게 하면서 동시에, 씬을 동기화시켜줌.
        // (서로 씬이 달라서 같은 포톤 뷰 개체를 못 찾아서 RPC함수 호출이 씹히는 문제를 막을 수 있음[RPC 손실 방지])
        PhotonNetwork.AutomaticallySyncScene = true;

        // 동기화 속도 늘려서 유저 이동이 끊어져 보이지 않게 함
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
        ConnectMasterServer();
    }

    public void ConnectMasterServer()
    {
        print("마스터 서버 접속 시도");
        
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print("마스터 서버에 연결 완료");
        OnJoinRandomRoomOrCreateRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        print("연결 끊김. 방으로 이동.\n" +
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

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("방 생성 완료");
    }

    // OnCreateRoom 함수 호출 뒤 이곳으로 들어옴
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("방 참가 완료");
    }

    public void LeaveRoom()
    {
        print("방 떠나기 시도");

        Debug.Assert(PhotonNetwork.InRoom == true);
        PhotonNetwork.LeaveRoom();
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

    public void AnimalMove(int parentCellx, int parentCelly, int nextCellx, int nextCelly)
    {
        photonView.RPC(nameof(AnimalMoveRPC), RpcTarget.AllBuffered, parentCellx, parentCelly, nextCellx, nextCelly);
    }
    [PunRPC] public void AnimalMoveRPC(int parentCellx, int parentCelly, int nextCellx, int nextCelly)
    {
        FindObjectOfType<TouchManager>().AnimalMove(parentCellx, parentCelly, nextCellx, nextCelly);
    }

    public void AnimalToInven(int parentCellx, int parentCelly, string tag)
    {
        photonView.RPC(nameof(AnimalToInvenRPC), RpcTarget.AllBuffered, parentCellx, parentCelly, tag);
    }

    [PunRPC]
    public void AnimalToInvenRPC(int parentCellx, int parentCelly, string tag)
    {
        FindObjectOfType<TouchManager>().AnimalToInven(parentCellx, parentCelly, tag);
    }

    public void AnimalComeBack(Vector3 transform, string tag)
    {
        photonView.RPC(nameof(AnimalComeBackRPC), RpcTarget.AllBuffered, transform, tag); ;
    }

    [PunRPC] public void AnimalComeBackRPC(Vector3 transform, string tag)
    {
        FindObjectOfType<TouchManager>().AnimalComeBack(transform, tag);
    }


}
