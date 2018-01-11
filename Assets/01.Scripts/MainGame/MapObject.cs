using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{

    //Unity Functions

	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}



    public void SetPosition(Vector2 position)
    {
        gameObject.transform.localPosition = position;
    }


    //Sorting

    virtual public void SetSortingOrder(int sortingID, int sortingOrder)
    {
        gameObject.GetComponent<SpriteRenderer>().sortingLayerID = sortingID;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
    }
}
