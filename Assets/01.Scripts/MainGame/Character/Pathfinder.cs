using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eFindMode
{
	NONE,
    VIEW_MOVERANGE,
	VIEW_ATTACKRANGE,
    FIND_PATH,
}

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
	int _range = 0;

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

        for (int i = 0; i < _rangeTile.Count; i++)
            _rangeTile[i].ClearColor();
        _rangeTile.Clear();
    }

	public void SetRange(int range)
	{
		_range = range;
	}

	public void FindPath(eFindMode mode, eFindMethod method)
    {
		bool isViewRange = (eFindMode.VIEW_MOVERANGE == mode || eFindMode.VIEW_ATTACKRANGE == mode);

        while (0 != _pathfindingQueue.Count)
        {
            sPathCommand command = _pathfindingQueue[0];
            _pathfindingQueue.RemoveAt(0);

            if (false == command.tileCell.IsVisit())
            {
                command.tileCell.Visit();

                //FIND TARGET
                if(eFindMode.FIND_PATH == mode)
                {
                    if ((_targetTileCell.GetTileX() == command.tileCell.GetTileX())
                        && (_targetTileCell.GetTileY() == command.tileCell.GetTileY()))
                    {
                        _reverseTileCell = _targetTileCell;
                        return;
                    }
                }


                for (eMoveDirection direction = 0; direction <= eMoveDirection.DOWN; direction++)
                {
                    sPosition curPosition;
                    curPosition.x = command.tileCell.GetTileX();
                    curPosition.y = command.tileCell.GetTileY();
                    sPosition nextPosition = GlobalUtility.GetPositionByDirection(curPosition, direction);

                    TileMap map = GameManager.Instance.GetMap();
                    TileCell nextTileCell = map.GetTileCell(nextPosition.x, nextPosition.y);

                    if(CheckPrecondition(mode, nextTileCell, _targetTileCell))
                    {
                        float distanceFromStart = command.tileCell.GetDistanceFromStart()
                            + command.tileCell.GetDistanceWeight();
                        float heuristic = CalcHeuristic(method, distanceFromStart,
                            command.tileCell, nextTileCell, _targetTileCell);

                        if (isViewRange && (_range < distanceFromStart))
                            return;

                        if(null == nextTileCell.GetPrevTileCell())
                        {
                            nextTileCell.SetDistanceFromStart(distanceFromStart);
                            nextTileCell.SetPrevTileCell(command.tileCell);

                            sPathCommand newCommand;
                            newCommand.tileCell = nextTileCell;
							newCommand.heuristic = heuristic;
							PushCommand(newCommand);

                            //검색범위를 그려준다.
                            if (isViewRange)
                                DrawSearchTile(nextTileCell);
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

    bool CheckPrecondition(eFindMode mode, TileCell nextTileCell, TileCell targetTileCell)
    {
		//if ((null != nextTileCell) && (true == nextTileCell.IsPathfindable() && false == nextTileCell.IsVisit() ||
		//    (nextTileCell.GetTileX() == _targetTileCell.GetTileX() && nextTileCell.GetTileY() == _targetTileCell.GetTileY())))
		//    return true;

		bool condition = false;

        if (false == nextTileCell.IsVisit())
            condition = true;

		if(eFindMode.VIEW_MOVERANGE == mode || eFindMode.FIND_PATH == mode)
		{
			if (condition)
				condition = nextTileCell.CanMove();
		}

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
        //경로를 도출
        while (null != _reverseTileCell)
        {
            _character.PushPathTileCell(_reverseTileCell);
            _reverseTileCell = _reverseTileCell.GetPrevTileCell();
        }
    }

    List<TileCell> _rangeTile = new List<TileCell>();

    void DrawSearchTile(TileCell tileCell)
    {
        tileCell.DrawColor();
        _rangeTile.Add(tileCell);
    }

    public bool CheckRange(TileCell tileCell)
    {
        for(int i = 0; i < _rangeTile.Count; i++)
        {
            if (_rangeTile[i] == tileCell)
                return true;
        }
        return false;
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
