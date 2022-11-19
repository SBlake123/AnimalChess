using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class WinManager : MonoBehaviour
{
    public static WinManager instance;
    public string invadeSuccessPlayer;
    public bool lionDie;
    public int invadeSuccessCount;
    public int turnOverCount;
    public GameObject canvas;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        invadeSuccessPlayer = "None";
    }

    private void Update()
    {
        InvadeWin(turnOverCount, invadeSuccessPlayer);
    }

    public void LionDie(string player)
    {
        lionDie = true;
        CanvasActive(player);
    }

    public void InvadeSuccess(string player)
    {
        if (invadeSuccessPlayer == "None")
        {
            invadeSuccessCount = 1;
            invadeSuccessPlayer = player;
        }
    }

    private void InvadeWin(int turnOverCount, string player)
    {
        if (turnOverCount >= 1 && !lionDie)
        {
            CanvasActive(player);
        }
    }

    private void CanvasActive(string player)
    {
        canvas.SetActive(true);
        TextMeshProUGUI tMUGUI = canvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        tMUGUI.text = ($"{player} Win!");
        Time.timeScale = 0f;
    }

}
