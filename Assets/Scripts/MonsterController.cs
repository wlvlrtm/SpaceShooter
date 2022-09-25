using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum State {
    IDLE,
    TRACE,
    ATTACK,
    DIE
}

public class MonsterController : MonoBehaviour {
    private Transform monstrerTr;
    private Transform playerTr;
    private NavMeshAgent agent;
    private Animator animator;
    private GameObject bloodEffect;

    private readonly int hashTrace = Animator.StringToHash("isTrace");
    private readonly int hashAttack = Animator.StringToHash("isAttack");
    private readonly int hashHit = Animator.StringToHash("Hit");
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");
    private readonly int hashSpeed = Animator.StringToHash("Speed");

    [SerializeField] private State state = State.IDLE;
    [SerializeField] private float traceDist = 10.0f;
    [SerializeField] private float attackDist = 1.8f;

    public bool isDie = false;


    private void Start() {
        monstrerTr = GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        bloodEffect = Resources.Load<GameObject>("_EffectExamples/Blood/Prefabs/BloodSprayEffect");

        StartCoroutine(CheckMonsterState());
        StartCoroutine(MonsterAction());
    }

    IEnumerator CheckMonsterState() {
        while(!isDie) {
            yield return new WaitForSeconds(0.3f);

            float distance = Vector3.Distance(playerTr.position, monstrerTr.position);

            if (distance <= attackDist) {
                state = State.ATTACK;
            }
            else if (distance <= traceDist) {
                state = State.TRACE;
            }
            else {
                state = State.IDLE;
            }
        }
    }

    IEnumerator MonsterAction() {
        while(!isDie) {
            switch(state) {
                case State.IDLE :
                    agent.isStopped = true;
                    animator.SetBool(hashTrace, false);
                    break;
                case State.TRACE :
                    agent.SetDestination(playerTr.position);
                    agent.isStopped = false;
                    animator.SetBool(hashTrace, true);
                    animator.SetBool(hashAttack, false);
                    break;
                case State.ATTACK :
                    animator.SetBool(hashAttack, true);
                    break;
                case State.DIE :
                    break;
            }

            yield return new WaitForSeconds(0.3f);
        }
    }


    private void ShowBloodEffect(Vector3 pos, Quaternion rot) {
        GameObject blood = Instantiate<GameObject>(bloodEffect, pos, rot, monstrerTr);
        Destroy(blood, 0.3f);
    }


    private void OnPlayerDie() {
        StopAllCoroutines();

        agent.isStopped = true;
        animator.SetTrigger(hashPlayerDie);
        animator.SetFloat(hashSpeed, Random.Range(0.8f, 1.2f));
    }


    private void OnDrawGizmos() {
        if (state == State.TRACE) {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, traceDist);
        }

        if (state == State.ATTACK) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackDist);
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.collider.CompareTag("Bullet")) {
            Destroy(other.gameObject);
            animator.SetTrigger(hashHit);
            
            Vector3 pos = other.GetContact(0).point;
            Quaternion rot = Quaternion.LookRotation(-other.GetContact(0).normal);
            ShowBloodEffect(pos, rot);
        }
    }
}