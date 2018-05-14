using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State
{
	override public void Start ()
    {
        base.Start();
        _character.SetCanMove(true);
        _character.gameObject.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);

        MessageParam msg = new MessageParam();

        msg.sender = _character;
        msg.receiver = _character.WhoAttackedMe();
        msg.message = "Died";
        msg.expPoint = _character.GetExpPoint();

        MessageSystem.Instance.Send(msg);

        int itemIndex = _character.GetItemIndex();
        int tileX = _character.GetTileX();
        int tileY = _character.GetTileY();

        GameObject.Destroy(_character.gameObject);

        //Create Item
        ItemSpawner.Instance.SpawnItem(itemIndex, tileX, tileY);
    }

}
