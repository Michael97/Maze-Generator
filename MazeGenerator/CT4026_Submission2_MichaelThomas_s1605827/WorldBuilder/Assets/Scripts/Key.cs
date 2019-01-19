using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

    public GameObject portal;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            portal.GetComponent<Portal>().canTeleport = true;
            Destroy(this.gameObject);
        }
    }

}
