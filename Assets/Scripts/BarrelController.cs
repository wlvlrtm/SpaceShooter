using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelController : MonoBehaviour {
    [SerializeField] private GameObject expEffect;
    [SerializeField] private Texture[] textures;

    private new MeshRenderer renderer;
    private Transform tr;
    private Rigidbody rb;
    private int hitCount = 0;
    private float radius = 10.0f;
    private Collider[] colls = new Collider[10];

    private void Start() {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        
        renderer = GetComponentInChildren<MeshRenderer>();

        int idx = Random.Range(0, textures.Length);
        renderer.material.mainTexture = textures[idx];
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
        
        //rb.mass = 1.0f;
        //rb.AddForce(Vector3.up * 1500.0f);

        IndirectDamage(tr.position);

        Destroy(gameObject, 3.0f);
    }

    private void IndirectDamage(Vector3 pos) {
        Collider[] colls = Physics.OverlapSphere(pos, radius, 1 << 3);
        //Physics.OverlapSphereNonAlloc(pos, radius, colls, 1 << 3);

        foreach(var coll in colls) {
            rb = coll.GetComponent<Rigidbody>();
            rb.mass = 1.0f;
            rb.constraints = RigidbodyConstraints.None;
            rb.AddExplosionForce(1500.0f, pos, radius, 1200.0f);
        }
    }
}
