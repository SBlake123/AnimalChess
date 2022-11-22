using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomList : MonoBehaviourPunCallbacks
{
    public Button[] cellBtn;
    public Button previousBtn, nextBtn;
    public List<RoomInfo> roomList =new List<RoomInfo>();

    private int currentPage = 1, maxPage, multiple;


    public void RoomListRenewal()
    {
        maxPage = (roomList.Count % cellBtn.Length == 0) ? roomList.Count / cellBtn.Length : (roomList.Count / cellBtn.Length) + 1;
        previousBtn.interactable = (currentPage <= 1) ? false : true;
        nextBtn.interactable = (currentPage >= maxPage) ? false : true;

        multiple = (currentPage - 1) * cellBtn.Length;
        for (int i = 0; i < cellBtn.Length; i++)
        {
            cellBtn[i].interactable = (multiple + i < roomList.Count) ? true : false;
            cellBtn[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (multiple + i < roomList.Count) ? roomList[multiple +i].Name : "";
            cellBtn[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = (multiple + i < roomList.Count) ? "Waiting" : "FULL";

            if(cellBtn[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text == "Waiting")
            {
                cellBtn[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.HSVToRGB(41, 190, 0);
            }
            else if (cellBtn[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text == "FULL")
            {
                cellBtn[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.HSVToRGB(212, 59, 19);
            }
        }
    } 

    public void BtnClick(int num)
    {
        if (num == -2) --currentPage;
        else if (num == -1) ++currentPage;
        else PhotonNetwork.JoinRoom(roomList[multiple + num].Name);

        RoomListRenewal();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (this.roomList.Contains(roomList[i]))
                    this.roomList.Add(roomList[i]);
                else
                    this.roomList[this.roomList.IndexOf(roomList[i])] = roomList[i];
            }
            else if (this.roomList.IndexOf(roomList[i]) != -1)
                this.roomList.RemoveAt(this.roomList.IndexOf(roomList[i]));
        }
        RoomListRenewal();
    }
}
