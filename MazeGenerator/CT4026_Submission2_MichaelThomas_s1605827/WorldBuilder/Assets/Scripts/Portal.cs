using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Portal : MonoBehaviour {

    public GameObject otherPortal;

    

    public bool canTeleport;

	// Use this for initialization
	void Start () {
        canTeleport = false;

        GetOtherPortal();
	}

    void GetOtherPortal()
    {
        GameObject[] portals = GameObject.FindGameObjectsWithTag("Portal");

        foreach (GameObject portal in portals)
        {
            if (portal == gameObject)
                continue;

            otherPortal = portal;
        }

        if (otherPortal == gameObject)
            otherPortal = GameObject.FindGameObjectWithTag("Portal");

    }

    void Teleport(GameObject player)
    {
        player.GetComponent<NavMeshAgent>().enabled = false;
        player.transform.position = otherPortal.transform.position + new Vector3(0.5f, 0, 0);
        player.GetComponent<NavMeshAgent>().enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (otherPortal == null)
            GetOtherPortal();

        if (other.gameObject.tag == "Player")
        {
            if (canTeleport)
                Teleport(other.gameObject);
        }
    }
}
