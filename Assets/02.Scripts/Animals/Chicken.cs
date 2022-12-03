using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : AnimalBase, IEvolve
{
    public bool isArrive { get; set; }

    public void Start()
    {
        ImageChange();
    }

    public override void ImageChange()
    {
        isArrive = false;
        transform.GetComponent<SpriteRenderer>().sprite = (player == Player.player_one) ? Resources.Load<Sprite>("chickenOne") : Resources.Load<Sprite>("chickenTwo");
    }

    public override bool Move()
    {
        bool moveCheckResult = false;

        moveCheckResult = isArrive ? SuperChickenMove() : ChickenMove();

        return moveCheckResult;
    }

    private bool ChickenMove()
    {
        bool moveResult = false;

        parentCell = transform.parent.GetComponent<Cell>();
        if (player == Player.player_one)
        {
            if (CanMove.instance.MoveCheck(parentCell.coord, nextCell.coord, "ChickenOne"))
            {
                if (nextCell.y == 3)
                {
                    LastCellArrive(player.ToString());
                    PhotonManager.instance.Evolve(player.ToString());
                }
                moveResult = true;
            }
        }

        else if (player == Player.player_two)
        {
            if (CanMove.instance.MoveCheck(parentCell.coord, nextCell.coord, "ChickenTwo"))
            {
                if (nextCell.y == 0)
                {
                    LastCellArrive(player.ToString());
                    PhotonManager.instance.Evolve(player.ToString());
                }
                moveResult = true;
            }
        }

        return moveResult;
    }

    private bool SuperChickenMove()
    {
        bool moveResult = false;

        parentCell = transform.parent.GetComponent<Cell>();

        if (player == Player.player_one)
        {
            if (CanMove.instance.BoundaryCheck(nextCell.coord))
            {
                if (CanMove.instance.MoveCheck(parentCell.coord, nextCell.coord, "SuperChickenOne"))
                {
                    moveResult = true;
                }
            }
        }

        else if (player == Player.player_two)
        {
            if (CanMove.instance.BoundaryCheck(nextCell.coord))
            {
                if (CanMove.instance.MoveCheck(parentCell.coord, nextCell.coord, "SuperChickenTwo"))
                {
                    moveResult = true;
                }
            }
        }

        return moveResult;
    }

    public void LastCellArrive(string player)
    {
        isArrive = true;
        transform.GetComponent<SpriteRenderer>().sprite = (player == Player.player_one.ToString()) ? Resources.Load<Sprite>("superChickenOne") : Resources.Load<Sprite>("superChickenTwo");
    }

    #region RPC

    public void DecideEvolve(string player)
    {
        if (this.transform.tag == "ANIMAL" && this.player.ToString() == player.ToString())
        {
            LastCellArrive(player);
        }
    }
    #endregion
}

