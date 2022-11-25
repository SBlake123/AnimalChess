using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : AnimalBase
{
    public void Start()
    {
        ImageChange();
    }

    public override void ImageChange()
    {
        if (player == Player.player_one)
            transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("elephantOne");
        else
            transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("elephantTwo");
    }

    public override bool Move()
    {
        if (transform.parent != null)
        {
            parentCell = transform.parent.GetComponent<Cell>();
            if (player == Player.player_one)
            {
                if (nextCell.x >= 0 && nextCell.x <= 2 && nextCell.y >= 0 && nextCell.y <= 3)
                {
                    if (parentCell.y + 1 == nextCell.y && (parentCell.x == nextCell.x - 1 || parentCell.x == nextCell.x + 1))
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
                    if (parentCell.y + 1 == nextCell.y && (parentCell.x == nextCell.x - 1 || parentCell.x == nextCell.x + 1))
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
}
