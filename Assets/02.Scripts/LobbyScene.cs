using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LobbyScene : MonoBehaviour
{
    public Button matchStartbtn;
    public Canvas matchStartCanvas;
    public Canvas loadingCanvas;
    // Start is called before the first frame update
    void Start()
    {
        loadingCanvas.enabled = false;
        matchStartbtn.onClick.AddListener(MatchStart);
    }

    private void MatchStart()
    {
        matchStartCanvas.enabled = false;
        loadingCanvas.enabled = true;
        PhotonManager.instance.OnJoinRandomRoomOrCreateRoom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
