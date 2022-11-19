using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chick : AnimalBase
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
            transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("chickOne");
        else
            transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("chickTwo");
    }

    public override bool Move()
    {
        if (!isEvolve)
        {
            parentCell = transform.parent.GetComponent<Cell>();
            if (player == Player.player_one)
            {
                if ((parentCell.y == nextCell.y - 1 && nextCell.y >= 0) && parentCell.x == nextCell.x)
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
                if ((parentCell.y == nextCell.y + 1 && nextCell.y <= 3) && parentCell.x == nextCell.x)
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

        else
        {
            parentCell = transform.parent.GetComponent<Cell>();
            if (player == Player.player_one) //양옆/ 앞대각선// 
            {
                if ((nextCell.x >= 0 && nextCell.x <= 2 && nextCell.y >= 0 && nextCell.y <= 3))
                {
                    if (parentCell.y == nextCell.y && (parentCell.x == nextCell.x - 1 || parentCell.x == nextCell.x + 1))
                    {
                        return true;
                    }
                    else if (parentCell.x == nextCell.x && (parentCell.y == nextCell.y - 1 || parentCell.y == nextCell.y + 1))
                    {
                        return true;
                    }
                    else if (parentCell.y + 1 == nextCell.y && (parentCell.x == nextCell.x - 1 || parentCell.x == nextCell.x + 1))
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }

            else if (player == Player.player_two)
            {
                if ((nextCell.x >= 0 && nextCell.x <= 2 && nextCell.y >= 0 && nextCell.y <= 3))
                {
                    if (parentCell.y == nextCell.y && (parentCell.x == nextCell.x - 1 || parentCell.x == nextCell.x + 1))
                    {
                        return true;
                    }
                    else if (parentCell.x == nextCell.x && (parentCell.y == nextCell.y - 1 || parentCell.y == nextCell.y + 1))
                    {
                        return true;
                    }
                    else if (parentCell.y - 1 == nextCell.y && (parentCell.x == nextCell.x - 1 || parentCell.x == nextCell.x + 1))
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
    }

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

    private void Evolve(Player player)
    {
        isEvolve = true;
        if (player == Player.player_one)
        {
            transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("chickenOne");
        }
        else if (player == Player.player_two)
        {
            transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("chickenTwo");
        }
    }
}

