using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MonoBehaviour
{

    //Unity Functions

	void Start ()
    {
		
	}

	void Update ()
    {
		
	}


    //Init

    public void Init(Sprite sprite)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void SetPosition(Vector2 position)
    {
        gameObject.transform.localPosition = position;
    }
}
