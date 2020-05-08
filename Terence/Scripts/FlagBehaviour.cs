using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FlagBehaviour : MonoBehaviour {

    ParticleSystem particles;
    Animator animator;
    new AudioSource audio;
    bool isActivated;

    public AudioClip activationSound;

    void Start() {
        particles = GetComponent<ParticleSystem>();
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other) {

        if(isActivated) return; // Don't activate multiple times.

        CargoBehaviour cargo = other.GetComponent<CargoBehaviour>();
        if(cargo) {
            particles.Play();
            animator.SetBool("Open",isActivated = true);
            audio.PlayOneShot(activationSound);

            // Run this first to set the game state to victory.
            GameManager.instance.NotifyVictory();
            cargo.Pop();
        }
    }
}
