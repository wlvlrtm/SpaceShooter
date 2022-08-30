using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmos : MonoBehaviour {
    [SerializeField] private Color color = Color.yellow;
    [SerializeField] private float radius = 0.1f;

    private void OnDrawGizmos() {
        Gizmos.color = this.color;
        Gizmos.DrawSphere(transform.position, this.radius);
    }

}
