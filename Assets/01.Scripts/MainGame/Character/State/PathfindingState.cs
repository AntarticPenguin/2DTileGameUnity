using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingState : State
{
    enum eUpdateState
    {
        PATHFINDING,
        BUILD_PATH,
    }
    struct sPathCommand
    {
        public TileCell tileCell;
        public TileCell prevTileCell;
    }

    struct sPosition
    {
        public int x;
        public int y;
    }

    Queue<TileCell> _pathfindingQueue = new Queue<TileCell>();
    //Stack<TileCell> _pathfindingStack = new Stack<TileCell>();
    TileCell _targetTileCell;
    TileCell _reverseTileCell;

    eUpdateState _updateState;

    override public void Start()
    {
        base.Start();
        _updateState = eUpdateState.PATHFINDING;

        _targetTileCell = _character.GetTargetTileCell();

        //시작타일셀을 큐에 넣는다.
        if (null != _targetTileCell)
        {
            //길찾기 관련 변수 초기화
            GameManager.Instance.GetMap().ResetPathfinding();      //나중에 최적화: 체크가 된 부분만

            //시작지점을 sPathCommand로 만들어서 큐에 삽입.
            TileCell startCell = GameManager.Instance.GetMap().GetTileCell(_character.GetTileX(), _character.GetTileY());
            _pathfindingQueue.Enqueue(startCell);
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
        _character.ResetTargetTileCell();
    }

    override public void Update()
    {
        base.Update();

        switch (_updateState)
        {
            case eUpdateState.PATHFINDING:
                UpdatePathfinding();
                break;
            case eUpdateState.BUILD_PATH:
                UpdateBuildPath();
                break;
        }
    }

    void UpdatePathfinding()
    {
        //큐의 데이터가 비어있을 때까지 검사
        if (0 != _pathfindingQueue.Count)
        {
            //커맨드 하나를 꺼낸다
            //sPathCommand command = _pathfindingQueue.Dequeue();
            TileCell tileCell = _pathfindingQueue.Dequeue();

            //커맨드에 포함된 타일셀이 방문되지 않은 경우에만 검사
            if (false == tileCell.IsVisit())
            {
                //방문 표시
                tileCell.Visit();

                //목표에 도달했으면 종료
                if ((_targetTileCell.GetTileX() == tileCell.GetTileX())
                    && (_targetTileCell.GetTileY() == tileCell.GetTileY()))
                {
                    _updateState = eUpdateState.BUILD_PATH;
                    _reverseTileCell = _targetTileCell;
                    return;
                }
                else
                {
                    //4방향 검사
                    for (eMoveDirection direction = 0; direction <= eMoveDirection.DOWN; direction++)
                    {
                        //각 방향별 타일셀을 도출
                        sPosition curPosition;
                        curPosition.x = tileCell.GetTileX();
                        curPosition.y = tileCell.GetTileY();
                        sPosition nextPosition = GetPositionByDirection(curPosition, direction);

                        //지나갈 수 있고, 방문되지 않은 타일
                        TileMap map = GameManager.Instance.GetMap();
                        TileCell nextTileCell = map.GetTileCell(nextPosition.x, nextPosition.y);
                        if (true == nextTileCell.CanMove() && false == nextTileCell.IsVisit())
                        {
                            //거리기반
                            float distanceFromStart = tileCell.GetDistanceFromStart() + tileCell.GetDistanceWeight();

                            /*새로운 커맨드를 만들어 큐에 삽입
                             *  새로운 커맨드에 이전 타일을 세팅(현재 타일이 이전 타일)
                             *      큐에 삽입
                             *      방향에 따라 찾은 타일은 거리값을 갱신
                             */
                            nextTileCell.SetDistanceFromStart(distanceFromStart);

                            nextTileCell.SetPrevTileCell(tileCell);
                            _pathfindingQueue.Enqueue(nextTileCell);

                            if ( !(nextTileCell.GetTileX() == _targetTileCell.GetTileX()
                                && nextTileCell.GetTileY() == _targetTileCell.GetTileY())
                              )
                            {
                                //검색범위를 그려준다.
                                nextTileCell.DrawColor();
                            }
                        }
                    }
                }
            }
        }
    }

    void UpdateBuildPath()
    {
        //command 쓸 수 없는 이유
        //command에서 prevTileCell을 가져온 다음 그 이후 prevTileCell을 가져올 수 없다.
        //왜? 이전 command를 알아야 함. 그런데 이전 command를 알 수 있는 정보가 없다.

        //_reverseTileCell = command.tileCell;
        //while (null != _reverseTileCell)
        //{
        //    _pathfindingStack.Push(_reverseTileCell);
        //    _reverseTileCell = command.prevTileCell;
        //}

        if (null != _reverseTileCell)
        {
            //_pathfindingStack.Push(_reverseTileCell);
            _character.PushPathTileCell(_reverseTileCell);

            //경로를 그려준다
            if (!(_reverseTileCell.GetTileX() == _targetTileCell.GetTileX()
                    && _reverseTileCell.GetTileY() == _targetTileCell.GetTileY()))
                _reverseTileCell.DrawColor2();

            _reverseTileCell = _reverseTileCell.GetPrevTileCell();
        }
        else
        {
            _nextState = eStateType.MOVE;
        }
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
