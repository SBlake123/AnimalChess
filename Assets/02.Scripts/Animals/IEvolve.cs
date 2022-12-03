using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEvolve : ILastCellArrive
{
    bool isArrive { get; set; }
}
