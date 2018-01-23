﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingIdleState : State
{
	override public void Update()
    {
        base.Update();
        Camera camera;
        GameObject cameraObject = _character.transform.Find("Main Camera").gameObject;
        camera = cameraObject.GetComponent<Camera>();

        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                hit.transform.GetComponent<SpriteRenderer>().color = Color.blue;
                TileObject hitTile = hit.transform.GetComponent<TileObject>();
                int tileX = hitTile.GetTileX();
                int tileY = hitTile.GetTileY();
                Debug.Log("X: " + tileX + "Y: " + tileY);
                Debug.Log("characterX: " + _character.GetTileX() + "characterY: " + _character.GetTileY());
                _character.SetTargetTileCell(tileX, tileY);
                _nextState = eStateType.PathfindingMove;
            }
        }
	}
}
