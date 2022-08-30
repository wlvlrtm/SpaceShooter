using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Transform tr;
    private Animation anim;
    [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] private float turnSpeed = 80.0f;


    private void Init() {
        tr = GetComponent<Transform>();
        anim = GetComponent<Animation>();
    }

    void Awake() {
        Init();
    }

    void Start() {
        // Idle 애니메이션 실행
        anim.Play("Idle");
    }

    void Update() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float mouseX = Input.GetAxis("Mouse X");
        
        // 전후좌우 이동 방향 벡터 계산
        Vector3 moveDir = (Vector3.forward * vertical) + (Vector3.right * horizontal);

        // Translate(이동 방향 * 속력 * Time.deltaTime)
        tr.Translate(moveDir.normalized * moveSpeed * Time.deltaTime);

        // Vector3.up 축을 기준으로 turnSpeed만큼의 속도로 회전
        tr.Rotate(Vector3.up * turnSpeed * Time.deltaTime * mouseX);
        
        // 주인공 캐릭터의 애니메이션 설정
        PlayerAnim(horizontal, vertical);
    }

    void PlayerAnim(float horizontal, float vertical) {
        // 키보드 입력값을 기준으로 동작할 애니메이션 수행
        if (vertical >= 0.1f) {
            anim.CrossFade("RunF", 0.25f);  // 전진 애니메이션 실행
        }
        else if (vertical <= -0.1f) {
            anim.CrossFade("RunB", 0.25f);  // 후진 애니메이션 실행
        }
        else if (horizontal >= 0.1f) {
            anim.CrossFade("RunR", 0.25f);  // 오른쪽 이동 애니메이션 실행
        }
        else if (horizontal <= -0.1f) {
            anim.CrossFade("RunL", 0.25f);  // 왼쪽 이동 애니메이션 실행
        }
        else {
            anim.CrossFade("Idle", 0.25f);   // 정지 시 Idle 애니메이션 실행
        }
    }
}