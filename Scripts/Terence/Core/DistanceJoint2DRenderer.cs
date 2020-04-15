using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DistanceJoint2D),typeof(LineRenderer))]
public class DistanceJoint2DRenderer : MonoBehaviour {

    LineRenderer lineRenderer;
    DistanceJoint2D joint;

    void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        joint = GetComponent<DistanceJoint2D>();
    }

    void Update() {
        lineRenderer.SetPosition(0, transform.TransformPoint(joint.anchor));
        lineRenderer.SetPosition(1, joint.connectedBody.transform.TransformPoint(joint.connectedAnchor));
    }

}
