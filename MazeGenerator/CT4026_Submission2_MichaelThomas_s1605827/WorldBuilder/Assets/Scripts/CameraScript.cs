using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public GameObject PlayerPosition;

	// Update is called once per frame
	void Update () {
        GetComponent<Camera>().gameObject.transform.position = new Vector3(PlayerPosition.transform.position.x, gameObject.transform.position.y, PlayerPosition.transform.position.z);

    }
}
