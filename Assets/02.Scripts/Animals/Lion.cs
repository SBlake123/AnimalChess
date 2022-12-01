using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lion : AnimalBase
{
    public void Start()
    {
        ImageChange();
    }

    public override void ImageChange()
    {
        if (player == Player.player_one)
            transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("LionOne");
        else
            transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("LionTwo");
    }

    public void Update()
    {
        LionInvade();
    }

    public override bool Move()
    {
        if (transform.parent != null)
        {
            parentCell = transform.parent.GetComponent<Cell>();

            if (CanMove.instance.BoundaryCheck(nextCell.coord))
            {
                if (CanMove.instance.MoveCheck(parentCell.coord, nextCell.coord, "Lion"))
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
        else
            return false;
    }

    public void LionInvade()
    {
        if (nextCell != null)
        {
            if (player == Player.player_one)
            {
                if (nextCell.y == 3)
                {
                    WinManager.instance.InvadeSuccess("player_one");
                    PhotonManager.instance.InvadeSuccess("player_one");
                }

            }
            else if (player == Player.player_two)
            {
                if (nextCell.y == 0)
                {
                    WinManager.instance.InvadeSuccess("player_two");
                    PhotonManager.instance.InvadeSuccess("player_two");
                }
            }
        }
    }
}
