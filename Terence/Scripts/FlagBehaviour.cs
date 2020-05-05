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
        if(other.CompareTag("Box")) {
            particles.Play();
            animator.SetBool("Open",true);

            GameManager.instance.NotifyVictory();
        }
    }
}
