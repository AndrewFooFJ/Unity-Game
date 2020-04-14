using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DistanceJoint2D),typeof(LineRenderer))]
public class Thread : MonoBehaviour {

    LineRenderer lineRenderer;
    DistanceJoint2D joint;

    void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        joint = GetComponent<DistanceJoint2D>();
    }

    void Update() {
        Debug.DrawLine((Vector2)transform.position + joint.anchor, (Vector2)transform.position + joint.connectedAnchor, Color.red);
    }

}
