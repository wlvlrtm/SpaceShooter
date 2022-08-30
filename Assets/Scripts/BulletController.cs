using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {
    [SerializeField] private float damage = 20.0f;
    [SerializeField] private float force = 1500.0f;
    private Rigidbody rb;


    private void Init() {
        rb = GetComponent<Rigidbody>();
    }

    void Awake() {
        Init();
    }

    void Start() {
        this.rb.AddRelativeForce(Vector3.forward * this.force);
    }
}
