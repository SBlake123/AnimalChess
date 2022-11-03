using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lion : AnimalBase
{
    public void Start()
    {
        ImageChange();
        parent = transform.parent.gameObject;
        transform.position = transform.parent.position + new Vector3(0, 0, -0.1f);
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
        if (parent != null)
        {
            Cell parentCell = parent.GetComponent<Cell>();
            if (player == Player.player_one)
            {
                if (nextCell.x >= 0 && nextCell.x <= 2 && nextCell.y >= 0 && nextCell.y <= 3)
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

            else if (player == Player.player_two)
            {
                if (nextCell.x >= 0 && nextCell.x <= 2 && nextCell.y >= 0 && nextCell.y <= 3)
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
                    WinManager.instance.InvadeSuccess("player_one");
            }
            else if (player == Player.player_two)
            {
                if (nextCell.y == 0)
                    WinManager.instance.InvadeSuccess("player_two");
            }
        }
    }
}
