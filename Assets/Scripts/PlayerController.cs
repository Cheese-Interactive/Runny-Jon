using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Header("References")]
    [SerializeField] private TMP_Text speedText;
    [SerializeField] private Transform cameraPos;
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

    [Header("Headbob")]
    [SerializeField] private float walkBobSpeed;
    [SerializeField] private float walkBobAmount;
    [SerializeField] private float sprintBobSpeed;
    [SerializeField] private float sprintBobAmount;
    private float startYPos;
    private float timer;

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

        None, Walking, Sprinting, Air

    }

    public enum SlopeState {

        None, Valid, Invalid

    }

    private void Start() {

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        movementState = MovementState.None;

        jumpReady = true;

        startHeight = transform.localScale.y;

        startYPos = cameraPos.localPosition.y;

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

        if (Input.GetKeyDown(KeyCode.C) && (horizontalInput != 0 || verticalInput != 0)) {

            StartSlide();

        }

        if ((Input.GetKeyUp(KeyCode.C) || Input.GetKeyDown(KeyCode.Space)) && isSliding) {

            StopSlide();

        }

        HandleHeadbob();

        if (isGrounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0f;

    }

    private void FixedUpdate() {

        movementDirection = transform.forward * verticalInput + transform.right * horizontalInput;

        if (CheckSlope() == SlopeState.Valid && !exitingSlope) {

            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 30f, ForceMode.Force);

            if (rb.velocity.y > 0f)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);

        } else if (CheckSlope() == SlopeState.Invalid) {

            rb.AddForce(Vector3.Cross(slopeHit.normal, Vector3.Cross(slopeHit.normal, Vector3.up)) * 500f, ForceMode.Acceleration);

        } else if (isGrounded) {

            rb.AddForce(movementDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        } else {

            rb.AddForce(movementDirection.normalized * moveSpeed * airMultiplier * 10f, ForceMode.Force);

        }

        rb.useGravity = CheckSlope() == SlopeState.None || CheckSlope() == SlopeState.Invalid;

    }

    private void HandleMovementState() {

        if (isSliding) {

            if (CheckSlope() == SlopeState.None)
                slideTimer -= Time.deltaTime;

            moveSpeed = slideSpeed;

            if (slideTimer <= 0f)
                StopSlide();

        } else if (isGrounded && movementDirection == Vector3.zero) {

            animator.SetBool("isWalking", false);
            animator.SetBool("isSprinting", false);
            animator.SetBool("inAir", false);
            movementState = MovementState.None;
            moveSpeed = walkSpeed;

        } else if (isGrounded && Input.GetKey(KeyCode.LeftShift)) {

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

        if (CheckSlope() == SlopeState.Valid || CheckSlope() == SlopeState.Invalid && !exitingSlope) {

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

    private void StartSlide() {

        isSliding = true;
        transform.localScale = new Vector3(transform.localScale.x, crouchHeight, transform.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        slideTimer = maxSlideTime;

    }

    private void StopSlide() {

        isSliding = false;
        transform.localScale = new Vector3(transform.localScale.x, startHeight, transform.localScale.z);

    }

    private void HandleHeadbob() {

        if (isGrounded) {

            if (Mathf.Abs(movementDirection.x) > 0.1f || Mathf.Abs(movementDirection.z) > 0.1f) {

                timer += (movementState == MovementState.Sprinting ? sprintBobSpeed : movementState == MovementState.Walking ? walkBobSpeed : 0f) * Time.deltaTime;
                cameraPos.localPosition = new Vector3(cameraPos.localPosition.x, startYPos + Mathf.Sin(timer) * (movementState == MovementState.Sprinting ? sprintBobAmount : movementState == MovementState.Walking ? walkBobAmount : 0f), cameraPos.localPosition.z);

            }
        }
    }

    private SlopeState CheckSlope() {

        if (Physics.Raycast(feet.position, Vector3.down, out slopeHit, slopeCheckDistance)) {

            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);

            if (angle != 0f) {

                if (angle <= maxSlopeAngle)
                    return SlopeState.Valid;
                else
                    return SlopeState.Invalid;

            }
        }

        return SlopeState.None;

    }

    private Vector3 GetSlopeMoveDirection() {

        return Vector3.ProjectOnPlane(movementDirection, slopeHit.normal).normalized;

    }
}
