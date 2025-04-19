using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerLocomotion playerLocomotion;
    AnimatorManager animatorManager;

    public Vector2 movementInput;
    public Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;


    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    public bool b_input;
    public bool attack_input;
    public bool jump_input;
    public bool dodge_input;

    public bool command_input;

    public bool heal_input;
    public bool skill_input;
    public Pet npc;

    private void Awake() {
        animatorManager = GetComponent<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();

    }

    private void OnEnable() {
        if (playerControls == null) {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            playerControls.PlayerActions.Sprint.performed += i => b_input = true;
            playerControls.PlayerActions.Sprint.canceled += i => b_input = false;

            playerControls.PlayerActions.Attack.performed += i => attack_input = true;
            playerControls.PlayerActions.Attack.canceled += i => attack_input = false;

            playerControls.PlayerActions.Dodge.performed += i => dodge_input = true;
            playerControls.PlayerActions.Dodge.canceled += i => dodge_input = false;

            playerControls.PlayerActions.Jump.performed += i => jump_input = true;
            
            playerControls.Command.Open.performed += i => command_input = true;
            playerControls.Command.Open.canceled += i => command_input = false;

            playerControls.Command.Heal.performed += i => heal_input = true;
            playerControls.Command.Heal.canceled += i => heal_input = false;

            playerControls.Command.Skill.performed += i => skill_input = true;
            //playerControls.Command.Skill.canceled += i => skill_input = false;

        }

        playerControls.Enable();
    }
    private void OnDisable() {
        playerControls.Disable();
    }

    public void HandleAllInputs() {
        HandleMovementInput();
        HandleSprintingInput();
        HandleAttackInput();
        HandleDodgeInput();
        HandleJumpingInput();

        OpenCommand();
        Heal();
        Skill1();
        //
        //
    }
    private void HandleMovementInput() {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputX = cameraInput.x;
        cameraInputY = cameraInput.y;



        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(0 , moveAmount , playerLocomotion.isSprinting);
    }
    private void HandleSprintingInput() {
        if (b_input && moveAmount > 0.5f && playerLocomotion.stamina > 0 ) {
            playerLocomotion.isSprinting = true;
        }
        else {
            playerLocomotion.isSprinting = false;
        }
    }
    private void HandleAttackInput() {
        if (attack_input) {
            attack_input = false;
            playerLocomotion.HandleAttack();
        }
        else {
            //playerLocomotion.isAttacking = false;
        }
    }
    private void HandleDodgeInput() {
        if (dodge_input) {
            dodge_input = false;
            playerLocomotion.HandleDodge();
            
        }
        else {
            //playerLocomotion.isDodging = false;
        }
    }
    private void HandleJumpingInput() {
        if (jump_input) {
            jump_input = false;
            playerLocomotion.HandleJumping();
        }
    }
    public void OpenCommand() {
        if (command_input) {
            Debug.Log(NPCState.Attack);
            npc.SetCommand(NPCState.Attack);
        }
    }

    public void Heal() {
        if (heal_input) {
            Debug.Log(NPCState.Heal);
            npc.SetCommand(NPCState.Heal);
        }
    }
    public void Skill1() {
        if (skill_input) {
            Debug.Log(NPCState.Skill_1);
            npc.SetCommand(NPCState.Skill_1);
        }
    }
}
