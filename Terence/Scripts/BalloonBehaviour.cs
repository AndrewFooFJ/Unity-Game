using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public CargoBehaviour cargo;

    [Header("Controls")]
    public float doubleClickThreshold = 0.2f;
    public bool fullscreenClick = false;

    [Header("Buoyancy")]
    public float volume;
    public float minimumVolume = 0.24f, maximumVolume = 2f;
    public float volumeLossRate = 0.03f, volumeGainRate = 0.1f;
    public float boostVolume = 1.4f, boostNudgeForce = 1f, density = 1.4f;

    public enum Mode { boost, gradual }
    public Mode controlMode = Mode.gradual;
    public bool stopLossWhenInflating = true, calculateMass = false;

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
    public AudioClip[] popSound, popWarnSound, boostSound, inflatingSound;

    [System.Serializable]
    public struct PullDirection {
        public Color baseColor, upColor, downColor;
    }
    public PullDirection pullDirectionUI;
    bool isPullAnimating = false;

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

        // Search for a cargo object if there are no others.
        cargo = cargo ?? FindObjectOfType<CargoBehaviour>();
    }

    void Update() {
        if(isDead) return;

        LineUpdate();

        // Reduce the volume every frame by the volume loss rate.
        if(volume > minimumVolume && !(isInflating && stopLossWhenInflating))
            volume = Mathf.Max(minimumVolume, volume - Mathf.Max(0,volumeLossRate) * Time.deltaTime);

        // If it is supposed to be inflating, then inflate it.
        if(isInflating && controlMode == Mode.gradual)
            volume += volumeGainRate * Time.deltaTime;

        UpdateVolume();

        //HandleInput();
    }

    void LineUpdate() {

        // Disable the line renderer if the cargo is destroyed.
        if(!cargo) {
            lineRenderer.enabled = false;
            joint.enabled = false;
            return;
        }

        Vector3 origin = transform.TransformPoint(joint.anchor),
                goal = joint.connectedBody.transform.TransformPoint(joint.connectedAnchor);

        float lerpAmt = 1f / (lineRenderer.positionCount - 1);
        for(int i=0;i < lineRenderer.positionCount;i++) {
            lineRenderer.SetPosition(i, Vector3.Lerp(origin,goal,lerpAmt * i));
        }

        // Show the highlight to show where the force is pulling.
        float vertical = Mathf.Abs(rigidbody.velocity.y);
        if(vertical > 0.1f)
            StartCoroutine(ShowPullFeedback(rigidbody.velocity, Mathf.Min(1.5f,1.5f - vertical / 3f)));        
    }

    // Shows feedback on whether the balloon is pulling up or down on the cargo.
    IEnumerator ShowPullFeedback(Vector2 pullDirection, float duration = 1f) {
        
        if(isPullAnimating) yield break;
        
        isPullAnimating = true;

        GradientColorKey start = new GradientColorKey(),
                         backFramer = new GradientColorKey(),
                         frontFramer = new GradientColorKey();

        // Sets the gradient properties.
        Color pullColor;
        bool pullUp = pullDirection.y > 0;
        if(pullUp) {
            start.color = pullColor = pullDirectionUI.upColor;
            backFramer.color = frontFramer.color = pullDirectionUI.baseColor;
            start.time = backFramer.time = 0.999f;
            frontFramer.time = 0.899f;
        } else {
            start.color = pullColor = pullDirectionUI.downColor;
            backFramer.color = frontFramer.color = pullDirectionUI.baseColor;
            start.time = backFramer.time = 0.001f;
            frontFramer.time = 0.101f;
        }

        // Array we are going to modify before injecting into the gradient.
        GradientColorKey[] colorKey = new GradientColorKey[5] {
            new GradientColorKey {
                color = pullDirectionUI.baseColor,
                time = 0f
            },
            backFramer,
            start,
            frontFramer,
            new GradientColorKey {
                color = pullDirectionUI.baseColor,
                time = 1f
            }
        };

        // This is the timed loop.
        WaitForEndOfFrame w = new WaitForEndOfFrame();
        float elapsed = 0;
        do {
            yield return w;
            elapsed += Time.deltaTime;
            float ratio = elapsed / duration;
            if(pullUp) ratio = 1 - ratio;

            // Updates the gradient colors on the line renderer.
            colorKey[1].time = Mathf.Clamp(ratio - 0.1f,0,1);
            colorKey[2].time = ratio;
            colorKey[3].time = Mathf.Clamp(ratio + 0.1f,0,1);
            lineRenderer.colorGradient = new Gradient() { colorKeys = colorKey };
        } while(elapsed < duration);

        // Undo the shine on the line renderer.
        isPullAnimating = false;
        lineRenderer.colorGradient = new Gradient {
            colorKeys = new GradientColorKey[1] {
                new GradientColorKey {
                    color = pullDirectionUI.baseColor,
                    time = 0f
                }
            }
        };
    }

    // For enabling / disabling inflation on balloon.
    public void Inflate(bool b) {
        if(b) {
            switch(controlMode) {
                case Mode.boost:
                    isInflating = false;
                    StartCoroutine(Boost(boostVolume));
                    break;
                case Mode.gradual:
                    isInflating = true;

                    // Play the inflating sound effect.
                    if(!audio.isPlaying && inflatingSound.Length > 0) {
                        audio.clip = inflatingSound[Random.Range(0,inflatingSound.Length)];
                        audio.loop = true;
                        audio.Play();
                    }
                    break;
            }
        } else {
            // Left click up.
            switch(controlMode) {
                case Mode.gradual:
                    isInflating = false;
                    break;
            }

            // Stop playing any audio on the balloon.
            if(inflatingSound.Contains(audio.clip) && audio.isPlaying) {
                audio.loop = false;
                audio.Stop();
            }
        }
    }

    void HandleInput() {
        // Disable input.
        if(GameManager.instance.levelState != GameManager.LevelState.inGame) return;

        if(Input.GetMouseButtonDown(0)) {

            // Limit length of clicks.
            if(clicks.Count == CLICK_LIST_LENGTH) clicks.RemoveAt(0);
            clicks.Add(new Click { startTime = Time.time });

            // Handles left click down.
            if(IsOver(Input.mousePosition)) {

                // Burst the balloon if it is a double click.
                if(IsDoubleClick()) cargo.Pop(this);
                else {
                    switch(controlMode) {
                        case Mode.boost:
                            isInflating = false;
                            StartCoroutine(Boost(boostVolume));
                            break;
                        case Mode.gradual:
                            isInflating = true;

                            // Play the inflating sound effect.
                            if(!audio.isPlaying && inflatingSound.Length > 0) {
                                audio.clip = inflatingSound[Random.Range(0,inflatingSound.Length)];
                                audio.loop = true;
                                audio.Play();
                            }
                            break;
                    }
                }
            }
        } else if(Input.GetMouseButtonUp(0)) {

            if(clicks.Count > 0)
                clicks[clicks.Count - 1].isActive = false;

            // Left click up.
            switch(controlMode) {
                case Mode.gradual:
                    isInflating = false;
                    break;
            }

            // Stop playing any audio on the balloon.
            if(inflatingSound.Contains(audio.clip) && audio.isPlaying) {
                audio.loop = false;
                audio.Stop();
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
    // Call cargo.Pop() instead of Death() to trigger game over screen.
    public void Death() {
        if(isDead) return;

        // Stop playing any audio on the balloon.
        audio.loop = false;
        audio.Stop();

        isDead = true;

        // Remove itself from the cargo list, if it has a cargo.
        if(cargo) cargo.attachedObjects.Remove(this);

        // Play death sounds and particle effects.
        if(popSound.Length > 0) {
            AudioClip popSfx = popSound[Random.Range(0,popSound.Length)];
            if(popEffect) Instantiate(popEffect,transform.position,transform.rotation);

            // Play the audio clip and disable the components.
            audio.PlayOneShot(popSfx);
            Destroy(gameObject,popSfx.length);
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
            cargo.Pop(this);
        } else {
            float threshold = maximumVolume - boostVolume * 2;
            if(volume > threshold) {
                // Play the stretched sound.
                if(renderer.color.Equals(originalColor) && popWarnSound.Length > 0) {
                    audio.PlayOneShot(popWarnSound[Random.Range(0,popWarnSound.Length)]);
                }

                Color danger = (originalColor + Color.red) / 2f;
                renderer.color = Color.Lerp(originalColor, danger, Mathf.InverseLerp(threshold,maximumVolume,volume));
            } else {
                renderer.color = originalColor;
            }
        }

        // Update the mass of the balloon's Rigidbody.
        if(calculateMass) {
            rigidbody.useAutoMass = false;
            rigidbody.mass = density * volume;
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
        if(boostSound.Length > 0) audio.PlayOneShot(boostSound[Random.Range(0,boostSound.Length)]);

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