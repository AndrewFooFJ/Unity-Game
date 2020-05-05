using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(DistanceJoint2D),typeof(LineRenderer),typeof(Rigidbody2D))]
public class BalloonBehaviour : MonoBehaviour {

    LineRenderer lineRenderer;
    DistanceJoint2D joint;
    new Rigidbody2D rigidbody;
    new SpriteRenderer renderer;
    new AudioSource audio;

    // Private variables for storing some attributes.
    new Collider2D collider;
    Vector2 originalBounds;
    Color originalColor;
    
    public Camera targetCamera;

    [Header("Controls")]
    public float doubleClickThreshold = 0.2f;
    public bool fullscreenClick = false;

    [Header("Buoyancy")]
    public float volume;
    public float minimumVolume = 0.24f, maximumVolume = 2f;
    public float volumeLossRate = 0.03f, volumeGainRate = 0.1f;
    public float boostVolume = 1.4f, boostNudgeForce = 1f;

    public enum Mode { boost, gradual }
    public Mode controlMode;
    public bool stopLossWhenInflating = true;

    bool isInflating = false;
    protected bool isDead = false;

    // Nested class for storing the last click on balloon.
    public class Click {
        public float startTime, duration = 0f;
        public bool isActive = true;
    }
    List<Click> clicks = new List<Click>(2); // Record of clicks made.
    const int CLICK_LIST_LENGTH = 2;

    [Header("Aesthetics")]
    public GameObject popEffect;
    public AudioClip popSound, inflateSound;

    void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        joint = GetComponent<DistanceJoint2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        audio = GetComponent<AudioSource>();

        collider = GetComponentInChildren<Collider2D>();
        if(volume <= 0) volume = CalculateVolume();
        else UpdateVolume();

        // Auto-populate camera if it is missing.
        targetCamera = targetCamera ?? Camera.main;
        originalColor = renderer.color;
    }

    void Update() {
        lineRenderer.SetPosition(0, transform.TransformPoint(joint.anchor));
        lineRenderer.SetPosition(1, joint.connectedBody.transform.TransformPoint(joint.connectedAnchor));

        // Reduce the volume every frame by the volume loss rate.
        if(volume > minimumVolume && !(isInflating && stopLossWhenInflating))
            volume = Mathf.Max(minimumVolume, volume - Mathf.Max(0,volumeLossRate) * Time.deltaTime);

        // If it is supposed to be inflating, then inflate it.
        if(isInflating && controlMode == Mode.gradual)
            volume += volumeGainRate * Time.deltaTime;

        UpdateVolume();

        HandleInput();
    }

    void HandleInput() {
        if(Input.GetMouseButtonDown(0)) {

            // Limit length of clicks.
            if(clicks.Count == CLICK_LIST_LENGTH) clicks.RemoveAt(0);
            clicks.Add(new Click { startTime = Time.time });

            // Handles left click down.
            if(IsOver(Input.mousePosition)) {

                // Burst the balloon if it is a double click.
                if(IsDoubleClick()) Death();
                else {
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
            }
        } else if(Input.GetMouseButtonUp(0)) {

            clicks[clicks.Count - 1].isActive = false;

            // Left click up.
            switch(controlMode) {
                case Mode.gradual:
                    isInflating = false;
                    break;
            }
        }

        // Update the current click duration if any.
        if(clicks.Count > 0) {
            clicks[clicks.Count - 1].duration += Time.deltaTime;
        }
    }

    // Check the clicks array to see if we have a double click.
    bool IsDoubleClick() {
        int count = clicks.Count;
        if(count > 1) {
            if(clicks[count-1].startTime + clicks[count-1].duration - clicks[count-2].startTime <= doubleClickThreshold)
                return true;
        }
        return false;
    }

    // Is our balloon over a particular point on the screen?
    bool IsOver(Vector2 screenPoint) {

        if(fullscreenClick) return true;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition),
                point = collider.ClosestPoint(mousePos);
        
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

    // What happens when the bubble pops.
    public void Death() {
        if(isDead) return;

        isDead = true;
        Destroy(gameObject,popSound.length);
        if(popEffect) Instantiate(popEffect,transform.position,transform.rotation);
        if(popSound) {
            audio.PlayOneShot(popSound);
            Destroy(gameObject,popSound.length);
            renderer.enabled = false;
            rigidbody.simulated = false;
            collider.enabled = false;
            return;
        }
        Destroy(gameObject);
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
            Death();
        } else {
            float threshold = maximumVolume - boostVolume * 2;
            if(volume > threshold) {
                Color danger = (originalColor + Color.red) / 2f;
                renderer.color = Color.Lerp(originalColor, danger, Mathf.InverseLerp(threshold,maximumVolume,volume));
            } else {
                renderer.color = originalColor;
            }
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

        // Add the nudge force too.
        rigidbody.AddForce(Vector2.up * boostNudgeForce, ForceMode2D.Impulse);
        
        // Play inflation sound.
        if(inflateSound) audio.PlayOneShot(inflateSound);

        isInflating = true; // Set flag to stop volume loss.

        while(time < duration) {
            yield return w;

            // Increases time by deltaTime, but never let time exceed duration.
            float delta = Time.deltaTime;
            time += delta;
            if(time > duration) delta -= time - duration;

            // Scale up the balloon.
            volume += Mathf.Lerp(0,addedVolume,delta / duration);
        }

        isInflating = false; // Set flag to stop volume loss.
    }

}