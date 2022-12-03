using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lion : AnimalBase, ILastCellArrive
{
    public void Start()
    {
        ImageChange();
    }

    public override void ImageChange()
    {
        transform.GetComponent<SpriteRenderer>().sprite = (player == Player.player_one) ? Resources.Load<Sprite>("LionOne") : Resources.Load<Sprite>("LionTwo");
    }

    public override bool Move()
    {
        bool moveResult = false;

        parentCell = transform.parent.GetComponent<Cell>();

        if (player == Player.player_one)
        {
            if (CanMove.instance.MoveCheck(parentCell.coord, nextCell.coord, "Lion"))
            {
                if(nextCell.y == 3)
                {
                    LastCellArrive(player.ToString());
                }
                moveResult = true;
            }
        }

        else if (player == Player.player_two)
        {
            if (CanMove.instance.MoveCheck(parentCell.coord, nextCell.coord, "Lion"))
            {
                if (nextCell.y == 0)
                {
                    LastCellArrive(player.ToString());
                }
                moveResult = true;
            }
        }
        return moveResult;
    }

    public void LastCellArrive(string player)
    {
        WinManager.instance.InvadeSuccess(player);
        PhotonManager.instance.InvadeSuccess(player);
    }
}

