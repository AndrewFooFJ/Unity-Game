using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeShooterAmmo : MonoBehaviour
{
    Rigidbody2D spikeAmmoRb;

    public GameObject spikeDestroyEffect;

    private void Start()
    {
        spikeAmmoRb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        FlyForward();
    }

    public void FlyForward()
    {
        spikeAmmoRb.velocity = transform.right * 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            Destroy(Instantiate(spikeDestroyEffect, transform.position, Quaternion.identity), 1f);
            Destroy(this.gameObject);
        }
    }
}
