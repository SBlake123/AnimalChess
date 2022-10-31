using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimalBase : MonoBehaviour
{
    public enum Player { player_one, player_two, none }

    public abstract Player player { get; set; }

    public abstract bool canMove { get;set; }
    public abstract void Move();
    public abstract void Select();
}
