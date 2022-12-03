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
        transform.GetComponent<SpriteRenderer>().sprite = player == Player.player_one ? Resources.Load<Sprite>("catOne") : Resources.Load<Sprite>("catTwo");
    }

    public override bool Move()
    {
        bool moveResult = false;

        if (transform.parent != null)
        {
            parentCell = transform.parent.GetComponent<Cell>();

            if (CanMove.instance.BoundaryCheck(nextCell.coord))
            {
                if (CanMove.instance.MoveCheck(parentCell.coord, nextCell.coord, "Cat"))
                {
                    moveResult = true;
                } 
            }
        }
       
        return moveResult;
    }
}
