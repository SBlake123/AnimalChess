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
        transform.GetComponent<SpriteRenderer>().sprite = (player == Player.player_one) ? Resources.Load<Sprite>("dogOne") : Resources.Load<Sprite>("dogTwo");
    }

    public override bool Move()
    {
        bool moveResult = false;

        if (transform.parent != null)
        {
            parentCell = transform.parent.GetComponent<Cell>();

            if (CanMove.instance.BoundaryCheck(nextCell.coord))
            {
                if (CanMove.instance.MoveCheck(parentCell.coord, nextCell.coord, "Dog"))
                {
                    moveResult = true;
                }
            }
        }

        return moveResult;
    }
}
