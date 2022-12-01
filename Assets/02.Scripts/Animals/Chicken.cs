using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : AnimalBase
{
    private bool isEvolve;

    public void Start()
    {
        ImageChange();
    }

    public override void ImageChange()
    {
        isEvolve = false;
        if (player == Player.player_one)
            transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("chickenOne");
        else
            transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("chickenTwo");
    }

    public override bool Move()
    {
        bool moveCheckResult = false;
        moveCheckResult = isEvolve ? SuperChickenMove() : ChickenMove();
        return moveCheckResult;
    }

    private bool ChickenMove()
    {
        parentCell = transform.parent.GetComponent<Cell>();
        if (player == Player.player_one)
        {
            Debug.Log("Move");
            if (CanMove.instance.MoveCheck(parentCell.coord, nextCell.coord, "ChickenOne"))
            {
                if (nextCell.y == 3)
                {
                    Evolve(player);
                    PhotonManager.instance.Evolve(player.ToString());
                }
                return true;
            }
            else
                return false;
        }

        else if (player == Player.player_two)
        {
            if (CanMove.instance.MoveCheck(parentCell.coord, nextCell.coord, "ChickenTwo"))
            {
                if (nextCell.y == 0)
                {
                    Evolve(player);
                    PhotonManager.instance.Evolve(player.ToString());
                }
                return true;
            }
            else
                return false;
        }
        else
            return false;
    }

    private bool SuperChickenMove()
    {
        parentCell = transform.parent.GetComponent<Cell>();
        if (player == Player.player_one) //양옆/ 앞대각선// 
        {
            if (CanMove.instance.BoundaryCheck(nextCell.coord))
            {
                if (CanMove.instance.MoveCheck(parentCell.coord, nextCell.coord, "SuperChickenOne"))
                {
                    return true;
                }
                return false;
            }
            else
                return false;
        }

        else if (player == Player.player_two)
        {
            if (CanMove.instance.BoundaryCheck(nextCell.coord))
            {
                if (CanMove.instance.MoveCheck(parentCell.coord, nextCell.coord, "SuperChickenTwo"))
                {
                    return true;
                }
                return false;
            }
            else
                return false;
        }
        else
            return false;
    }

    private void Evolve(Player player)
    {
        isEvolve = true;
        if (player == Player.player_one)
        {
            transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("superChickenOne");
        }
        else if (player == Player.player_two)
        {
            transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("superChickenTwo");
        }
    }

    #region RPC

    public Player StringToEnum(string str)
    {
        return (Player)Enum.Parse(typeof(Player), str);
    }

    public void DecideEvolve(string player)
    {
        if (this.transform.tag == "ANIMAL" && this.player.ToString() == player.ToString())
        {
            Player me = StringToEnum(player);
            Evolve(me);
        }
    }

    #endregion
}

