using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chick : AnimalBase
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
            transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("chickOne");
        else
            transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("chickTwo");
    }



    public override bool Move()
    {
        if (parent != null)
        {
            Cell parentCell = parent.GetComponent<Cell>();
            if (player == Player.player_one)
            {
                if ((parentCell.y == nextCell.y - 1 && nextCell.y >= 0) && parentCell.x == nextCell.x)
                {
                    return true;
                }
                else
                    return false;
            }

            else if (player == Player.player_two)
            {
                if ((parentCell.y == nextCell.y + 1 && nextCell.y <= 3)  && parentCell.x == nextCell.x)
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



    public override void Select()
    {
        throw new System.NotImplementedException();
    }
}



