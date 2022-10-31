using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chick : AnimalBase
{
    public override bool canMove { get; set; }
    public override Player player { get; set; } = Player.player_one;

    public override void Move()
    {
        throw new System.NotImplementedException();
    }

    public override void Select()
    {
        throw new System.NotImplementedException();
    }
}
