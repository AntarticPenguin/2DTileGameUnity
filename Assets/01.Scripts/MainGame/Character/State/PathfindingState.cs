using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingState : State
{
    struct sPathCommand
    {
        public TileCell tileCell;
        public TileCell prevTileCell;
    }

    Queue<sPathCommand> _pathfindingQueue = new Queue<sPathCommand>();

    override public void Start()
    {
        base.Start();

        //시작타일셀을 큐에 넣는다.
        TileMap map = GameManager.Instance.GetMap();
        map.InitPathfinding();
        //길찾기 관련 변수 초기화


        //시작지점을 sPathCommand로 만들어서 큐에 삽입.
        TileCell startCell = map.GetTileCell(_character.GetTileX(), _character.GetTileY());
        sPathCommand startCommand;
        startCommand.tileCell = startCell;
        startCommand.prevTileCell = null;

        _pathfindingQueue.Enqueue(startCommand);
    }

    override public void Update()
    {
        base.Update();

        //큐의 데이터가 비어있을 때까지 검사
        if( 0 != _pathfindingQueue.Count)
        {
            //커맨드 하나를 꺼낸다
            sPathCommand command = _pathfindingQueue.Dequeue();

            //커맨드에 포함된 타일셀이 방문되지 않은 경우에만 검사
            if(false == command.tileCell.IsVisit())
            {
                //방문 표시
                command.tileCell.Visit();

                //목표에 도달했으면 종료
                if ((_character.GetTargetTileCell().GetTileX() == command.tileCell.GetTileX())
                    && (_character.GetTargetTileCell().GetTileY() == command.tileCell.GetTileY()))
                {
                    _nextState = eStateType.MOVE;
                    return;
                }

                //4방향 검사
                for(eMoveDirection direciton = 0; direciton <= eMoveDirection.DOWN; direciton++)
                {
                    int moveX = command.tileCell.GetTileX();
                    int moveY = command.tileCell.GetTileY();

                    //각 방향별 타일셀을 도출
                    switch (direciton)
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

                    //지나갈 수 있고, 방문되지 않은 타일
                    TileMap map = GameManager.Instance.GetMap();
                    TileCell nextTileCell = map.GetTileCell(moveX, moveY);
                    if (true == nextTileCell.CanMove() && false == nextTileCell.IsVisit())
                    {
                        //거리값 계산
                        //int distanceFromStart = command.tileCell.GetDistanceFromStart() + command.tileCell.GetDistanceWeight();

                        /*새로운 커맨드를 만들어 큐에 삽입
                         *  새로운 커맨드에 이전 타일을 세팅(현재 타일이 이전 타일)
                         *      큐에 삽입
                         *      방향에 따라 찾은 타일은 거리값을 갱신
                         */
                        sPathCommand newCommand;
                        newCommand.tileCell = nextTileCell;
                        newCommand.prevTileCell = command.tileCell;
                        //newCommand.tileCell.SetDistanceFromStart(distanceFromStart);
                        _pathfindingQueue.Enqueue(newCommand);

                        if (
                            !(nextTileCell.GetTileX() == _character.GetTargetTileCell().GetTileX() && nextTileCell.GetTileY() == _character.GetTargetTileCell().GetTileY())
                            &&
                            !(nextTileCell.GetTileX() == _character.GetTileX() && nextTileCell.GetTileY() == _character.GetTileY())
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
