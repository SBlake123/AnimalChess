using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinManager : MonoBehaviour
{
    public static WinManager instance;
    public string invadeSuccessPlayer;
    public int invadeSuccessCount;

    private void Awake()
    {
        instance = this;
    }

    public void LionDie(string player)
    {
        Debug.Log($"{player} Win!");
        Time.timeScale = 0f;
    }

    public void InvadeSuccess(string player)
    {
        InvadeTrigger(player);
        if((TurnManager.instance.player).ToString() == player && invadeSuccessPlayer == player)
            Debug.Log( $"{player} Win!");
        Time.timeScale = 0f;
    }

    private void InvadeTrigger(string player)
    {
        if(invadeSuccessCount == 0)
        {
            invadeSuccessCount++;
            invadeSuccessPlayer = player;
        }
    }
}
