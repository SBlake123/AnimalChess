using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanMove : MonoBehaviour
{
    public static CanMove instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public bool BoundaryCheck(int[] nextCellCoord)
    {
        bool checkResult = (nextCellCoord[0] >= 0 && nextCellCoord[0] <= 2 && nextCellCoord[1] >= 0 && nextCellCoord[1] <= 3) ? true : false;
        return checkResult;
    }

    public bool MoveCheck(int[] parentCellCoord, int[] nextCellCoord, string animal)
    {
        BasicCoordAndBoundary basic = new BasicCoordAndBoundary();

        int[][] lionBoundary = { basic.centerTop, basic.centerBottom, basic.centerLeft, basic.centerRight, basic.leftTop, basic.leftBottom, basic.rightTop, basic.rightBottom };
        int[][] catBoundary = { basic.leftTop, basic.leftBottom, basic.rightTop, basic.rightBottom };
        int[][] dogBoundary = { basic.centerTop, basic.centerBottom, basic.centerLeft, basic.centerRight };
        int[][] chickenOneBoundary = { basic.centerRight };
        int[][] chickenTwoBoundary = { basic.centerLeft };
        int[][] superChickenOneBoundary = { basic.centerTop, basic.centerBottom, basic.centerLeft, basic.centerRight, basic.rightTop, basic.rightBottom };
        int[][] superChickenTwoBoundary = { basic.centerTop, basic.centerBottom, basic.centerLeft, basic.centerRight, basic.leftTop, basic.leftBottom };

        Dictionary<string, int[][]> nameAndBoundaryDic = new Dictionary<string, int[][]>
        {
            { "Lion",lionBoundary },
            { "Cat", catBoundary },
            { "Dog", dogBoundary },
            { "ChickenOne", chickenOneBoundary },
            { "ChickenTwo", chickenTwoBoundary },
            { "SuperChickenOne", superChickenOneBoundary },
            { "SuperChickenTwo", superChickenTwoBoundary },
        };

        bool checkResult = false;
        int[] calcCell = { (nextCellCoord[1] - parentCellCoord[1]), (nextCellCoord[0] - parentCellCoord[0]) };

        return checkResult = CompareBoundary(calcCell, nameAndBoundaryDic[animal]);
    }

    private bool CompareBoundary(int[] calcCell, int[][] animalBoundary)
    {
        bool checkResult = false;

        foreach (var item in animalBoundary)
        {
            if (calcCell[0] == item[0] && calcCell[1] == item[1])
            {
                return checkResult = true;
            }
        }
        return checkResult;
    }

    public bool ReturnCheck(string player, int[] nextCellCoord)
    {
        bool returnCheck;

        if (player == "player_one")
        {
            return returnCheck = (nextCellCoord[1] >= 0 && nextCellCoord[1] <= 2) ? true : false;
        }
        else if (player == "player_two")
        {
            return returnCheck = (nextCellCoord[1] >= 1 && nextCellCoord[1] <= 3) ? true : false;
        }
        else
            return false;
    }

}
