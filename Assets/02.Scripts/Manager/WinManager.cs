using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class WinManager : MonoBehaviour
{
    public static WinManager instance;
    public string invadeSuccessPlayer;
    public int invadeSuccessCount;
    public int turnOverCount;
    public GameObject canvas;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        InvadeWin(turnOverCount, invadeSuccessPlayer);
    }

    public void LionDie(string player)
    {
        Debug.Log($"{player} Win!");
        canvas.SetActive(true);
        TextMeshProUGUI tMUGUI = canvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        tMUGUI.text = ($"{player} Win!");
        Time.timeScale = 0f;
    }

    public void InvadeSuccess(string player)
    {
        invadeSuccessCount = 1;
        invadeSuccessPlayer = player;
    }

    private void InvadeWin(int turnOverCount, string player)
    {
        if(turnOverCount == 2)
        {
            canvas.SetActive(true);
            TextMeshProUGUI tMUGUI = canvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            tMUGUI.text = ($"{player} Win!");
            Time.timeScale = 0f;
        }
    }

}
