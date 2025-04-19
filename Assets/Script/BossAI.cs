using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{

    public float maxTime = 1.0f;
    public float maxDistance = 0.5f;
    public float timer = 0.0f;
    public int damage = 10;
    [SerializeField] private Collider weapon;

    //Animator animator;
    //float speed = 0.5f;

    public Animator animator;
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public float health;
    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    
    //Attack
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    bool isAttacking = false;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake() {
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        //animator = GetComponent<Animator>();
        //speed = Animator.StringToHash("speed");
    }
    private void Update() {
        if (isAttacking) {
            return;
        }

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange , whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position , attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) {
            Patroling();
        }
        if (playerInSightRange && !playerInAttackRange) {
            ChasePlayer();
        }
        if (playerInAttackRange && playerInSightRange) {
            AttackPlayer();
        }
    } 
    private void Patroling() {
        if (!walkPointSet) {
            SearchWalkPoint();
        }
        if (walkPointSet) {
            agent.SetDestination(walkPoint);
        }
        Vector3 distnceToWalkPoint = transform.position - walkPoint;
        if (distnceToWalkPoint.magnitude < 1f) {
            walkPointSet = false;
        }

    }
    private void SearchWalkPoint() {
        float randomZ = Random.Range(-walkPointRange , walkPointRange); 
        float randomX = Random.Range(-walkPointRange , walkPointRange); 
        walkPoint = new Vector3(transform.position.x + randomX , transform.position.y , transform.position.z +randomZ);
        if (Physics.Raycast(walkPoint, transform.up, 2f , whatIsGround)) {
            walkPointSet = true;
        }
    }
    private void ChasePlayer() {
        //animator.SetFloat("Speed", 1f , 0.1f , Time.deltaTime);
        animator.SetBool("Chasing", true);
        agent.SetDestination(player.position);
    }
   private void AttackPlayer() {
        isAttacking = true;
        animator.SetBool("Chasing", false);
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked) {
            Debug.Log("Attack!!");
            alreadyAttacked = true;
            animator.Play("attack");

            // เรียก Coroutine เพื่อรอให้อนิเมชันจบก่อนรีเซ็ต
            StartCoroutine(ResetAttackAfterAnimation("attack"));
        }
    }

    private IEnumerator ResetAttackAfterAnimation(string animationName) {
        // ดึงความยาวของ animation clip จาก Animator
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        float animationLength = 1f; // default fallback

        foreach (AnimationClip clip in clips) {
            if (clip.name == animationName) {
                animationLength = clip.length;
                break;
            }
        }

        yield return new WaitForSeconds(animationLength);
        ResetAttack();
    }

    private void ResetAttack() {
        alreadyAttacked = false;
        isAttacking = false;
    }


}


