using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class Pet : MonoBehaviour
{

    public Transform player;
    public Transform[] patrolPoints; // For patrol behavior
    public Animator animator;
    public float followDistance = 5f;
    public float attackDistance = 3f;
    public float detectRadius = 7f;
    
    private int currentPatrolIndex = 0;
    private Transform currentTarget;
    private NavMeshAgent agent;
    public NPCState currentState = NPCState.Attack;
    public Health health;
    


    public GameObject skillPrefab;   // ใส่ prefab ลูกกระสุน
    public Transform firePoint;
    public float projectileSpeed = 5f;

    public GameObject healPrefab;


    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player").transform;
        //speed = Animator.StringToHash("speed");
    }
    void Update()
    {
        switch (currentState)
        {
            case NPCState.Follow:
                FollowPlayer();
                break;
            case NPCState.Attack:
                AttackEnemy();
                break;
            /*case NPCState.Heal:
                DefendPlayer();
                break;*/
            case NPCState.Stay:
                StayPut();
                break;
            case NPCState.Heal:
                HealPlayer();
                break;
            case NPCState.Skill_1:
                Skill1();
                break;
        }
    }
    void FollowPlayer()
    {
        FindClosestEnemy();
        if (currentTarget != null) {
            currentState = NPCState.Attack;
        }
        animator.SetBool("isWalking", true);
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > followDistance) {
            agent.SetDestination(player.position);
        }
            
        else {
            agent.SetDestination(transform.position); // Stop if close
            animator.SetBool("isWalking", false);
        }
            
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;
        
        if (Vector3.Distance(transform.position, patrolPoints[currentPatrolIndex].position) < 1f)
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;

        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    void AttackEnemy()
    {
        FindClosestEnemy();
        if (currentTarget != null)
        
        {
            Debug.Log("abc");
            float distance = Vector3.Distance(transform.position, currentTarget.position);
            if (distance > attackDistance) {
                animator.SetBool("isWalking", true);
                agent.SetDestination(currentTarget.position);
            }
                
            else
                Attack();
                animator.SetBool("isWalking", false);
                
        }
        else {
            currentState = NPCState.Follow;
        }
    }

    void DefendPlayer()
    {
        FindClosestEnemy();
        if (currentTarget != null)
        {
            agent.SetDestination(currentTarget.position);
        }
        else
        {
            FollowPlayer(); // If no threats, stay near player
        }
    }
    void HealPlayer()
    {
        if (health == null)
        {
            Debug.LogError("Health is NULL!");
            return;
        }

        if (health.currentMana < 99)
        {
            Debug.Log("Not enough mana.");
            currentState = NPCState.Attack;
            return;
        }

        animator.Play("Skill");

        GameObject skill = Instantiate(healPrefab, player.position , Quaternion.identity);
        Destroy(skill, 3f);

        health.currentMana -= 100f;
        Debug.Log("Mana after heal: " + health.currentMana);

        currentState = NPCState.Attack;
        Debug.Log("Set state to ATTACK");
    }
    void StayPut()
    {
        animator.SetBool("Run Forward", false);
        agent.SetDestination(transform.position);
    }

    void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        currentTarget = enemies
            .Where(e => Vector3.Distance(player.position, e.transform.position) < detectRadius)
            .OrderBy(e => Vector3.Distance(transform.position, e.transform.position))
            .Select(e => e.transform)
            .FirstOrDefault();
    }

    void Attack()
    {
        Debug.Log("NPC attacks " + currentTarget.name);
        health.HealMana();
        animator.Play("Attack02");
    }

    public void SetCommand(NPCState newState)
    {
        currentState = newState;
    }


    public void Skill1()
    {
        
        if (health.currentMana < 99) {
            currentState = NPCState.Attack;
            return;
        }
         
        FindClosestEnemy();
        if (currentTarget == null) {
            Debug.Log("No target found.");
            return;
        }

        //Debug.Log(currentTarget.name);
        //transform.LookAt(currentTarget.position);
        animator.Play("Skill");
        Debug.Log("aa");
        currentState = NPCState.Attack;
        Debug.Log("ab");
        
        
        

        // สร้าง Particle
        GameObject projectile = Instantiate(skillPrefab, firePoint.position + new Vector3(0, 0.2f, 0), Quaternion.identity);
        Vector3 direction = (currentTarget.position - firePoint.position).normalized;

        // ปรับทิศทางให้ Particle พุ่ง
        ParticleSystem ps = projectile.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            var vel = ps.velocityOverLifetime;
            vel.enabled = true;
            vel.space = ParticleSystemSimulationSpace.World;
            vel.x = direction.x * projectileSpeed;
            vel.y = direction.y * projectileSpeed;
            vel.z = direction.z * projectileSpeed;

            Destroy(projectile, ps.main.duration);
            health.currentMana -= 100f;
            
        }
        else
        {
            Debug.LogWarning("Projectile prefab has no ParticleSystem!");
        }
        
    }

}

