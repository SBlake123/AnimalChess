using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chick : AnimalBase
{
    public void Start()
    {
        parent = transform.parent.gameObject;
        transform.position = transform.parent.position + new Vector3(0, 0, -0.1f);
    }

    public override bool Move()
    {
        if (parent != null)
        {
            if (player == Player.player_one)
            {
                if (parent.GetComponent<Cell>().y == nextCell.y - 1 && nextCell.y >= 0)
                {
                    return true;
                }
                else
                    return false;
            }

            else if (player == Player.player_two)
            {
                if (parent.GetComponent<Cell>().y == nextCell.y + 1 && nextCell.y <= 3)
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



