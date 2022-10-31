using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elephant : AnimalBase
{
    public override bool canMove { get; set; }
    public override Player player { get; set; } = Player.player_two;

    public override void Move()
    {
        throw new System.NotImplementedException();
    }

    public override void Select()
    {
        throw new System.NotImplementedException();
    }
}
