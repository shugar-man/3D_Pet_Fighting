using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Animator animator;
    InputManager inputManager;
    CameraManager cameraManager;
    PlayerLocomotion playerLocomotion;

    public bool isInteracting;

    private void Awake() {
        animator = GetComponent<Animator>();
        inputManager = GetComponent<InputManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();

    }
    private void Update() {
        inputManager.HandleAllInputs();
    }
    private void FixedUpdate() {
        playerLocomotion.HandleAllMovment();
    }

    private void LateUpdate() {
        cameraManager.HandleAllCameraMovement();

        isInteracting = animator.GetBool("isInteracting");
        playerLocomotion.isAttacking  = animator.GetBool("isAttacking");
        playerLocomotion.isJumping = animator.GetBool("isJumping");
        animator.SetBool("isGrounded" , playerLocomotion.isGrounded);
    }
}
