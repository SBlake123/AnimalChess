using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomList : MonoBehaviour
{
    public Button[] cellBtn;
    public Button previousBtn, nextBtn;
    public List<string> roomList;

    private int currentPage = 1, maxPage, multiple;

    private void Start()
    {
        MakeARoomList(16);
        Renewal();
    }

    private void Renewal()
    {
        maxPage = (roomList.Count % cellBtn.Length == 0) ? roomList.Count / cellBtn.Length : (roomList.Count / cellBtn.Length) + 1;
        previousBtn.interactable = (currentPage <= 1) ? false : true;
        nextBtn.interactable = (currentPage >= maxPage) ? false : true;

        multiple = (currentPage - 1) * cellBtn.Length;
        for (int i = 0; i < cellBtn.Length; i++)
        {
            cellBtn[i].interactable = (multiple + i < roomList.Count) ? true : false;
            cellBtn[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (multiple + i < roomList.Count) ? roomList[multiple + i] : "";
        }
    }

    public void BtnClick(int num)
    {
        if (num == -2) --currentPage;
        else if (num == -1) ++currentPage;
        else print(roomList[multiple + num]);

        Renewal();
    }

    private void MakeARoomList(int roomCount)
    {
        for(int i = 1; i <= roomCount; i++)
        {
            roomList.Add(i.ToString());
        }
    }
}
