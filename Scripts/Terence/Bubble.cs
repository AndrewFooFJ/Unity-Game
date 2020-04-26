using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DistanceJoint2D),typeof(LineRenderer),typeof(Rigidbody2D))]
public class Bubble : MonoBehaviour {

    LineRenderer lineRenderer;
    DistanceJoint2D joint;
    new Rigidbody2D rigidbody;

    // For calculating volume.
    new Collider2D collider;
    Vector2 originalBounds;
    
    public Camera targetCamera;

    [Header("Buoyancy")]
    public float volume;
    public float minimumVolume = 0.24f, maximumVolume = 2f;
    public float volumeLossRate = 0.03f, volumeGainRate = 0.1f;
    public float boostVolume = 1.4f;

    public enum Mode { boost, gradual }
    public Mode controlMode;

    bool isInflating = false;

    void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        joint = GetComponent<DistanceJoint2D>();
        rigidbody = GetComponent<Rigidbody2D>();

        collider = GetComponentInChildren<Collider2D>();
        if(volume <= 0) volume = CalculateVolume();
        else UpdateVolume();

        // Auto-populate camera if it is missing.
        targetCamera = targetCamera ?? Camera.main;
    }

    void Update() {
        lineRenderer.SetPosition(0, transform.TransformPoint(joint.anchor));
        lineRenderer.SetPosition(1, joint.connectedBody.transform.TransformPoint(joint.connectedAnchor));

        // Reduce the volume every frame by the volume loss rate.
        if(volume > minimumVolume) volume = Mathf.Max(minimumVolume, volume - Mathf.Max(0,volumeLossRate) * Time.deltaTime);

        // If it is supposed to be inflating, then inflate it.
        if(isInflating) volume += volumeGainRate * Time.deltaTime;

        UpdateVolume();

        HandleInput();
    }

    void HandleInput() {
        if(Input.GetMouseButtonDown(0)) {
            // Handles left click down.
            if(IsOver(Input.mousePosition)) {
                switch(controlMode) {
                    case Mode.boost:
                        isInflating = false;
                        StartCoroutine(Boost(boostVolume));
                        break;
                    case Mode.gradual:
                        isInflating = true;
                        break;
                }
            }
        } else if(Input.GetMouseButtonUp(0)) {
            // Left click up.
            switch(controlMode) {
                case Mode.gradual:
                    isInflating = false;
                    break;
            }
        }
    }

    // Is our balloon over a particular point on the screen?
    bool IsOver(Vector2 screenPoint) {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition),
                point = collider.ClosestPoint(mousePos);
        print(mousePos + " | " + point);
        if(point == mousePos) {
            return true;
        }
        return false;
    }

    float CalculateVolume() {
        if(collider) {
            float vol = 4f * Mathf.PI * collider.bounds.size.x * collider.bounds.size.x * collider.bounds.size.y / 24f;
            return vol;
        }
        return 0;
    }

    // Updates the scale of the object based on its volume.
    public void UpdateVolume() {
        if(originalBounds.sqrMagnitude <= 0)
            originalBounds = new Vector2(
                collider.bounds.size.x / transform.localScale.x,
                collider.bounds.size.y / transform.localScale.y
            );

        // Limits the volume of the balloon.
        if(volume > maximumVolume) {
            Destroy(gameObject);
        }

        // Reverse engineers the volume equation to get the scale.
        float scale = Mathf.Pow(
            Mathf.Max(0,volume) / (4f * Mathf.PI * originalBounds.x * originalBounds.x * originalBounds.y / 24f),
            1f / 3f
        );

        transform.localScale = new Vector3(scale,scale,1f);
    }

    // Increases the scale of the balloon over a given duration.
    public IEnumerator Boost(float addedVolume, float duration = 0.25f) {
        float time = 0;
        WaitForEndOfFrame w = new WaitForEndOfFrame();
        
        while(time < duration) {
            yield return w;

            // Increases time by deltaTime, but never let time exceed duration.
            float delta = Time.deltaTime;
            time += delta;
            if(time > duration) delta -= time - duration;

            // Scale up the balloon.
            volume += Mathf.Lerp(0,addedVolume,delta / duration);
        }
    }

}