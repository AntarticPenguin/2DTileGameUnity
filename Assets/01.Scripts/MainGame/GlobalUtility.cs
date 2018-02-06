using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//position
public struct sPosition
{
    public int x;
    public int y;
}

static class GlobalUtility
{
    public static sPosition GetPositionByDirection(sPosition position, eMoveDirection direction)
    {
        int moveX = position.x;
        int moveY = position.y;

        switch (direction)
        {
            case eMoveDirection.LEFT:
                moveX--;
                break;
            case eMoveDirection.RIGHT:
                moveX++;
                break;
            case eMoveDirection.UP:
                moveY++;
                break;
            case eMoveDirection.DOWN:
                moveY--;
                break;
        }

        sPosition newPosition;
        newPosition.x = moveX;
        newPosition.y = moveY;

        return newPosition;
    }

    public static eMoveDirection GetDirection(sPosition curPosition, sPosition nextPosition)
    {
        if (nextPosition.x > curPosition.x)
            return eMoveDirection.RIGHT;
        else if (curPosition.x > nextPosition.x)
            return eMoveDirection.LEFT;
        else if (curPosition.y > nextPosition.y)
            return eMoveDirection.UP;
        else if (nextPosition.y > curPosition.y)
            return eMoveDirection.DOWN;

        return eMoveDirection.UP;
    }
}
