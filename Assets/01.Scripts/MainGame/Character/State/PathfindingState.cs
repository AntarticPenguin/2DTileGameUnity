using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingState : State
{
    struct sPathCommand
    {
        public TileCell tileCell;
        public float heuristic;
    }

    struct sPosition
    {
        public int x;
        public int y;
    }

    //Queue<sPathCommand> _pathfindingQueue = new Queue<sPathCommand>();
    List<sPathCommand> _pathfindingQueue = new List<sPathCommand>();
    TileCell _targetTileCell;

    override public void Start()
    {
        base.Start();

        _targetTileCell = _character.GetTargetTileCell();

        //시작타일셀을 큐에 넣는다.
        if (null != _targetTileCell)
        {
            //길찾기 관련 변수 초기화
            GameManager.Instance.GetMap().ResetPathfinding();      //나중에 최적화: 체크가 된 부분만

            //시작지점을 sPathCommand로 만들어서 큐에 삽입.
            TileCell startCell = GameManager.Instance.GetMap().GetTileCell(_character.GetTileX(), _character.GetTileY());
            sPathCommand command;
            command.tileCell = startCell;
            command.heuristic = 0.0f;
            PushCommand(command);
        }
        else
        {
            _nextState = eStateType.IDLE;
        }
    }

    public override void Stop()
    {
        base.Stop();
        _pathfindingQueue.Clear();
    }

    override public void Update()
    {
        base.Update();
        UpdatePathfinding();
    }

    void UpdatePathfinding()
    {
        //큐의 데이터가 비어있을 때까지 검사
        if (0 != _pathfindingQueue.Count)
        {
            //커맨드 하나를 꺼낸다
            sPathCommand command = _pathfindingQueue[0];
            _pathfindingQueue.RemoveAt(0);

            //커맨드에 포함된 타일셀이 방문되지 않은 경우에만 검사
            if (false == command.tileCell.IsVisit())
            {
                //방문 표시
                command.tileCell.Visit();

                //목표에 도달했으면 종료
                if ((_targetTileCell.GetTileX() == command.tileCell.GetTileX())
                    && (_targetTileCell.GetTileY() == command.tileCell.GetTileY()))
                {
                    _nextState = eStateType.BUILD_PATH;
                    return;
                }
                else
                {
                    //4방향 검사
                    for (eMoveDirection direction = 0; direction <= eMoveDirection.DOWN; direction++)
                    {
                        //각 방향별 타일셀을 도출
                        sPosition curPosition;
                        curPosition.x = command.tileCell.GetTileX();
                        curPosition.y = command.tileCell.GetTileY();
                        sPosition nextPosition = GetPositionByDirection(curPosition, direction);

                        //지나갈 수 있고, 방문되지 않은 타일
                        TileMap map = GameManager.Instance.GetMap();
                        TileCell nextTileCell = map.GetTileCell(nextPosition.x, nextPosition.y);
                        if (true == nextTileCell.CanMove() && false == nextTileCell.IsVisit())
                        {
                            //거리기반
                            float distanceFromStart = command.tileCell.GetDistanceFromStart()
                                + command.tileCell.GetDistanceWeight();

                            /*새로운 커맨드를 만들어 큐에 삽입
                             *  새로운 커맨드에 이전 타일을 세팅(현재 타일이 이전 타일)
                             *      큐에 삽입
                             *      방향에 따라 찾은 타일은 거리값을 갱신
                             */
                            
                            if(null == nextTileCell.GetPrevTileCell())
                            {
                                nextTileCell.SetDistanceFromStart(distanceFromStart);
                                nextTileCell.SetPrevTileCell(command.tileCell);

                                //검색범위를 그려준다.
                                nextTileCell.DrawColor();

                                sPathCommand newCommand;
                                newCommand.tileCell = nextTileCell;
                                newCommand.heuristic = distanceFromStart;
                                PushCommand(newCommand);
                            }
                            else
                            {
                                if (distanceFromStart < nextTileCell.GetDistanceFromStart())
                                {
                                    nextTileCell.SetDistanceFromStart(distanceFromStart);
                                    nextTileCell.SetPrevTileCell(command.tileCell);

                                    sPathCommand newCommand;
                                    newCommand.tileCell = nextTileCell;
                                    newCommand.heuristic = distanceFromStart;
                                    PushCommand(newCommand);
                                }
                            }
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
        //heuristic 값이 더 적은 것을 앞으로 오게 sorting
        //sorting 검색해서(MSDN)
        _pathfindingQueue.Sort(CompareHeuristic);
    }

    int CompareHeuristic(sPathCommand command1, sPathCommand command2)
    {
        float x = command1.heuristic;
        float y = command1.heuristic;

        return x.CompareTo(y);
    }

    //position
    sPosition GetPositionByDirection(sPosition position, eMoveDirection direction)
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
}
