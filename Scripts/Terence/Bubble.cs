using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DistanceJoint2D),typeof(LineRenderer),typeof(Rigidbody2D))]
public class Bubble : MonoBehaviour {

    LineRenderer lineRenderer;
    DistanceJoint2D joint;
    new Rigidbody2D rigidbody;

    public float initialScale = 1f;
    public float minimumScale = 0.2f;
    public float buoyancyPerScaleUnit = 1.7f, buoyancyLossRate = 0.2f;
    Vector3 originalScale; // Saves the original scale as point of reference.

    // Implementation for setting the scale of the object as a whole.
    float _currentScale;
    public float currentScale {
        get { return _currentScale; }
        set {
            _currentScale = value;
            transform.localScale = originalScale * _currentScale;
            rigidbody.gravityScale = -_currentScale * buoyancyPerScaleUnit;
        }
    }

    void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        joint = GetComponent<DistanceJoint2D>();
        rigidbody = GetComponent<Rigidbody2D>();

        // Saves some scaling values.
        originalScale = transform.localScale;
        currentScale = initialScale;
    }

    void Update() {
        lineRenderer.SetPosition(0, transform.TransformPoint(joint.anchor));
        lineRenderer.SetPosition(1, joint.connectedBody.transform.TransformPoint(joint.connectedAnchor));

        if(currentScale > minimumScale) {
            currentScale -= buoyancyLossRate * Time.deltaTime;
        }
    }

    private void OnMouseDown() {
        StartCoroutine(Boost(0.3f));
    }

    // Increases the scale of the balloon over a given duration.
    public IEnumerator Boost(float scaleAmount, float duration = 0.25f) {
        float time = 0;
        WaitForEndOfFrame w = new WaitForEndOfFrame();
        while(time < duration) {
            yield return w;

            // Increases time by deltaTime, but never let time exceed duration.
            float delta = Time.deltaTime;
            time += delta;
            if(time > duration) delta -= time - duration;

            // Scale up the balloon.
            currentScale += Mathf.Lerp(0,scaleAmount,delta / duration);
        }
    }

}
