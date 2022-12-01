using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanMove : MonoBehaviour
{
    public static CanMove instance;

    public void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    static int[] centerTop = { 0, 1 };
    static int[] centerBottom = { 0, -1 };
    static int[] centerLeft = { -1, 0 };
    static int[] centerRight = { 1, 0 };
    static int[] leftTop = { -1, 1 };
    static int[] leftBottom = { -1, -1 };
    static int[] rightTop = { 1, 1 };
    static int[] rightBottom = { 1, 0 };

    int[][] lionBoundary = { centerTop, centerBottom, centerLeft, centerRight, leftTop, leftBottom, rightTop, rightBottom };
    int[][] catBoundary = { leftTop, leftBottom, rightTop, rightBottom };
    int[][] dogBoundary = { centerTop, centerBottom, centerLeft, centerRight };
    int[][] chickenOneBoundary = { centerRight };
    int[][] chickenTwoBoundary = { centerLeft };
    int[][] superChickenOneBoundary = { centerTop, centerBottom, centerLeft, centerRight, rightTop, rightBottom };
    int[][] superChickenTwoBoundary = { centerTop, centerBottom, centerLeft, centerRight, leftTop, leftBottom };

    public bool BoundaryCheck(int[] nextCellCoord)
    {
        bool checkResult = (nextCellCoord[0] >= 0 && nextCellCoord[0] <= 2 && nextCellCoord[1] >= 0 && nextCellCoord[1] <= 3) ? true : false;
        return checkResult;
    }

    public bool MoveCheck(int[] parentCellCoord, int[] nextCellCoord, string animal)
    {
        bool checkResult = false;
        int[] calcCell = { (nextCellCoord[1] - parentCellCoord[1]), (nextCellCoord[0] - parentCellCoord[0]) };
        Debug.Log(calcCell[0]);
        switch (animal)
        {
            case ("Lion"):
                checkResult = CompareBoundary(calcCell, lionBoundary);
                break;

            case ("Cat"):
                checkResult = CompareBoundary(calcCell, catBoundary);
                break;

            case ("Dog"):
                checkResult = CompareBoundary(calcCell, dogBoundary);
                break;

            case ("ChickenOne"):
                checkResult = CompareBoundary(calcCell, chickenOneBoundary);
                break;

            case ("ChickenTwo"):
                checkResult = CompareBoundary(calcCell, chickenTwoBoundary);
                break;

            case ("SuperChickenOne"):
                checkResult = CompareBoundary(calcCell, superChickenOneBoundary);
                break;

            case ("SuperChickenTwo"):
                checkResult = CompareBoundary(calcCell, superChickenTwoBoundary);
                break;
        }

        return checkResult;
    }

    public bool ReturnCheck(int[] nextCellCoord)
    {
        return false;
    }

    public bool CompareBoundary(int[] calcCell, int[][] animalBoundary)
    {

        bool checkResult = false;

        foreach (var item in animalBoundary)
        {
            if (calcCell[0] == item[0] && calcCell[1] == item[1])
            {
                checkResult = true;
                break;
            }
        }

        return checkResult;
    }
}
