using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;
    public TextMeshProUGUI tMPUGUI;

    public enum Player { player_one, player_two};
    public Player player;
    public Player me;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            me = UnityEngine.Random.Range(0, 2) == 0 ? Player.player_one : Player.player_two;
            string enemy = me == Player.player_one ? (Player.player_two).ToString() : (Player.player_one).ToString();
            Debug.Log(enemy);
            PhotonManager.instance.DecidePlayer(enemy);
        }
        Debug.Log(PhotonNetwork.IsMasterClient);
        tMPUGUI.text = me.ToString();
    }

    public Player StringToEnum(string str)
    {
        return (Player)Enum.Parse(typeof(Player), str);
    }

    public void DecidePlayer(string player)
    {
        me = StringToEnum(player);
        tMPUGUI.text = me.ToString();
    }

    public void DecideTurn(string player)
    {
        this.player = StringToEnum(player);
        if (WinManager.instance.invadeSuccessCount == 1) WinManager.instance.turnOverCount++;
    }

    public void TurnOver()
    {
        if (player == Player.player_one)
            player = Player.player_two;
        else
            player = Player.player_one;

        PhotonManager.instance.DecideTurn(player.ToString());
        if (WinManager.instance.invadeSuccessCount == 1) WinManager.instance.turnOverCount++;
    }

    public bool MyTurn()
    {
        return player == me;
    }
    //내 턴일때만 입력이 가능하다. 턴은 계속 바뀐다.
}
