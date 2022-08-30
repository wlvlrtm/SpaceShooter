using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFire : MonoBehaviour {
    [SerializeField] private GameObject bulletObj;
    [SerializeField] private Transform firePos;


    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Fire();
        }
    }

    private void Fire() {
        Instantiate(this.bulletObj, this.firePos.position, this.firePos.rotation);
    }

}
