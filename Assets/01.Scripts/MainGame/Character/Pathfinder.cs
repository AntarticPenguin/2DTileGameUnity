using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eFindMethod
{
    DISTANCE,
	SIMPLE,
	COMPLEX,
	ASTAR,
}

public class Pathfinder
{
    struct sPathCommand
    {
        public TileCell tileCell;
        public float heuristic;
    }

    List<sPathCommand> _pathfindingQueue = new List<sPathCommand>();
    Character _character;
    TileCell _targetTileCell = null;
    TileCell _reverseTileCell = null;

    public void Init(Character character)
    {
        _character = character;
        _targetTileCell = _character.GetTargetTileCell();

        //시작지점을 sPathCommand로 만들어서 큐에 삽입.
        TileCell startCell = GameManager.Instance.GetMap().GetTileCell(character.GetTileX(), character.GetTileY());
        sPathCommand command;
        command.tileCell = startCell;
        command.heuristic = 0.0f;
        PushCommand(command);
    }

    public void Reset()
    {
        GameManager.Instance.GetMap().ResetPathfinding();
        _pathfindingQueue.Clear();
    }

    public void FindPath(eFindMethod method)
    {
        while (0 != _pathfindingQueue.Count)
        {
            sPathCommand command = _pathfindingQueue[0];
            _pathfindingQueue.RemoveAt(0);

            if (false == command.tileCell.IsVisit())
            {
                command.tileCell.Visit();

                //FIND TARGET
                if (eMapType.TOWN == GameManager.Instance.GetMapType() &&
                    (_targetTileCell.GetTileX() == command.tileCell.GetTileX())
                    && (_targetTileCell.GetTileY() == command.tileCell.GetTileY()))
                {
                    _reverseTileCell = _targetTileCell;
                    return;
                }

                for (eMoveDirection direction = 0; direction <= eMoveDirection.DOWN; direction++)
                {
                    sPosition curPosition;
                    curPosition.x = command.tileCell.GetTileX();
                    curPosition.y = command.tileCell.GetTileY();
                    sPosition nextPosition = GlobalUtility.GetPositionByDirection(curPosition, direction);

                    TileMap map = GameManager.Instance.GetMap();
                    TileCell nextTileCell = map.GetTileCell(nextPosition.x, nextPosition.y);

                    if(CheckPrecondition(nextTileCell, _targetTileCell))
                    {
                        float distanceFromStart = command.tileCell.GetDistanceFromStart()
                            + command.tileCell.GetDistanceWeight();
                        float heuristic = CalcHeuristic(method, distanceFromStart,
                            command.tileCell, nextTileCell, _targetTileCell);

                        if (eMapType.DUNGEON == GameManager.Instance.GetMapType()
                            && _character.GetMoveRange() < distanceFromStart)
                            return;

                        if(null == nextTileCell.GetPrevTileCell())
                        {
                            nextTileCell.SetDistanceFromStart(distanceFromStart);
                            nextTileCell.SetPrevTileCell(command.tileCell);

                            sPathCommand newCommand;
                            newCommand.tileCell = nextTileCell;
                            newCommand.heuristic = CalcHeuristic(method, distanceFromStart, command.tileCell, nextTileCell, _targetTileCell);
                            PushCommand(newCommand);

                            //검색범위를 그려준다.
                            if (eMapType.DUNGEON == GameManager.Instance.GetMapType())
                                nextTileCell.DrawColor();
                        }
                    }
                }
            }
        }
    }

    void PushCommand(sPathCommand command)
    {
        _pathfindingQueue.Add(command);

        //sorting
        _pathfindingQueue.Sort(CompareHeuristic);
    }

    bool CheckPrecondition(TileCell nextTileCell, TileCell targetTileCell)
    {
		//if ((null != nextTileCell) && (true == nextTileCell.IsPathfindable() && false == nextTileCell.IsVisit() ||
		//    (nextTileCell.GetTileX() == _targetTileCell.GetTileX() && nextTileCell.GetTileY() == _targetTileCell.GetTileY())))
		//    return true;

		bool condition = false;

        if (false == nextTileCell.IsVisit())
            condition = true;

        if (condition)
            condition = nextTileCell.CanMove();

        if (eMapType.DUNGEON == GameManager.Instance.GetMapType())
            return condition;
		else if(eMapType.TOWN == GameManager.Instance.GetMapType())
		{
			//해당 타일이 목표 타일
			if (nextTileCell.GetTileX() == targetTileCell.GetTileX() && nextTileCell.GetTileY() == targetTileCell.GetTileY())
				condition = true;
		}
        return condition;
    }

    int CompareHeuristic(sPathCommand command1, sPathCommand command2)
    {
        return command1.heuristic.CompareTo(command2.heuristic);
    }

   public void BuildPath()
    {
        while (null != _reverseTileCell)
        {
            //경로를 그려준다
            //_reverseTileCell.DrawColor2();
            _character.PushPathTileCell(_reverseTileCell);
            _reverseTileCell = _reverseTileCell.GetPrevTileCell();
        }
    }

    #region CALCULATE HEURISTIC

    float CalcHeuristic(eFindMethod eMethod, float distanceFromStart, TileCell tileCell, TileCell nextTileCell, TileCell targetTileCell)
    {
        switch (eMethod)
        {
            case eFindMethod.DISTANCE:
                return distanceFromStart;
            case eFindMethod.SIMPLE:
                return CalcSimpleHeuristic(tileCell, nextTileCell, targetTileCell);
            case eFindMethod.COMPLEX:
                return CalcComplexHeuristic(nextTileCell, targetTileCell);
            case eFindMethod.ASTAR:
                return CalcAStarHeuristic(distanceFromStart, nextTileCell, targetTileCell);
            default:
                return 0;
        }
    }

    float CalcSimpleHeuristic(TileCell tileCell, TileCell nextTileCell, TileCell targetTileCell)
    {
        float heuristic = 0.0f;

        int diffFromCurrent = 0;
        int diffFromNext = 0;

        //x축
        {
            //현재타일 ~ 목표타일까지 거리
            diffFromCurrent = tileCell.GetTileX() - targetTileCell.GetTileX();
            diffFromCurrent = Mathf.Abs(diffFromCurrent);
            //다음타일 ~ 목표타일까지 거리
            diffFromNext = nextTileCell.GetTileX() - targetTileCell.GetTileX();
            diffFromNext = Mathf.Abs(diffFromNext);

            if (diffFromCurrent < diffFromNext)
                heuristic += 1.0f;
            else if (diffFromNext < diffFromCurrent)
                heuristic -= 1.0f;
        }

        //y축
        {
            //현재타일 ~ 목표타일까지 거리
            diffFromCurrent = tileCell.GetTileY() - targetTileCell.GetTileY();
            diffFromCurrent = Mathf.Abs(diffFromCurrent);
            //다음타일 ~ 목표타일까지 거리
            diffFromNext = nextTileCell.GetTileY() - targetTileCell.GetTileY();
            diffFromNext = Mathf.Abs(diffFromNext);

            if (diffFromCurrent < diffFromNext)
                heuristic += 1.0f;
            else if (diffFromNext < diffFromCurrent)
                heuristic -= 1.0f;
        }

        return heuristic;
    }

    float CalcComplexHeuristic(TileCell nextTileCell, TileCell targetTileCell)
    {
        int distanceW = nextTileCell.GetTileX() - targetTileCell.GetTileX();
        int distanceH = nextTileCell.GetTileY() - targetTileCell.GetTileY();

        distanceW = distanceW * distanceW;
        distanceH = distanceH * distanceH;

        return (float)(distanceW + distanceH);
    }

    float CalcAStarHeuristic(float distanceFromStart, TileCell nextTileCell, TileCell targetTileCell)
    {
        return distanceFromStart + CalcComplexHeuristic(nextTileCell, targetTileCell);
    }

    #endregion
}
