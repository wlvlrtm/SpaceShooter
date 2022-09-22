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
        StartCoroutine(ShowMuzzleFlash());
    }

    IEnumerator ShowMuzzleFlash() {
        Vector2 offset = new Vector2(Random.Range(0, 2), Random.Range(0, 2)) * 0.5f;
        float angle = Random.Range(0, 360);
        float scale = Random.Range(1.0f, 2.0f);

        muzzleFlash.material.mainTextureOffset = offset;                        // muzzleFlash Offset
        muzzleFlash.transform.localRotation = Quaternion.Euler(0, 0, angle);    // muzzleFlash Rotation
        muzzleFlash.transform.localScale = Vector3.one * scale;                 // muzzleFlash Size
        muzzleFlash.enabled = true;

        yield return new WaitForSeconds(0.05f);

        muzzleFlash.enabled = false;
    }
}