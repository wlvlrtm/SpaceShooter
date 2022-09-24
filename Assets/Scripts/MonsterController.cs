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
    
    private readonly int hashTrace = Animator.StringToHash("isTrace");
    private readonly int hashAttack = Animator.StringToHash("isAttack");
    private readonly int hashHit = Animator.StringToHash("Hit");

    [SerializeField] private State state = State.IDLE;
    [SerializeField] private float traceDist = 10.0f;
    [SerializeField] private float attackDist = 2.0f;

    public bool isDie = false;


    private void Start() {
        monstrerTr = GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        //agent.SetDestination(playerTr.position);
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
        }
    }
}