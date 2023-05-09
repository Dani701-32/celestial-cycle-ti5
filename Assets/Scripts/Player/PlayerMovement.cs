using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed, maxSpeed, rotation, jumpSpeed;
    private float ySpeed, stepOffset;
    private CharacterController characterController;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform cameraTransform;
    bool isRunning = false;
    public bool isGrounded = false;
    private float currentSpeed;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        stepOffset = characterController.stepOffset;
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        isRunning = (Input.GetKey(KeyCode.LeftShift));

        //Animation
        animator.SetBool("isWalking", (verticalInput != 0 || horizontalInput != 0));
        animator.SetBool("isRunning", isRunning);
        

        //Movment
        currentSpeed = (isRunning) ? maxSpeed : speed;
        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput);
        float magnitude = Mathf.Clamp01(movement.magnitude) * currentSpeed;
        movement = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movement;
        movement.Normalize();

        //Jump
        isGrounded = characterController.isGrounded;                
        ySpeed += Physics.gravity.y * Time.deltaTime;
        if (characterController.isGrounded)
        {
            ySpeed = -0.5f;
            characterController.stepOffset = stepOffset;
            if (Input.GetButtonDown("Jump"))
            {
                ySpeed = jumpSpeed;
            }
            // animator.SetBool("IsJumping", false);
        }
        else
        {
            // animator.SetBool("IsJumping", true);
            characterController.stepOffset = 0;
        }

        Vector3 velocity = movement * magnitude;
        velocity.y = ySpeed;
        characterController.Move(velocity * Time.deltaTime);

        //Rotate player
        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotation * Time.deltaTime);
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        Cursor.lockState = (hasFocus) ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
