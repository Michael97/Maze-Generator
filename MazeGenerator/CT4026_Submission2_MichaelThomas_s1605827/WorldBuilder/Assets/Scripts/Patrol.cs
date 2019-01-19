using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour {

    public GameObject player;
    private float time = 5.0f;
    public float walkRadius;

	// Use this for initialization
	void Start () {
        //After 2 seconds, call NewPosition() every 0.2 secs
        InvokeRepeating("NewPosition", 2.0f, time);
	}

    void NewPosition()
    {
        //chases the player if within distance
        if (Vector3.Distance(player.transform.position, transform.position) < 30.0f)
        {
            time = 0.05f;
            GetComponent<NavMeshAgent>().destination = player.transform.position;
            Debug.Log("Chasing");
        }
        //wanders
        else
        {
            time = 5.0f;
            Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
            Debug.Log("not chasing");
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
            GetComponent<NavMeshAgent>().destination = hit.position;
        }
    }
}
