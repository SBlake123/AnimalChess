using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;
    public TextMeshProUGUI turnText;
    public TextMeshProUGUI masterText;
    public TextMeshProUGUI clientText;

    public string[] playerArray = new string[2];

    public enum Player { player_one, player_two, none};
    public Player player;
    public Player me;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            me = UnityEngine.Random.Range(0, 2) == 0 ? Player.player_one : Player.player_two;
            string enemy = me == Player.player_one ? (Player.player_two).ToString() : (Player.player_one).ToString();
            PhotonManager.instance.DecidePlayer(enemy);
        }
        turnText.text = me.ToString();

        if (PhotonNetwork.IsMasterClient)
        {
            if (me == Player.player_one)
            {
                masterText.text = PhotonNetwork.PlayerList[0].NickName;
                clientText.text = PhotonNetwork.PlayerList[1].NickName;

                playerArray[0] = masterText.text;
                playerArray[1] = clientText.text;
            }
            else
            {
                masterText.text = PhotonNetwork.PlayerList[1].NickName;
                clientText.text = PhotonNetwork.PlayerList[0].NickName;

                playerArray[0] = masterText.text;
                playerArray[1] = clientText.text;
            }

            PhotonManager.instance.NameTransfer(masterText.text, clientText.text, playerArray[0], playerArray[1]);
            turnText.text = $"{masterText.text}'s Turn";
        }
        
    }

    public void NameTransfer(string oneName, string twoName, string arrayOne, string arrayTwo)
    {
        masterText.text = oneName;
        clientText.text = twoName;
        playerArray[0] = arrayOne;
        playerArray[1] = arrayTwo;
        turnText.text = $"{masterText.text}'s Turn";
    }

    public Player StringToEnum(string str)
    {
        return (Player)Enum.Parse(typeof(Player), str);
    }

    public void DecidePlayer(string player)
    {
        me = StringToEnum(player);
    }

    public void DecideTurn(string player)
    {
        this.player = StringToEnum(player);
        if(this.player == Player.player_one)
            turnText.text = $"{masterText.text}'s Turn";
        else
            turnText.text = $"{clientText.text}'s Turn";
        if (WinManager.instance.invadeSuccessCount == 1) WinManager.instance.turnOverCount++;
    }

    public void TurnOver()
    {
        if (player == Player.player_one)
        {
            player = Player.player_two;
            turnText.text = $"{clientText.text}'s Turn";
        }             
        else
        {
            player = Player.player_one;
            turnText.text = $"{masterText.text}'s Turn";
        }
        Debug.Log(WinManager.instance.turnOverCount);
        PhotonManager.instance.DecideTurn(player.ToString());
        if (WinManager.instance.invadeSuccessCount == 1) WinManager.instance.turnOverCount++;
    }

    public bool MyTurn()
    {
        return player == me;
    }

    public void BackToLobby()
    {
        PhotonManager.instance.backToLfromIn = true;
        PhotonNetwork.LoadLevel(SSceneName.LOBBY_SCENE);
    }
}
