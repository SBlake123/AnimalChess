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
            transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("catOne");
        else
            transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("catTwo");
    }

    public override bool Move()
    {
        if (transform.parent != null)
        {
            parentCell = transform.parent.GetComponent<Cell>();

            if (CanMove.instance.BoundaryCheck(nextCell.coord))
            {
                if (CanMove.instance.MoveCheck(parentCell.coord, nextCell.coord, "Cat"))
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
