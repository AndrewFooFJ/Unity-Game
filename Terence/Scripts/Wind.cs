using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour {

    ParticleSystem particles;
    new BoxCollider2D collider;
    public float force = 1f;

    // Start is called before the first frame update
    void Start() {
        particles = GetComponent<ParticleSystem>();

        // Initialises collider.
        collider = gameObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        UpdateCollider();
    }

    // Update is called once per frame
    void Update() {
        
    }

    void UpdateCollider() {
        ParticleSystem.MainModule main = particles.main;
        float startLifetime = 0f;
        switch(main.startLifetime.mode) {
            case ParticleSystemCurveMode.Constant:
                startLifetime = main.startLifetime.constant;
                break;
            case ParticleSystemCurveMode.TwoConstants:
                startLifetime = main.startLifetime.constantMax;
                break;
            case ParticleSystemCurveMode.Curve:
            case ParticleSystemCurveMode.TwoCurves:
                startLifetime = main.startLifetime.Evaluate(particles.time / main.duration);
                break;
        }

        float startSpeed = 0f;
        switch(main.startSpeed.mode) {
            case ParticleSystemCurveMode.Constant:
                startSpeed = main.startSpeed.constant;
                break;
            case ParticleSystemCurveMode.TwoConstants:
                startSpeed = main.startSpeed.constantMax;
                break;
            case ParticleSystemCurveMode.Curve:
            case ParticleSystemCurveMode.TwoCurves:
                startSpeed = main.startSpeed.Evaluate(particles.time / main.duration);
                break;
        }

        collider.size = new Vector2(startSpeed * startLifetime, particles.shape.radius * 2f);
        collider.offset = new Vector2(collider.size.x * 0.5f, 0);
    }

    void OnTriggerStay2D(Collider2D other) {
        other.attachedRigidbody.AddForce(Vector2.right * force);
    }
}
