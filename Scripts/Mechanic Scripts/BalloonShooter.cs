using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonShooter : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletShootPos;

    public float fireRate;

    Animator shooterAnim;

    private void Start()
    {
        shooterAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (fireRate <= 0)
        {
            shooterAnim.SetTrigger("Shoot");
            fireRate = 3f; //set back to starting firerate
        } else if (fireRate > 0)
        {
            fireRate -= Time.deltaTime;
        }
    }


    public void ShootBullet()
    {
        bool shoot = false;

        Instantiate(bullet, bulletShootPos.position, bulletShootPos.rotation);
        shoot = true;
    }

}
