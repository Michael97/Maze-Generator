using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour {

    public float AttackRange;
    public float AttackCoolDown;
    //public GameObject Target;

    public GameObject Projectile;
    private ClickToMove playerScript;

    // Use this for initialization
    void Start()
    {
        playerScript = GetComponent<ClickToMove>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AttackCoolDown -= Time.deltaTime;
    }

    public void Shoot()
    {
        if (Vector3.Distance(playerScript.Target.transform.position, transform.position) <= AttackRange)
        {
            if (AttackCoolDown <= 0.0f)
            {
                GameObject myProjectile = Instantiate(Projectile, transform.position, transform.rotation);
                myProjectile.GetComponent<AttackScript>().Target = playerScript.Target.transform;
                myProjectile.GetComponent<AttackScript>().Origin = transform.position;
                AttackCoolDown = 0.5f;
            }
        }
    }
}
