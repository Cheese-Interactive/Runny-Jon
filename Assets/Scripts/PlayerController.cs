using System.Security.Cryptography;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Header("References")]
    [SerializeField] private TMP_Text speedText;
    private Animator animator;
    private Rigidbody rb;

    [Header("Movement")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    private float moveSpeed;
    private float verticalInput;
    private float horizontalInput;
    private Vector3 movementDirection;
    private MovementState movementState;

    [Header("Jumping")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    private bool jumpReady;

    [Header("Crouching")]
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float crouchHeight;
    private float startHeight;

    [Header("Sliding")]
    [SerializeField] private float maxSlideTime;
    [SerializeField] private float slideSpeed;
    private float slideTimer;
    private bool isSliding;

    [Header("Ground Check")]
    [SerializeField] private Transform feet;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask environmentMask;
    private bool isGrounded;

    [Header("Slope Handling")]
    [SerializeField] private float maxSlopeAngle;
    [SerializeField] private float slopeCheckDistance;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Drag Control")]
    [SerializeField] private float groundDrag;

    public enum MovementState {

        None, Walking, Sprinting, Crouching, Air

    }

    private void Start() {

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        movementState = MovementState.None;

        jumpReady = true;

        startHeight = transform.localScale.y;

    }

    private void Update() {

        isGrounded = Physics.CheckSphere(feet.position, groundCheckRadius, environmentMask);
        animator.SetBool("isGrounded", isGrounded);

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        ControlSpeed();
        speedText.text = "Speed: " + (Mathf.Round(rb.velocity.magnitude * 100f) / 100f);
        HandleMovementState();

        if (Input.GetKeyDown(KeyCode.Space) && jumpReady && isGrounded) {

            jumpReady = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);

        }

        if (Input.GetKeyDown(KeyCode.LeftControl)) {

            Crouch();

        }

        if (Input.GetKeyUp(KeyCode.LeftControl)) {

            Uncrouch();

        }

        if (Input.GetKeyDown(KeyCode.C) && (horizontalInput != 0 || verticalInput != 0)) {

            StartSlide();

        }

        if ((Input.GetKeyUp(KeyCode.C) || Input.GetKeyDown(KeyCode.Space)) && isSliding) {

            StopSlide();

        }

        if (isGrounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0f;

    }

    private void FixedUpdate() {

        movementDirection = transform.forward * verticalInput + transform.right * horizontalInput;

        if (CheckSlope() && !exitingSlope) {

            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 50f, ForceMode.Force);

            if (rb.velocity.y > 0f)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);

        } else if (isGrounded) {

            rb.AddForce(movementDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        } else {

            rb.AddForce(movementDirection.normalized * moveSpeed * airMultiplier * 10f, ForceMode.Force);

        }

        rb.useGravity = !CheckSlope();

    }

    private void HandleMovementState() {

        if (isSliding && isGrounded) {

            if (!CheckSlope())
                slideTimer -= Time.deltaTime;

            moveSpeed = slideSpeed;

            if (slideTimer <= 0f)
                StopSlide();

        } else if (Input.GetKey(KeyCode.LeftControl)) {

            movementState = MovementState.Crouching;
            moveSpeed = crouchSpeed;

        } else if (isGrounded && movementDirection == Vector3.zero) {

            animator.SetBool("isWalking", false);
            animator.SetBool("isSprinting", false);
            animator.SetBool("inAir", false);
            movementState = MovementState.None;
            moveSpeed = walkSpeed;

        } else if (isGrounded && Input.GetKey(KeyCode.LeftShift) && !CheckSlope()) {

            animator.SetBool("isWalking", false);
            animator.SetBool("isSprinting", true);
            animator.SetBool("inAir", false);
            movementState = MovementState.Sprinting;
            moveSpeed = sprintSpeed;

        } else if (isGrounded) {

            animator.SetBool("isWalking", true);
            animator.SetBool("isSprinting", false);
            animator.SetBool("inAir", false);
            movementState = MovementState.Walking;
            moveSpeed = walkSpeed;

        } else if (!isGrounded && rb.velocity.magnitude > 0.01f) {

            animator.SetBool("isWalking", false);
            animator.SetBool("isSprinting", false);
            animator.SetBool("inAir", true);
            movementState = MovementState.Air;

        }
    }

    private void ControlSpeed() {

        if (CheckSlope() && !exitingSlope) {

            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;

        } else {

            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > moveSpeed) {

                Vector3 controlledVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(controlledVel.x, rb.velocity.y, controlledVel.z);

            }
        }
    }

    private void Jump() {

        jumpReady = false;
        exitingSlope = true;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

    }

    private void ResetJump() {

        jumpReady = true;
        exitingSlope = false;

    }

    private void Crouch() {

        transform.localScale = new Vector3(transform.localScale.x, crouchHeight, transform.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

    }

    private void Uncrouch() {

        transform.localScale = new Vector3(transform.localScale.x, startHeight, transform.localScale.z);

    }

    private void StartSlide() {

        isSliding = true;
        Crouch();

        slideTimer = maxSlideTime;

    }

    private void StopSlide() {

        isSliding = false;
        Uncrouch();

    }

    private bool CheckSlope() {

        if (Physics.Raycast(feet.position, Vector3.down, out slopeHit, slopeCheckDistance)) {

            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle <= maxSlopeAngle && angle != 0;

        }

        return false;

    }

    private Vector3 GetSlopeMoveDirection() {

        return Vector3.ProjectOnPlane(movementDirection, slopeHit.normal).normalized;

    }
}
