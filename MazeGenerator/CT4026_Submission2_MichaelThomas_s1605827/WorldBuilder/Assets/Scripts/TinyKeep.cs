using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinyKeep : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Vector2 a = new Vector2(3, 3);
        Vector2 b = new Vector2(4, 2);

        //inverse square law seperation or somehting
        Debug.Log((a-b).magnitude + ", " + Vector2.Distance(a, b));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void PlaceRandomRectangles()
    {

    }

    void SpreadRectangles()
    {

    }
}
