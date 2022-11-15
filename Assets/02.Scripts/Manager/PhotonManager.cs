using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

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

        PhotonNetwork.LoadLevel(SSceneName.MAIN_SCENE);
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

}
