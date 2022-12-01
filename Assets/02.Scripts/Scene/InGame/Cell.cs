using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int x;
    public int y;

    [HideInInspector]
    public int[] coord;

    private void Start()
    {
        coord = new int[]{ x , y };
    }
}
