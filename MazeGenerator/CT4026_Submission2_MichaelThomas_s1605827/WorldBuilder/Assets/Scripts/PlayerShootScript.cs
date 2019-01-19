using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootScript : MonoBehaviour {

    public float AttackRange;
    public float AttackCoolDown;

    public GameObject Projectile;

    private ClickToMove controllerScript;

    // Use this for initialization
    void Start()
    {
        controllerScript = GetComponent<ClickToMove>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AttackCoolDown -= Time.deltaTime;
    }

    public void Shoot()
    {
        if (Vector3.Distance(controllerScript.Target.transform.position, transform.position) <= AttackRange)
        {
            if (AttackCoolDown <= 0.0f)
            {
                transform.rotation =
                    Quaternion.LookRotation(controllerScript.Target.transform.position - transform.position, Vector3.up);
                GameObject myProjectile = Instantiate(Projectile, transform.position, transform.rotation);
                myProjectile.GetComponent<AttackScript>().Target = controllerScript.Target.transform;
                myProjectile.GetComponent<AttackScript>().Origin = transform.position;
                AttackCoolDown = 0.5f;
            }
        }
    }
}
