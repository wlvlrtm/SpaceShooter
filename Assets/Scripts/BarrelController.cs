using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelController : MonoBehaviour {
    [SerializeField] private GameObject expEffect;
    
    private Transform tr;
    private Rigidbody rb;
    private int hitCount = 0;


    private void Start() {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        
    }

    private void OnCollisionEnter(Collision other) {
        if (other.collider.CompareTag("Bullet")) {
            if (++hitCount == 3) {
                ExpBarrel();
            }
        }
    }

    private void ExpBarrel() {
        GameObject exp = Instantiate(expEffect, tr.position, Quaternion.identity);
        
        Destroy(exp, 5.0f);
        
        rb.mass = 1.0f;
        rb.AddForce(Vector3.up * 1500.0f);

        Destroy(gameObject, 3.0f);
        
        
    }

}
