using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRemove : MonoBehaviour {
    [SerializeField] private GameObject sparkEffect;


    private void OnCollisionEnter(Collision other) {
        if (other.collider.CompareTag("Bullet")) {
            ContactPoint cp = other.GetContact(0);
            Quaternion rot = Quaternion.LookRotation(-cp.normal);
            GameObject spark = Instantiate(sparkEffect, cp.point, rot);

            Destroy(spark, 0.4f);
            Destroy(other.gameObject);  // Bullet Destroy
        }
    }
}
