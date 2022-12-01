using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : AnimalBase
{
    public void Start()
    {
        ImageChange();
    }

    public override void ImageChange()
    {
        if (player == Player.player_one)
            transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("dogOne");
        else
            transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("dogTwo");
    }

    public override bool Move()
    {
        if (transform.parent != null)
        {
            parentCell = transform.parent.GetComponent<Cell>();

            if (CanMove.instance.BoundaryCheck(nextCell.coord))
            {
                if (CanMove.instance.MoveCheck(parentCell.coord, nextCell.coord, "Dog"))
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
