using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickToMove : MonoBehaviour {

    public Camera cam;

    public GameObject Projectile;
    public GameObject Target;

    public PlayerShootScript shootScript;

    /// <summary>
    /// Code to navigate with click to move
    /// </summary>
    void Update () {
        // If the player clicked the mouse button
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit; //Create a temporary RaycastHit object

            // Then call the raycast function, casting into the scene, from where the mouse is on the 
            // main camera render. 
            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, 200)) {
                // If we hit something, then set playernavmeshagents destination to that thing, so
                // the player will attempt to move there
                GetComponent<NavMeshAgent>().destination = hit.point;

                if (hit.transform.tag == "Enemy")
                    Target = hit.transform.gameObject;

                if (canShoot())
                    shootScript.Shoot();
            }
        }
    }

    private bool canShoot()
    {
        if (Target != null)
        {
            if (Target.tag == "Enemy")
            {
                return true;
            }
            return false;
        }
        return false;
    }

}
