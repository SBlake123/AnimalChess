using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimalBase : MonoBehaviour
{
    public int x;
    public int y;

    public Cell parentCell;
    public Cell nextCell;

    public enum Player { player_one, player_two };
    public Player player = Player.player_one;

    public abstract bool Move();
    public abstract void ImageChange();

    public bool Return()
    {
        if (player == Player.player_one)
        {
            if (nextCell.y >= 0 && nextCell.y <= 2)
                return true;
            else
                return false;
        }
        else if (player == Player.player_two)
        {
            if (nextCell.y >= 1 && nextCell.y <= 3)
                return true;
            else
                return false;
        }
        else
            return false;

    }
}
