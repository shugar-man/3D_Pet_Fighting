using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    public Transform player;
    private Transform playerPositionAtPoint;
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

    public GameObject skill;
    public GameObject skill2;
    public GameObject skill3;
    public int damage = 20;
    public int co=0;

    //public LayerMask whatIsGround, whatIsPlayer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        agent.isStopped = true;
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        if (isAttacking)
        {
            // ระหว่างโจมตี: อยู่นิ่ง หยุดหมุน หยุดเดิน
            agent.isStopped = true;
            agent.velocity = Vector3.zero; // หยุดแรงเคลื่อนที่
            anim.SetBool("Chasing", false);
            anim.SetBool("isAttacking", true);
            anim.SetFloat("StrafeDirection", 0);
            return; // ไม่ต้องทำอย่างอื่น
        }
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        //anim.SetFloat("Distance", distanceToPlayer);


        if (distanceToPlayer < detectionRange)
        {
            // Always face the player
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
            if (distanceToPlayer > circleDistance + 0.5f) // Move toward player until within range
            {
                if (isAttacking) return;

                Debug.Log("asf");
                
                agent.isStopped = false;
                agent.SetDestination(player.position);
                anim.SetBool("Chasing",true);
                anim.SetBool("Battle" , false);
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
                if (!isAttacking)
                {
                    AttackPlayer();
                    //lastAttackTime = Time.time;
                }
            }

            
        }
    }


    void StrafeAroundPlayer()
    {
        if (isAttacking) return;
        anim.SetBool("Battle" , true);
        if (isAttacking) return;
        strafeSide = 1; // 1 = ขวา, -1 = ซ้าย

        // เพิ่ม timer
        strafeTimer += Time.deltaTime * strafeSpeed;

        // Set animation
        anim.SetFloat("StrafeDirection", strafeSide);
        anim.SetBool("WalkRight", strafeSide > 0);
        anim.SetBool("WalkLeft", strafeSide < 0);

        // หมุนรอบผู้เล่น
        float rotationSpeed = strafeSpeed * strafeSide; // ควบคุมทิศทางด้วย strafeSide
        transform.RotateAround(player.position, Vector3.up, rotationSpeed * Time.deltaTime);

        // มองผู้เล่นตลอดเวลา
        transform.LookAt(player);

        // ระยะห่างจากผู้เล่น
        Vector3 offsetDir = (transform.position - player.position).normalized;
        transform.position = player.position + offsetDir * circleDistance;
    }
    void AttackPlayer()
    {

        isAttacking = true;
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        anim.ResetTrigger("Skill");
        //anim.ResetTrigger("Attack_2");
        //anim.ResetTrigger("Attack_3");

        agent.isStopped = true;
        int skillType = Random.Range(1, 3); // Random number between 1 and 3

            if (skillType == 1) {
                anim.SetTrigger("Skill");
                Invoke("ActivateSkill", 2.5f);
            }
               
             
            else if (skillType == 2) {
                anim.SetTrigger("Skill");
                Invoke("ActivateSkill2", 2.8f);
            }
        float animationTime = anim.GetCurrentAnimatorStateInfo(0).length;

        // Resume strafing after the animation ends
        Invoke("EndAttack", 3.7f);
                
    }
    void EndAttack()
    {
        isAttacking = false; // Resume strafing
        anim.SetBool("isAttacking", false);
        agent.isStopped = false;
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player")) // Check if it's the player
        {
        Debug.Log("Particle hit: " + other.name);

        // Example: damage the enemy
        Health playerHealth = other.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Enemy hit the player!");
            }
        }
    }


    public void ActivateSkill()
    {
        GameObject skillActive = Instantiate(skill, player.position , player.rotation);

        Destroy(skillActive, 3f);
        anim.ResetTrigger("Skill");
    }

    public void ActivateSkill2()
    {
        if (player == null) return;

        // 1. คำนวณทิศทางไปยังผู้เล่น
        Vector3 direction = (player.position - transform.position).normalized;

        // 2. หาตำแหน่งเกิดของสกิลให้ออกห่างจากศัตรูนิดหนึ่ง
        Vector3 spawnPosition = transform.position + direction * 1f;

        // 3. Instantiate สกิลพร้อมหันหน้าไปทางผู้เล่น
        GameObject skillActive_2 = Instantiate(skill2, spawnPosition, Quaternion.LookRotation(direction));

        // 4. ยิงสกิลไปทิศที่คำนวณไว้
        Rigidbody rb = skillActive_2.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.useGravity = false; // ปิดถ้าไม่ต้องการให้ตก
        rb.velocity = direction * 5f;

        // 5. ตั้ง forward ให้สกิลหมุนไปทางผู้เล่น (เผื่อมี Visual Effect)
        skillActive_2.transform.forward = direction;

        // 6. ลบสกิลหลัง 0.5 วินาที
        Destroy(skillActive_2, 0.5f);

        // 7. รีเซ็ต trigger ของ animation (เผื่อใช้กับ state machine)
        anim.ResetTrigger("Skill");
    }
}