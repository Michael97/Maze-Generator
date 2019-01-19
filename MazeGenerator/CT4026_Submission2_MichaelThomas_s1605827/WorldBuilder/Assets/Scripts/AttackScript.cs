using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour {

    public Transform Target;
    public Vector3 Origin;

    public float Speed;

    // Update is called once per frame

    void Start()
    {
        transform.position = Origin;
    }

    void FixedUpdate()
    {
        if (Target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, Speed);

            if (transform.position == Target.transform.position)
            {
                Destroy(gameObject);
                Destroy(Target.gameObject);
            }
        }
        else if (Target == null)
            Destroy(gameObject);
    }
}
