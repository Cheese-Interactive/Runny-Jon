using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    [Header("References")]
    [SerializeField] private new Camera camera;
    [SerializeField] private TMP_Text speedText;
    [SerializeField] private Transform cameraPos;
    [SerializeField] private Transform muzzle;
    [SerializeField] private Image crosshair;
    private Animator animator;
    private Rigidbody rb;
    private LineRenderer lineRenderer;

    [Header("Looking")]
    [SerializeField][Range(0f, 100f)] private float xSensitivity;
    [SerializeField][Range(0f, 100f)] private float ySensitivity;
    [SerializeField][Range(0f, 90f)] private float topCameraClamp;
    [SerializeField][Range(0f, 90f)] private float bottomCameraClamp;
    private float xRotation;
    private float yRotation;

    [Header("Movement")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float speedIncreaseMultiplier;
    [SerializeField] private float slopeIncreaseMultiplier;
    private float moveSpeed;
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
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
    [SerializeField] private float crouchHeight;
    private float startHeight;

    [Header("Sliding")]
    [SerializeField] private float maxSlideSpeed;
    [SerializeField] private float maxSlideTime;
    private float slideTimer;
    private bool isSliding;

    [Header("Swinging")]
    [SerializeField] private float maxSwingSpeed;
    [SerializeField] private float maxGrappleDistance;
    [SerializeField] private float jointSpring;
    [SerializeField] private float jointDamper;
    [SerializeField] private float jointMassScale;
    [SerializeField] private LayerMask grappleableMask;
    private Vector3 swingPoint;
    private Vector3 currentGrapplePosition;
    private SpringJoint joint;
    private bool isSwinging;

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

        None, Walking, Sprinting, Sliding, Swinging, Air

    }

    public enum SlopeType {

        None, Valid, Invalid

    }

    private void Start() {

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
        rb.freezeRotation = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        movementState = MovementState.None;

        jumpReady = true;

        startHeight = transform.localScale.y;

        startYPos = cameraPos.localPosition.y;

    }

    private void Update() {

        float mouseX = Input.GetAxisRaw("Mouse X") * xSensitivity * 10f * Time.fixedDeltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * ySensitivity * 10f * Time.fixedDeltaTime;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -topCameraClamp, bottomCameraClamp);

        cameraPos.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);

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

        if (Input.GetKeyDown(KeyCode.C) && (horizontalInput != 0 || verticalInput != 0) && !isSwinging)
            StartSlide();

        if ((Input.GetKeyUp(KeyCode.C) || Input.GetKeyDown(KeyCode.Space)) && isSliding)
            StopSlide();

        if (Input.GetMouseButtonDown(1))
            StartSwing();

        if (Input.GetMouseButtonUp(1) && isSwinging)
            StopSwing();

        HandleHeadbob();

        if (isGrounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0f;

    }

    private void FixedUpdate() {

        if (isSwinging)
            return;

        movementDirection = transform.forward * verticalInput + transform.right * horizontalInput;

        if (CheckSlope() == SlopeType.Valid && !exitingSlope) {

            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 30f, ForceMode.Force);

            if (rb.velocity.y > 0f)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);

        } else if (CheckSlope() == SlopeType.Invalid) {

            rb.AddForce(Vector3.Cross(slopeHit.normal, Vector3.Cross(slopeHit.normal, Vector3.up)) * 500f, ForceMode.Acceleration);

        } else if (isGrounded) {

            rb.AddForce(movementDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        } else {

            rb.AddForce(movementDirection.normalized * moveSpeed * airMultiplier * 10f, ForceMode.Force);

        }

        rb.useGravity = CheckSlope() == SlopeType.None || CheckSlope() == SlopeType.Invalid;

    }

    private void LateUpdate() {

        DrawRope();

    }

    private void HandleMovementState() {

        if (isSwinging) {

            movementState = MovementState.Swinging;
            moveSpeed = maxSwingSpeed;

        } else if (isSliding) {

            movementState = MovementState.Sliding;

            if (CheckSlope() == SlopeType.None && isGrounded)
                slideTimer -= Time.deltaTime;
            else if (CheckSlope() == SlopeType.Valid)
                desiredMoveSpeed = maxSlideSpeed;
            else
                desiredMoveSpeed = sprintSpeed;

            if (slideTimer <= 0f)
                StopSlide();

        } else if (isGrounded && movementDirection == Vector3.zero) {

            animator.SetBool("isWalking", false);
            animator.SetBool("isSprinting", false);
            movementState = MovementState.None;
            desiredMoveSpeed = walkSpeed;

        } else if (isGrounded && Input.GetKey(KeyCode.LeftShift)) {

            animator.SetBool("isWalking", false);
            animator.SetBool("isSprinting", true);
            movementState = MovementState.Sprinting;
            desiredMoveSpeed = sprintSpeed;

        } else if (isGrounded) {

            animator.SetBool("isWalking", true);
            animator.SetBool("isSprinting", false);
            movementState = MovementState.Walking;
            desiredMoveSpeed = walkSpeed;

        } else if (!isGrounded && rb.velocity.magnitude > 0.01f) {

            animator.SetBool("isWalking", false);
            animator.SetBool("isSprinting", false);
            movementState = MovementState.Air;

        }

        if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > sprintSpeed - walkSpeed && moveSpeed != 0f) {

            StopAllCoroutines();
            StartCoroutine(LerpMoveSpeed());

        } else {

            moveSpeed = desiredMoveSpeed;

        }

        lastDesiredMoveSpeed = desiredMoveSpeed;

    }

    private IEnumerator LerpMoveSpeed() {

        float timer = 0f;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (timer < difference) {

            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, timer / difference);

            if (CheckSlope() == SlopeType.Valid) {

                timer += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * (1f + (Vector3.Angle(Vector3.up, slopeHit.normal) / 90f));

            } else {

                timer += Time.deltaTime * speedIncreaseMultiplier;

            }

            yield return null;

        }

        moveSpeed = desiredMoveSpeed;

    }

    private void ControlSpeed() {

        if (CheckSlope() == SlopeType.Valid || CheckSlope() == SlopeType.Invalid && !exitingSlope) {

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
        animator.SetBool("isSliding", true);
        transform.localScale = new Vector3(transform.localScale.x, crouchHeight, transform.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        slideTimer = maxSlideTime;

    }

    private void StopSlide() {

        isSliding = false;
        animator.SetBool("isSliding", false);
        transform.localScale = new Vector3(transform.localScale.x, startHeight, transform.localScale.z);

    }

    private void StartSwing() {

        RaycastHit hit;

        if (Physics.Raycast(cameraPos.position, cameraPos.forward, out hit, maxGrappleDistance, grappleableMask)) {

            isSwinging = true;

            swingPoint = hit.point;
            joint = gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = swingPoint;

            float distanceFromPoint = Vector3.Distance(transform.position, swingPoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = jointSpring;
            joint.damper = jointDamper;
            joint.massScale = jointMassScale;

            lineRenderer.positionCount = 2;
            currentGrapplePosition = muzzle.position;

        }
    }

    private void StopSwing() {

        isSwinging = false;
        lineRenderer.positionCount = 0;
        Destroy(joint);

    }

    private void DrawRope() {

        if (!joint)
            return;

        // TODO: Make 8f a variable?
        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, swingPoint, Time.deltaTime * 8f);

        lineRenderer.SetPosition(0, muzzle.position);
        lineRenderer.SetPosition(1, swingPoint);

    }

    private void HandleHeadbob() {

        if (isGrounded) {

            if (Mathf.Abs(movementDirection.x) > 0.1f || Mathf.Abs(movementDirection.z) > 0.1f) {

                timer += (movementState == MovementState.Sprinting ? sprintBobSpeed : movementState == MovementState.Walking ? walkBobSpeed : 0f) * Time.deltaTime;
                cameraPos.localPosition = new Vector3(cameraPos.localPosition.x, startYPos + Mathf.Sin(timer) * (movementState == MovementState.Sprinting ? sprintBobAmount : movementState == MovementState.Walking ? walkBobAmount : 0f), cameraPos.localPosition.z);

            }
        }
    }

    private SlopeType CheckSlope() {

        if (Physics.Raycast(feet.position, Vector3.down, out slopeHit, slopeCheckDistance)) {

            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);

            if (angle != 0f) {

                if (angle <= maxSlopeAngle)
                    return SlopeType.Valid;
                else
                    return SlopeType.Invalid;

            }
        }

        return SlopeType.None;

    }

    private Vector3 GetSlopeMoveDirection() {

        return Vector3.ProjectOnPlane(movementDirection, slopeHit.normal).normalized;

    }
}
