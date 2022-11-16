using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;

    public enum Player { player_one, player_two};
    public Player player;

    private void Awake()
    {
        instance = this;
    }

    public void TurnOver()
    {
        if (player == Player.player_one)
            player = Player.player_two;
        else
            player = Player.player_one;
        Debug.Log($"{player}'s Turn");
    }

    //�� ���϶��� �Է��� �����ϴ�. ���� ��� �ٲ��.
}
