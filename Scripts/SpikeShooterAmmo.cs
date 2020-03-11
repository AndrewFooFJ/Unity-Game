using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeShooterAmmo : MonoBehaviour
{
    Rigidbody2D spikeAmmoRb;

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
}
