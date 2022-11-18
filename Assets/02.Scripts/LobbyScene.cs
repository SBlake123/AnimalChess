using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LobbyScene : MonoBehaviour
{
    public Button matchStartbtn;
    // Start is called before the first frame update
    void Start()
    {
        matchStartbtn.onClick.AddListener(MatchStart);
    }

    private void MatchStart()
    {
        PhotonManager.instance.OnJoinRandomRoomOrCreateRoom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
