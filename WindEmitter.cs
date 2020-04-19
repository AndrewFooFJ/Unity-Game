using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindEmitter : MonoBehaviour {

    ParticleSystem particles;
    public float width = 1f;

    // Start is called before the first frame update
    void Start() {
        particles = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void UpdateParticleSystem() {
        ParticleSystem.ShapeModule shape = particles.shape;
        shape.radius = width * 0.5f;
    }
}
