using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed;
    public float runSpeed;
    public float rotationSpeed;
    public float jumpSpeed;

    [Header("Movement Transition")]
    [Range(0, 1)]
    public float speedDampTime;

    private bool isRunning = false;
    private float ySpeed;
    private float stepOffset;
    private float currentSpeed;

    private InputAction runningAction;
    private InputAction jumpAction;
    private InputAction drawWeaponAction;
    private InputAction attackAction;
    public InputAction interactAction;
    public PlayerInput playerInput { get; private set; }
    private PlayerInputActions playerInputActions;
    private CharacterController characterController;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Transform cameraTransform;
    public bool isGrounded = false;
    public bool combatMode = false;

    [Header("Combat Controllers")]
    public GameObject currentWeapon;
    public float timeToDraw;
    private bool counter = false;
    private bool drawWeapon = false;

    [SerializeField]
    private float currentTime;

    [Header("Artifact Controllers")]
    public GameObject currentArtifact;

    //  Attack

    public bool isCombat = false;
    private bool attack = false;
    public float currentAttackTime = 0f;
    public float delayAttack = 0;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        playerInput = GetComponent<PlayerInput>();

        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();

        runningAction = playerInput.actions["Run"];
        jumpAction = playerInput.actions["Jump"];
        drawWeaponAction = playerInput.actions["DrawWeapon"];
        attackAction = playerInput.actions["Attack"];
        interactAction = playerInput.actions["Interact"];
    }

    void Start()
    {
        currentSpeed = walkSpeed;
        stepOffset = characterController.stepOffset;
        if (currentWeapon != null)
        {
            currentWeapon.SetActive(combatMode);
        }
        currentTime = timeToDraw;
        animator.SetFloat("speed", 0, speedDampTime, Time.deltaTime);
        isRunning = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        if (runningAction.triggered)
            isRunning = (isRunning) ? false : true;
        if (drawWeaponAction.triggered && currentWeapon != null)
        {
            drawWeapon = (drawWeapon) ? false : true;
            counter = true;
        }
        if (attackAction.triggered && !counter && isCombat && !isRunning)
        {
            delayAttack = 1.2f;
            currentAttackTime = 0f;
            attack = true;
            if(currentWeapon.GetComponent<SlashAttack>() != null)
            {
                currentWeapon.GetComponent<SlashAttack>().Slash();
            }
            
        }

        Walking(inputVector);

        isGrounded = characterController.isGrounded;
        ySpeed += Physics.gravity.y * Time.deltaTime;

        Jump();
        DrawWeapon();

        if (drawWeapon)
        {
            Attack();
        }

        if (animator.GetFloat("speed") < .001f)
        {
            animator.SetFloat("speed", 0);
        }
    }

    void OnDisable()
    {
        animator.SetFloat("speed", 0);
        isRunning = false;
    }

    private void Jump()
    {
        if (characterController.isGrounded)
        {
            ySpeed = -.5f;
            characterController.stepOffset = stepOffset;

            if (jumpAction.triggered)
            {
                Debug.Log("Jumping");
                ySpeed = jumpSpeed;
            }

            return;
        }
        characterController.stepOffset = 0;
    }

    private void Walking(Vector2 inputVector)
    {
        float speedAnim = inputVector.magnitude;
        if (isRunning && inputVector != Vector2.zero)
            speedAnim += .5f;

        currentSpeed = (isRunning) ? runSpeed : walkSpeed;
        animator.SetFloat("speed", speedAnim, speedDampTime, Time.deltaTime);
        Vector3 movement = new Vector3(inputVector.x, 0, inputVector.y);

        float magnitude = Mathf.Clamp01(movement.magnitude) * currentSpeed;

        movement =
            Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movement;
        movement.Normalize();
        Vector3 velocity = movement * magnitude;
        velocity.y = ySpeed;
        characterController.Move(velocity * Time.deltaTime);

        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                toRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    void Attack()
    {
        if (attack)
        {
            if (currentAttackTime == 0)
            {
                animator.SetTrigger("attack");
            }
            currentAttackTime += Time.deltaTime;
            if (currentAttackTime >= delayAttack)
            {
                animator.SetTrigger("move");
                attack = false;
                currentAttackTime = 0;
            }
        }
    }

    void DrawWeapon()
    {
        if (counter)
        {
            if (currentTime <= 0)
            {
                if (currentWeapon != null)
                {
                    currentWeapon.SetActive(drawWeapon);
                }
                currentTime = timeToDraw;
                counter = false;
                animator.ResetTrigger("drawWeapon");
                animator.ResetTrigger("sheathWeapon");
                return;
            }
            currentTime -= Time.deltaTime;
            if (drawWeapon)
            {
                animator.SetTrigger("drawWeapon");
                isCombat = true;
            }
            else
            {
                animator.SetTrigger("sheathWeapon");
                isCombat = false;
            }
        }
    }

    public void UniqueppedWeapon()
    {
        counter = true;
        currentTime = timeToDraw;
        drawWeapon = false;
        Destroy(currentWeapon);
        DrawWeapon();
        currentWeapon = null;
    }

    public void StartDealDamage()
    {
        currentWeapon.GetComponentInChildren<DamageDealer>().StartDealDamage();
    }

    public void EndDamage()
    {
        currentWeapon.GetComponentInChildren<DamageDealer>().EndDamage();
    }

    void OnApplicationFocus(bool hasFocus)
    {
        Cursor.lockState = (hasFocus) ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
