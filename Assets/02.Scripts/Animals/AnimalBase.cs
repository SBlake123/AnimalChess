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
}
