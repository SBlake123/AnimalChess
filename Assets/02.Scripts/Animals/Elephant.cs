using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elephant : AnimalBase
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
            transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("elephantOne");
        else
            transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("elephantTwo");
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
                    if (parentCell.y+1 == nextCell.y && (parentCell.x == nextCell.x - 1 || parentCell.x == nextCell.x + 1))
                    {
                        return true;
                    }
                    else if (parentCell.y-1 == nextCell.y && (parentCell.x== nextCell.x - 1 || parentCell.x == nextCell.x + 1))
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


    public override void Select()
    {
        throw new System.NotImplementedException();
    }
}
