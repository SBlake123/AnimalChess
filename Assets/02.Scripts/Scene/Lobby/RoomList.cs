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
            cellBtn[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = (multiple + i < roomList.Count) ? "Waiting" : "";

            if(cellBtn[i].interactable)
            {
                int k = i;
                if (roomList[k].PlayerCount == 2)
                {
                    Debug.Log("FULL");
                    cellBtn[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "FULL";
                }

                if(roomList[k].IsOpen == false)
                {
                    cellBtn[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Playing";
                    cellBtn[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = new Color(212 / 255f, 59 / 255f, 19 / 255f, 255 / 255f);
                }
            }


            if(cellBtn[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text == "Waiting")
            {
                Debug.Log("Waiting Color Change");
                cellBtn[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = new Color(41 / 255f, 190 / 255f, 0 / 255f, 255 / 255f);
            }
            else if (cellBtn[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text == "FULL")
            {
                cellBtn[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = new Color(212 / 255f, 59 / 255f, 19 / 255f, 255 / 255f);
            }
        }
    } 

    public void BtnClick(int num)
    {
        if (num == -2) --currentPage;
        else if (num == -1) ++currentPage;
        else
        {
            Debug.Log("Room Click");
            PhotonNetwork.JoinRoom(roomList[multiple + num].Name);

        }

        RoomListRenewal();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("RoomListUpdate");
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (this.roomList.Contains(roomList[i]))
                    continue;
                else
                    this.roomList.Add(roomList[i]);
            }
            else if (this.roomList.IndexOf(roomList[i]) != -1)
                this.roomList.RemoveAt(this.roomList.IndexOf(roomList[i]));
        }
        RoomListRenewal();
    }
}
