using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerManager playerManager;
    AnimatorManager animatorManager;
    InputManager inputManager;


    Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody playerRigidbody;

    [Header("Falling")]
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public float rayCastHeightOffSet = 0.5f;
    public LayerMask groundLayer;

    [Header("Movement Flages")]
    public bool isSprinting;
    public bool isAttacking;
    public bool isGrounded;
    public bool isJumping;
    public bool isHitting;
    public bool isDodging;
    public bool died;

    [Header("Movement Speeds")]
    public float walkingSpeed = 4;
    public float runningSpeed = 7;
    public float sprintingspeed = 10;
    public float rotationSpeed = 15;

    [Header("Jump Speeds")]
    public float jumpHeight = 3;
    public float gravityIntensity = -15;

    public Slider staminaBar;
    public float stamina;
    public float staminaMax = 100;
    private bool isRecoveringStamina = false;
    public int damage = 90;


    [SerializeField] int gravity = 25;
    float velocityY;
    [SerializeField] AnimationCurve dodgeCurve;
    float dodgeTimer;

    CharacterController characterController;

    public void Awake() {
        stamina = staminaMax;
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<AnimatorManager>();
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        cameraObject = Camera.main.transform;

        Keyframe dodge_lasatFrame = dodgeCurve[dodgeCurve.length - 1];
        dodgeTimer = dodge_lasatFrame.time;

    }
    void Update() {
        if (staminaBar.value != stamina) {
            staminaBar.value = stamina;
        }

        if (staminaBar.value <=0) {
            isSprinting = false;
        }
        if (stamina < 100 && !isRecoveringStamina) {
        StartCoroutine(RecoverStamina());
    }
    }
    private IEnumerator RecoverStamina() {
        isRecoveringStamina = true;

        if (stamina <= 0) {
            Debug.Log("ff");
            yield return new WaitForSeconds(3f); // รอ 3 วิถ้า stamina หมด
        }

        while (stamina < 100) {
            stamina += 3;
            yield return new WaitForSeconds(0.1f); // ค่อยๆ เพิ่มทีละนิด
        }

        isRecoveringStamina = false;
    }
    public void HandleAllMovment() {
        //HandleAttack();
        //HandleFallingAndLanding();
        if (playerManager.isInteracting) {
            return;
        }
        if (isJumping) {
            return;
        }
        if (isAttacking) {
            return;
        }
        if (isDodging) {
            return;
        }
        if (isHitting) {
            return;
        }
        if (died) {
            return;
        }
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement() {
        
        isHitting = false;
        animatorManager.animator.SetBool("isHitting" , false);
        isAttacking = false;
        isDodging = false;

        velocityY -= Time.deltaTime * gravity;
        velocityY = Mathf.Clamp(velocityY, -10 , 10);
        
        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;
        if (isSprinting ) {
            moveDirection = moveDirection * sprintingspeed;
            stamina -= 1;
        }
        else {
            if (inputManager.moveAmount >= 0.5f) {
            moveDirection = moveDirection * runningSpeed;
            }
        else {
            moveDirection = moveDirection * walkingSpeed;
            }
            if (stamina < 100) {
                //stamina += 2;
            }
        }
        


        //spt
        //run
        //walk
        //moveDirection = moveDirection * movementSpeed;


        Vector3 movementVelocity = moveDirection;
        playerRigidbody.velocity = movementVelocity;
        //Movement Input
    }
    

    private void HandleRotation() {
        if (isJumping) {
            return;
        }

        Vector3 targetDirection = Vector3.zero;
        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero) {
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation , targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    private void HandleFallingAndLanding() {
        RaycastHit hit;
        Vector3 rayCastsOrigin = transform.position;
        rayCastsOrigin.y = rayCastsOrigin.y + rayCastHeightOffSet;
        if (!isGrounded && !isJumping) {
            playerRigidbody.drag = 0;

            if (!playerManager.isInteracting) {
                animatorManager.PlayerTargetAnimation("Falling" , true);
            }
            inAirTimer = inAirTimer + Time.deltaTime;
            playerRigidbody.AddForce(transform.forward * leapingVelocity);
            playerRigidbody.AddForce(Vector3.down * (fallingVelocity * (1 + inAirTimer)* 3), ForceMode.Acceleration);
        }
        float sphereCastDistance = 1.0f; // Adjust this value based on character height
        if (Physics.SphereCast(rayCastsOrigin, 0.2f, -Vector3.up, out hit, sphereCastDistance, groundLayer)) {
            if (!isGrounded && !playerManager.isInteracting) {
                Debug.Log("bbbb");
                animatorManager.PlayerTargetAnimation("Landing" , true);
                
            }
                inAirTimer = 0;
                isGrounded = true;
            
        }
        else {

            isGrounded = false;
        }

    }
    public void HandleJumping() {
        if (isGrounded) {
            animatorManager.animator.SetBool("isJumping" , true);
            animatorManager.PlayerTargetAnimation("jump" , true);

            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            Vector3 palyerVelocity = moveDirection;
            palyerVelocity.y = jumpingVelocity;
            playerRigidbody.velocity = palyerVelocity;
        }
    }

    public void HandleAttack() {
        if (isGrounded) {
            animatorManager.animator.SetBool("isAttacking" , true);
            animatorManager.PlayerTargetAnimation("attack" , false);


        }
        StartCoroutine(ResetAttackBool());
        
    }

    public void HandleDodge() {
        
        StartCoroutine(Dodge());
        StartCoroutine(ResetDodgeBool());
        
    }
    IEnumerator Dodge() {
        isDodging = true;
        animatorManager.animator.SetBool("isDodging", true);
        animatorManager.PlayerTargetAnimation("dodge", false);

        float timer = 0f;
        float speed = 10f;

        Vector3 dodgeDirection = transform.forward;

        while (timer < dodgeTimer) {
            // อัปเดตแรงโน้มถ่วง
            velocityY -= gravity * Time.deltaTime;
            velocityY = Mathf.Clamp(velocityY, -10f, 10f);

            Vector3 move = (dodgeDirection * speed) + (Vector3.up * velocityY);
            characterController.Move(move * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }

        isDodging = false;
        animatorManager.animator.SetBool("isDodging", false);
    }

    IEnumerator ResetDodgeBool()
    {
        yield return new WaitForSeconds(1f);
        animatorManager.animator.SetBool("isDodging" , false);
        isDodging = false;
    }



    IEnumerator ResetAttackBool()
    {
        yield return new WaitForSeconds(1.5f);
        animatorManager.animator.SetBool("isAttacking", false);
    }
    /*private void OnCollisionEnter(GameObject other)
    {
        if (other.gameObject.tag.Equals("Attack_Spot") || other.gameObject.tag.Equals("Enemy")) // Check if it's the player
        {
            Debug.Log("Play hit!");
            Health BossHealth = other.GetComponent<Health>();
            if (BossHealth != null)
            {
                BossHealth.TakeDamage(damage);
            }
        }
    }*/
}
