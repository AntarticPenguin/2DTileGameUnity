using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayerIdleState : State
{
	override public void Update()
    {
        base.Update();
        //Camera camera;
        //GameObject cameraObject = _character.transform.Find("Main Camera").gameObject;
        //camera = cameraObject.GetComponent<Camera>();

        //if(null != _character.GetTargetTileCell())
        //{
        //    _nextState = eStateType.PATHFINDING;
        //    return;
        //}

        //if (Input.GetMouseButtonDown(0))
        //{
        //    RaycastHit hit;
        //    Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        if(EventSystem.current.IsPointerOverGameObject())
        //        {
        //            Debug.Log("Clicked on the UI");
        //            return;
        //        }

        //        string filePath = "Prefabs/Effect/DamageEffect";

        //        Vector3 pos = new Vector3(hit.transform.position.x, hit.transform.position.y, 1);

        //        GameObject effectPrefab = Resources.Load<GameObject>(filePath);
        //        GameObject effectObject = GameObject.Instantiate(effectPrefab, pos, Quaternion.identity);
        //        GameObject.Destroy(effectObject, 1.0f);

        //        TileObject hitTile = hit.transform.GetComponent<TileObject>();
        //        int tileX = hitTile.GetTileX();
        //        int tileY = hitTile.GetTileY();

        //        TileCell hitCell = GameManager.Instance.GetMap().GetTileCell(tileX, tileY);
        //        _character.SetTargetTileCell(tileX, tileY);
        //    }
        //}

        //if (Input.GetKeyDown(KeyCode.F1))
        //{
        //    DataManager.Instance.SaveCharacter(_character.GetCharacterInfo());
        //    SceneManager.LoadScene("Map01");
        //}
	}
}
