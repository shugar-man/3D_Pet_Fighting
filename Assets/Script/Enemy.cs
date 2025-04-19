using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public Transform player;
    public NavMeshAgent agent;
    public Animator anim;

    public float detectionRange = 10f;
    public float circleDistance = 1f; // Distance from player
    public float strafeSpeed = 2f;    // Speed of left-right movement
    public float strafeRange = 5f;    // How far left and right
    private int strafeSide = 0;


    private float strafeTimer = 0f;
    private bool isStrafing = false;


    private float lastAttackTime = 5f;
    public float attackCooldown = 5f; // Time between attacks
    public float attackRange = 1.5f;
    public bool isAttacking = false;

    public GameObject gameObject;
    public GameObject skill;
    public int damage = 20;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        agent.isStopped = true;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        anim.SetFloat("Distance", distanceToPlayer);


        if (distanceToPlayer < detectionRange)
        {
            // Always face the player
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
            if (distanceToPlayer > circleDistance + 0.5f) // Move toward player until within range
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
                anim.SetBool("Chasing",true);
                isStrafing = false;
                anim.SetFloat("StrafeDirection", 0);
            }
            else
            {
                if (!isAttacking) // Stop strafing when attacking
                {
                    if (!isStrafing)
                    {
                        agent.isStopped = true;
                        isStrafing = true;
                        anim.SetBool("Chasing",false);
                    }
                    StrafeAroundPlayer();
                }

                // Attack if cooldown is over
                if (Time.time > lastAttackTime + attackCooldown)
                {
                    AttackPlayer();
                    lastAttackTime = Time.time;
                }
            }

            
        }
    }


    void StrafeAroundPlayer()
    {
        if (isAttacking) return;
        strafeSide = 1;
        strafeTimer += Time.deltaTime * strafeSpeed;

        // Set animation parameter
        anim.SetFloat("StrafeDirection", strafeSide);
        if (strafeSide > 0)
        {
            anim.SetBool("WalkRight", true);
            anim.SetBool("WalkLeft", false);
        }
        else
        {
            anim.SetBool("WalkLeft", true);
            anim.SetBool("WalkRight", false);
        }
        Vector3 strafeOffset = transform.right * strafeSide * strafeRange;
        Vector3 forwardOffset = -transform.forward * circleDistance;
        Vector3 strafePosition = player.position + strafeOffset + forwardOffset;


        transform.position = Vector3.Lerp(transform.position, strafePosition, Time.deltaTime * strafeSpeed);
    }
    
    void AttackPlayer()
    {

        isAttacking = true;
        anim.ResetTrigger("Attack");


        agent.isStopped = true;
        anim.SetTrigger("Attack");
        ActivateHitbox();
        float animationTime = anim.GetCurrentAnimatorStateInfo(0).length;
        Invoke("EndAttack", animationTime);          
    }
    void EndAttack()
    {
        isAttacking = false; // Resume strafing
    }

    public void ActivateHitbox()
    {
        gameObject.SetActive(true);
        gameObject.transform.position = player.position;
        Invoke("DeactivateHitbox", 3f); // Hitbox stays active for 0.2 seconds
    }
    void DeactivateHitbox()
    {
        gameObject.SetActive(false);
        skill.SetActive(false);
    }
}


