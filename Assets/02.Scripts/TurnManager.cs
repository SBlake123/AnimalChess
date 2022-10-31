using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;

    public enum Player { player_one, player_two};
    public Player player = Player.player_one;

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

    //턴이 넘어가는 것은 말의 이동, 말을 재배치했을때;
    //게임보드에 칠드런이 변화할 때==
}
