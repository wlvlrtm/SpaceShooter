using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingCamera : MonoBehaviour {
    [SerializeField] private Transform target;
    [SerializeField] private float damping = 10.0f;
    [SerializeField] [Range(2.0f, 20.0f)] private float distance = 10.0f;
    [SerializeField] [Range(0.0f, 10.0f)] private float height = 2.0f;
    private Transform camera;
    private Vector3 velocity = Vector3.zero;

    private void Awake() {
        this.camera = GetComponent<Transform>();
    }

    private void LateUpdate() {
        Vector3 targetPos = this.target.position + (-this.target.forward * this.distance) + (Vector3.up * this.height);
        
        // Slerp (Camera <-> Target)
        this.camera.position = Vector3.SmoothDamp(this.camera.position, targetPos, ref this.velocity, this.damping);
        
        // LookAt (Target)
        this.camera.LookAt(this.target.position);        
    }

}
