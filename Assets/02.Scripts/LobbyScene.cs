using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LobbyScene : MonoBehaviour
{
    public Button nickNameBtn, makeARoomBtn, joinBtn;

    public TMP_InputField nickNameInput;
    public TextMeshProUGUI greetingText;

    public Canvas loginCanvas, matchStartCanvas, loadingCanvas, roomCanvas;

    public string[] animalArray = new string[] { "COW", "DOG", "PAPAGO", "MOMO", "DIDI", "KIKI" };
    // Start is called before the first frame update
    void Start()
    {
        loadingCanvas.enabled = false;
        roomCanvas.enabled = false;

        nickNameBtn.onClick.AddListener(JoinLobby);
        makeARoomBtn.onClick.AddListener(MakeARoom);
        joinBtn.onClick.AddListener(JoinARoom);
    }

    private void JoinLobby()
    {
        loginCanvas.enabled = false;
        roomCanvas.enabled = true;
        PhotonManager.instance.OnJoinedLobby(nickNameInput.text);
        if(nickNameInput.text.Length > 6)
        {
            nickNameInput.text = animalArray[UnityEngine.Random.Range(0, animalArray.Length)];
        }
        greetingText.text = $"Hello! {nickNameInput.text}!";
    }

    private void JoinARoom()
    {
        PhotonManager.instance.OnJoinedRoom();
    }

    private void MakeARoom()
    {
        PhotonManager.instance.OnCreatedRoom();
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
