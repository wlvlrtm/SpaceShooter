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
    private int hp = 100;

    private readonly int hashTrace = Animator.StringToHash("isTrace");
    private readonly int hashAttack = Animator.StringToHash("isAttack");
    private readonly int hashHit = Animator.StringToHash("Hit");
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashDie = Animator.StringToHash("Die");

    [SerializeField] private State state = State.IDLE;
    [SerializeField] private float traceDist = 10.0f;
    [SerializeField] private float attackDist = 1.8f;

    public bool isDie = false;


    private void OnEnable() {
        PlayerController.OnPlayerDie += this.OnPlayerDie;
        StartCoroutine(CheckMonsterState());    
        StartCoroutine(MonsterAction());
    }


    private void OnDisable() {
        PlayerController.OnPlayerDie -= this.OnPlayerDie;
    }


    private void Awake() {
        monstrerTr = GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        animator = GetComponent<Animator>();
        bloodEffect = Resources.Load<GameObject>("_EffectExamples/Blood/Prefabs/BloodSprayEffect");
    }

    private void Update() {
        if (agent.remainingDistance >= 2.0f) {
            Vector3 direction = agent.desiredVelocity;
            Quaternion rot = Quaternion.LookRotation(direction);
            monstrerTr.rotation = Quaternion.Slerp(monstrerTr.rotation, rot, Time.deltaTime * 10.0f);
        }
    }


    IEnumerator CheckMonsterState() {
        while(!isDie) {
            yield return new WaitForSeconds(0.3f);

            if (state == State.DIE) {
                yield break;
            }

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
                    isDie = true;
                    agent.isStopped = true;
                    animator.SetTrigger(hashDie);
                    GetComponent<CapsuleCollider>().enabled = false;
                    yield return new WaitForSeconds(3.0f);
                    hp = 100;
                    isDie = false;
                    state = State.IDLE;
                    GetComponent<CapsuleCollider>().enabled = true;
                    this.gameObject.SetActive(false);
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
        }
    }

    public void OnDamage(Vector3 pos, Vector3 normal) {
        animator.SetTrigger(hashHit);
        Quaternion rot = Quaternion.LookRotation(normal);
        ShowBloodEffect(pos, rot);

        this.hp -= 10;

        if (this.hp <= 0) {
            state = State.DIE;
            GameManager.instance.DisplayScore(50);
        }
    }
}