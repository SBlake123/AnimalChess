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

    public bool backToLfromIn;

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
        PhotonNetwork.AutomaticallySyncScene = true;

        // ����ȭ �ӵ� �÷��� ���� �̵��� ������ ������ �ʰ� ��
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
        ConnectMasterServer();
    }


    #region SERVER
    public void ConnectMasterServer()
    {
        print("������ ���� ���� �õ�");

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print("������ ������ ���� �Ϸ�");

        if (!(SceneManager.GetActiveScene().name == SSceneName.LOBBY_SCENE))
            PhotonNetwork.LoadLevel(SSceneName.LOBBY_SCENE);
        else
            PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        print("���� ����. �κ�� �̵�.\n" +
            $"���� [{cause}]");
    }
    #endregion

    #region IngameRPC
    public void NameTransfer(string oneName, string twoName, string arrayOne, string arrayTwo)
    {
          photonView.RPC(nameof(NameTransferRPC), RpcTarget.OthersBuffered, oneName, twoName, arrayOne, arrayTwo);
    }

    [PunRPC]
    private void NameTransferRPC(string oneName, string twoName, string arrayOne, string arrayTwo)
    {
        FindObjectOfType<TurnManager>().NameTransfer(oneName, twoName, arrayOne, arrayTwo);
    }

    public void AnimalMove(int parentCellx, int parentCelly, int nextCellx, int nextCelly)
    {
        photonView.RPC(nameof(AnimalMoveRPC), RpcTarget.OthersBuffered, parentCellx, parentCelly, nextCellx, nextCelly);
    }

    [PunRPC]
    private void AnimalMoveRPC(int parentCellx, int parentCelly, int nextCellx, int nextCelly)
    {
        FindObjectOfType<MoveManager>().AnimalMove(parentCellx, parentCelly, nextCellx, nextCelly);
    }

    public void AnimalToInven(int parentCellx, int parentCelly)
    {
        photonView.RPC(nameof(AnimalToInvenRPC), RpcTarget.OthersBuffered, parentCellx, parentCelly);
    }

    [PunRPC]
    private void AnimalToInvenRPC(int parentCellx, int parentCelly)
    {
        FindObjectOfType<MoveManager>().AnimalToInven(parentCellx, parentCelly);
    }

    public void AnimalComeBack(int invenCellx, int invenCelly, int parentCellx, int parentCelly)
    {
        photonView.RPC(nameof(AnimalComeBackRPC), RpcTarget.OthersBuffered, invenCellx, invenCelly, parentCellx, parentCelly);
    }

    [PunRPC]
    private void AnimalComeBackRPC(int invenCellx, int invenCelly, int parentCellx, int parentCelly)
    {
        FindObjectOfType<MoveManager>().AnimalComeBack(invenCellx, invenCelly, parentCellx, parentCelly);
    }

    public void DecidePlayer(string player)
    {
        photonView.RPC(nameof(DecidePlayerRPC), RpcTarget.OthersBuffered, player);
    }

    [PunRPC]
    private void DecidePlayerRPC(string player)
    {
        FindObjectOfType<TurnManager>().DecidePlayer(player);
    }

    public void DecideTurn(string player)
    {
        photonView.RPC(nameof(DecideTurnRPC), RpcTarget.OthersBuffered, player);
    }

    [PunRPC]
    private void DecideTurnRPC(string player)
    {
        FindObjectOfType<TurnManager>().DecideTurn(player);
    }

    public void LionDie(string player)
    {
        photonView.RPC(nameof(LionDieRPC), RpcTarget.OthersBuffered, player);
    }

    [PunRPC]
    private void LionDieRPC(string player)
    {
        FindObjectOfType<WinManager>().LionDie(player);
    }

    public void Evolve(string player)
    {
        photonView.RPC(nameof(EvolveRPC), RpcTarget.OthersBuffered, player);
    }

    [PunRPC]
    private void EvolveRPC(string player)
    {
        Chicken[] chicklist = FindObjectsOfType<Chicken>();
        foreach (Chicken item in chicklist)
        {
            item.DecideEvolve(player);
        }
    }

    public void InvadeSuccess(string player)
    {
        photonView.RPC(nameof(InvadeSuccessRPC), RpcTarget.OthersBuffered, player);
    }

    [PunRPC]
    private void InvadeSuccessRPC(string player)
    {
        FindObjectOfType<WinManager>().InvadeSuccess(player);
    }
    #endregion
}
