using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FireController : MonoBehaviour {
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform firePos;
    [SerializeField] private AudioClip fireSFX;

    private new AudioSource audio;
    private MeshRenderer muzzleFlash;


    private void Start() {
        audio = GetComponent<AudioSource>();
        
        muzzleFlash = firePos.GetComponentInChildren<MeshRenderer>();
        muzzleFlash.enabled = false;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Fire();
        }
    }

    private void Fire() {
        Instantiate(bullet, firePos.position, firePos.rotation);
        audio.PlayOneShot(fireSFX, 1.0f);
    }

}