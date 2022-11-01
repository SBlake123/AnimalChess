using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimalBase : MonoBehaviour
{
    public bool canMove;
    public GameObject parent;

    public enum Player { player_one, player_two };
    public Player player = Player.player_one;

    public abstract void Move(GameObject gameObject);
    public abstract void Select();
}
