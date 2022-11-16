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
        // ȣ��Ʈ�� ���� �̵��� ��, �ٸ� Ŭ���̾�Ʈ�鵵 ���� �̵��ϰ� �ϸ鼭 ���ÿ�, ���� ����ȭ������.
        // (���� ���� �޶� ���� ���� �� ��ü�� �� ã�Ƽ� RPC�Լ� ȣ���� ������ ������ ���� �� ����[RPC �ս� ����])
        PhotonNetwork.AutomaticallySyncScene = true;

        // ����ȭ �ӵ� �÷��� ���� �̵��� ������ ������ �ʰ� ��
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
        ConnectMasterServer();
    }

    public void ConnectMasterServer()
    {
        print("������ ���� ���� �õ�");
        
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print("������ ������ ���� �Ϸ�");
        OnJoinRandomRoomOrCreateRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        print("���� ����. ������ �̵�.\n" +
            $"���� [{cause}]");

    }

    public void OnJoinRandomRoomOrCreateRoom()
    {
        print("�� ���� Ȥ�� ����");

        // 2�� ���� ������ ��
        PhotonNetwork.JoinRandomOrCreateRoom(null, 0, MatchmakingMode.FillRoom,
            null, null, null, new RoomOptions { MaxPlayers = 2 }, null);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("�� ���� ���� :\n" +
            $"�ڵ� : {returnCode}\n" +
            $"�޼��� : {message}");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.Log("�� ���� ���� :\n" +
            $"�ڵ� : {returnCode}\n" +
            $"�޼��� : {message}");
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("�� ���� �Ϸ�");
    }

    // OnCreateRoom �Լ� ȣ�� �� �̰����� ����
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("�� ���� �Ϸ�");
    }

    public void LeaveRoom()
    {
        print("�� ������ �õ�");

        Debug.Assert(PhotonNetwork.InRoom == true);
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        print("�� ����, �ڵ����� ���� ���� ���� ���� �� ������ ���� ���� �õ�..");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        print($"{otherPlayer.NickName}�� ����");
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        print($"{newPlayer.NickName}�� �濡 ����");

        // ���� ���� �Ŀ��� ������ �� ���� (���� ���� ��, ���� ������ �ٸ� ���� ������ �� ����)
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(SSceneName.MAIN_SCENE);

        print("���� ������ �̵�");
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
