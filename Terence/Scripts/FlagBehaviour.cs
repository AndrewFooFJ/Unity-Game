using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagBehaviour : MonoBehaviour {

    ParticleSystem particles;
    Animator animator;

    void Start() {
        particles = GetComponent<ParticleSystem>();
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        CargoBehaviour cargo = other.GetComponent<CargoBehaviour>();
        if(cargo) {
            particles.Play();
            animator.SetBool("Open",true);

            // Run this first to set the game state to victory.
            GameManager.instance.NotifyVictory();
            cargo.Pop();
        }
    }
}
