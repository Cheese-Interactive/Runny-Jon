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
    [SerializeField] private Transform[] obstacleCheckers;
    private GameManager gameManager;
    private GameUIController gameUIController;
    private Animator animator;
    private Rigidbody rb;
    private LineRenderer lineRenderer;
    private Spring spring;

    [Header("Looking")]
    [SerializeField][Range(0f, 100f)] private float xSensitivity;
    [SerializeField][Range(0f, 100f)] private float ySensitivity;
    [SerializeField][Range(0f, 90f)] private float topCameraClamp;
    [SerializeField][Range(0f, 90f)] private float bottomCameraClamp;
    [SerializeField][Range(0f, 90f)] private float wallSideCameraClamp;
    [SerializeField][Range(0f, 90f)] private float wallOutsideCameraClamp;
    private float xRotation;
    private float yRotation;
    [SerializeField] private bool lookEnabled;

    [Header("Movement")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float speedIncreaseMultiplier;
    [SerializeField] private float slopeIncreaseMultiplier;
    private bool levelWalkEnabled;
    private bool levelSprintEnabled;
    private bool levelJumpEnabled;
    private bool levelCrouchEnabled;
    private bool levelSlideEnabled;
    private bool levelWallRunEnabled;
    private bool levelSwingEnabled;
    private bool levelZiplineEnabled;
    private bool walkEnabled;
    private bool sprintEnabled;
    private bool jumpEnabled;
    private bool crouchEnabled;
    private bool slideEnabled;
    private bool wallRunEnabled;
    private bool swingEnabled;
    private bool ziplineEnabled;
    private float moveSpeed;
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    private float verticalInput;
    private float horizontalInput;
    private Vector3 movementDirection;
    private MovementState movementState;
    private Coroutine moveSpeedCoroutine;
    private bool firstMovementDetected;

    [Header("Jumping")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    private bool jumpReady;

    [Header("Crouching")]
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float crouchScale;
    [SerializeField] private float uncrouchMinClearing;
    private float startScale;
    private float startHeight;
    private bool uncrouchQueued;

    [Header("Sliding")]
    [SerializeField] private float maxSlideSpeed;
    [SerializeField] private float maxSlideTime;
    [SerializeField] private float upwardsSlideFactor;
    private float slideTimer;
    private bool slideQueued;

    [Header("Wall Running")]
    [SerializeField] private float wallRunSpeed;
    [SerializeField] private float wallRunForce;
    [SerializeField] private float wallNormalForce;
    [SerializeField] private float exitWallTime;
    [SerializeField] private bool useWallRunTimer;
    [SerializeField] private float maxWallRunTime;
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private float lookRotationLerpDuration;
    private bool canWallRun;
    private Transform lastWall;
    private Vector3 wallForward;
    private Vector3 wallNormal;
    private Coroutine lookRotationLerpCoroutine;
    private float wallRunTimer;
    private float exitWallTimer;
    private bool exitingWall;

    [Header("Wall Jumping")]
    [SerializeField] private float wallJumpUpForce;
    [SerializeField] private float wallJumpSideForce;

    [Header("Wall Climbing")]
    [SerializeField] private float wallClimbSpeed;
    private bool wallRunningUpwards;
    private bool wallRunningDownwards;

    [Header("Wall Run Gravity")]
    [SerializeField] private bool useWallRunGravity;
    [SerializeField] private float initialGravityCounterForce;
    [SerializeField] private float gravityDelay;
    [SerializeField] private float gravityDecrementRate;
    [SerializeField] private float gravityDecrementAmount;
    private float gravityCounterForce;
    private Coroutine gravityDelayCoroutine;

    [Header("Wall Run Animations")]
    [SerializeField] private float wallRunCameraFOV;
    [SerializeField] private float camFOVLerpDuration;
    [SerializeField] private float wallRunCamTilt;
    [SerializeField] private float camTiltLerpDuration;
    private float startCameraFOV;
    private float startCameraZTilt;
    private Coroutine lerpCamFOVCoroutine;
    private Coroutine lerpCamTiltCoroutine;

    [Header("Wall Detection")]
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float minJumpHeight;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft;
    private bool wallRight;

    [Header("Swinging")]
    [SerializeField] private float maxSwingSpeed;
    [SerializeField] private float maxSwingDistance;
    [SerializeField] private float horizontalThrustForce;
    [SerializeField] private float forwardThrustForce;
    [SerializeField] private float cableExtendSpeed;
    [SerializeField] private float jointSpring;
    [SerializeField] private float jointDamper;
    [SerializeField] private float jointMassScale;
    [SerializeField] private LayerMask swingableMask;
    private Vector3 swingPoint;
    private Vector3 currentSwingPosition;
    private SpringJoint joint;

    [Header("Swing Rope Animation")]
    [SerializeField] private int quality;
    [SerializeField] private float damper;
    [SerializeField] private float strength;
    [SerializeField] private float velocity;
    [SerializeField] private float waveCount;
    [SerializeField] private float waveHeight;
    [SerializeField] private AnimationCurve effectCurve;

    [Header("Swing Prediction")]
    [SerializeField] private Transform predictionObj;
    [SerializeField] private float predictionRadius;
    private RaycastHit predictionHit;

    [Header("Ziplining")]
    [SerializeField] private float ziplineCheckRadius;
    [SerializeField] private float ziplineExitForce;
    [SerializeField] private LayerMask ziplineMask;
    private Zipline currZipline;

    [Header("Pausing")]
    private bool gamePaused;

    [Header("Headbob")]
    [SerializeField] private float walkBobSpeed;
    [SerializeField] private float walkBobAmount;
    [SerializeField] private float sprintBobSpeed;
    [SerializeField] private float sprintBobAmount;
    [SerializeField] private float crouchBobSpeed;
    [SerializeField] private float crouchBobAmount;
    [SerializeField] private float ziplineBobSpeed;
    [SerializeField] private float ziplineBobAmount;
    private Vector3 startCameraPos;
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

    [Header("Death")]
    [SerializeField] private string deathZoneTag;

    [Header("Keybinds")]
    [SerializeField] private KeyCode sprintKey;
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private KeyCode slideKey;
    [SerializeField] private KeyCode upwardsWallRunKey;
    [SerializeField] private KeyCode downwardsWallRunKey;
    [SerializeField] private KeyCode cableExtendKey;
    [SerializeField] private KeyCode interactKey;
    [SerializeField] private KeyCode resetKey;
    [SerializeField] private KeyCode pauseKey;

    public enum MovementState {

        None, Walking, Sprinting, Crouching, Sliding, WallRunning, Swinging, Ziplining, Air

    }

    public enum SlopeType {

        None, ValidUp, ValidDown, Invalid

    }

    private void Awake() {

        gameManager = FindObjectOfType<GameManager>();
        gameUIController = FindObjectOfType<GameUIController>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
        spring = new Spring();

        rb.freezeRotation = true;
        spring.SetTarget(0);

        movementState = MovementState.None;

        jumpReady = true;
        canWallRun = true;

        startCameraFOV = camera.fieldOfView;
        startCameraZTilt = camera.transform.localRotation.z;

        startScale = transform.localScale.y;
        startHeight = GetComponent<CapsuleCollider>().height;
        startCameraPos = cameraPos.localPosition;

        predictionObj.gameObject.SetActive(false);

        gravityCounterForce = initialGravityCounterForce;

        Level level = gameManager.GetCurrentLevel();
        walkEnabled = levelWalkEnabled = level.walkEnabled;
        sprintEnabled = levelSprintEnabled = level.sprintEnabled;
        jumpEnabled = levelJumpEnabled = level.jumpEnabled;
        crouchEnabled = levelCrouchEnabled = level.crouchEnabled;
        slideEnabled = levelSlideEnabled = level.slideEnabled;
        wallRunEnabled = levelWallRunEnabled = level.wallRunEnabled;
        swingEnabled = levelSwingEnabled = level.swingEnabled;
        ziplineEnabled = levelZiplineEnabled = level.ziplineEnabled;

    }

    private void Update() {

        if (lookEnabled) {

            float mouseX = Input.GetAxisRaw("Mouse X") * xSensitivity * 10f * Time.fixedDeltaTime;
            float mouseY = Input.GetAxisRaw("Mouse Y") * ySensitivity * 10f * Time.fixedDeltaTime;

            yRotation += mouseX;

            if (movementState != MovementState.WallRunning) {

                transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

            } else {

                Vector3 rotation = Quaternion.LookRotation(wallForward, Vector3.up).eulerAngles;

                if (lookRotationLerpCoroutine == null) {

                    if (wallLeft)
                        yRotation = Mathf.Clamp(yRotation, rotation.y - wallSideCameraClamp, rotation.y + wallOutsideCameraClamp);
                    else
                        yRotation = Mathf.Clamp(yRotation, rotation.y - wallOutsideCameraClamp, rotation.y + wallSideCameraClamp);

                }
            }

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -topCameraClamp, bottomCameraClamp);

            cameraPos.rotation = Quaternion.Euler(xRotation, yRotation, 0f);

        }

        isGrounded = Physics.CheckSphere(feet.position, groundCheckRadius, environmentMask);
        animator.SetBool("isGrounded", isGrounded);

        if (isGrounded && lastWall != null)
            lastWall = null;

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        ControlSpeed();
        speedText.text = "Speed: " + (Mathf.Round(rb.velocity.magnitude * 100f) / 100f);

        if (Input.GetKeyDown(jumpKey) && jumpReady && jumpEnabled && (isGrounded || movementState == MovementState.Ziplining)) {

            if (movementState == MovementState.Sliding) {

                StopSlide();
                slideQueued = true;

            }

            jumpReady = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);

        }

        HandleMovementState();
        HandleHeadbob();

        if (uncrouchQueued) {

            foreach (Transform checker in obstacleCheckers) {

                if (!Physics.Raycast(checker.position, Vector3.up, startHeight + uncrouchMinClearing, environmentMask)) {

                    Uncrouch();
                    break;

                }
            }
        }

        if (slideQueued && jumpReady && Input.GetKey(slideKey) && isGrounded && movementState == MovementState.Air) {

            StartSlide();
            slideQueued = false;

        }

        if (Input.GetKeyDown(slideKey) && movementState != MovementState.Crouching && movementState != MovementState.Swinging && movementState != MovementState.WallRunning) {

            if ((movementState == MovementState.Sprinting || movementState == MovementState.Air) && slideEnabled)
                StartSlide();
            else if (crouchEnabled)
                Crouch();

        } else if (Input.GetKeyUp(slideKey)) {

            if (movementState == MovementState.Sliding)
                StopSlide();
            else if (movementState == MovementState.Crouching)
                Uncrouch();

        }

        CheckWall();
        HandleWallRunState();

        if (Input.GetMouseButtonDown(1) && swingEnabled && movementState != MovementState.WallRunning)
            StartSwing();

        if (Input.GetMouseButtonUp(1) && movementState == MovementState.Swinging)
            StopSwing();

        CheckSwingPoints();

        Collider[] colliders = Physics.OverlapSphere(transform.position, ziplineCheckRadius, ziplineMask);

        if (colliders.Length > 0 && movementState != MovementState.Ziplining) {

            if (ziplineEnabled) {

                if (Input.GetKeyDown(interactKey)) {

                    currZipline = colliders[0].GetComponent<Zipline>();
                    movementState = MovementState.Ziplining;
                    currZipline.StartZipline();

                }

                gameUIController.FadeInInteractIcon();

            }
        } else {

            gameUIController.FadeOutInteractIcon();

        }

        if (isGrounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0f;

        if (Input.GetKeyDown(resetKey))
            gameManager.KillPlayer();

        if (Input.GetKeyDown(pauseKey)) {

            if (gamePaused) {

                gameUIController.ResumeGame();
                EnableAllMovement();
                EnableLook();

            } else {

                gameUIController.PauseGame();
                DisableAllMovement();
                DisableLook();

            }

            gamePaused = !gamePaused;

        }
    }

    private void FixedUpdate() {

        if (movementState == MovementState.Ziplining)
            return;

        movementDirection = transform.forward * verticalInput + transform.right * horizontalInput;

        if (!firstMovementDetected && movementDirection != Vector3.zero) {

            gameManager.StartTimer();
            firstMovementDetected = true;

        }

        if (movementState == MovementState.Swinging && joint != null) {

            HandleSwingMovement();

        } else if (movementState == MovementState.WallRunning) {

            HandleWallRunMovement();

        } else if (CheckSlope() == SlopeType.ValidUp || CheckSlope() == SlopeType.ValidDown && !exitingSlope) {

            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 30f, ForceMode.Force);

            if (rb.velocity.y > 0f)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);

        } else if (CheckSlope() == SlopeType.Invalid) {

            rb.AddForce(Vector3.Cross(slopeHit.normal, Vector3.Cross(slopeHit.normal, Vector3.up)) * 500f, ForceMode.Acceleration);

        } else if (isGrounded && movementState != MovementState.None) {

            rb.AddForce(movementDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        } else if (movementState != MovementState.None) {

            rb.AddForce(movementDirection.normalized * moveSpeed * airMultiplier * 10f, ForceMode.Force);

        }

        if (movementState != MovementState.WallRunning)
            rb.useGravity = CheckSlope() == SlopeType.None || CheckSlope() == SlopeType.Invalid;

    }

    private void LateUpdate() {

        DrawRope();

    }

    private void OnTriggerEnter(Collider collider) {

        if (collider.CompareTag(deathZoneTag)) {

            gameManager.KillPlayer();

        }
    }

    private void StartLerpCameraFOV(float targetFOV) {

        if (lerpCamFOVCoroutine != null)
            StopCoroutine(lerpCamFOVCoroutine);

        lerpCamFOVCoroutine = StartCoroutine(LerpCameraFOV(camera.fieldOfView, targetFOV));

    }

    private IEnumerator LerpCameraFOV(float startFOV, float targetFOV) {

        float currentTime = 0f;

        while (currentTime < camFOVLerpDuration) {

            currentTime += Time.deltaTime;
            camera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, currentTime / camFOVLerpDuration);
            yield return null;

        }

        camera.fieldOfView = targetFOV;
        lerpCamFOVCoroutine = null;

    }

    private void StartLerpCameraTilt(float targetZTilt) {

        if (lerpCamTiltCoroutine != null)
            StopCoroutine(lerpCamTiltCoroutine);

        lerpCamTiltCoroutine = StartCoroutine(LerpCameraTilt(camera.transform.localRotation.z, targetZTilt));

    }

    private IEnumerator LerpCameraTilt(float startZTilt, float targetZTilt) {

        float currentTime = 0f;

        while (currentTime < camTiltLerpDuration) {

            currentTime += Time.deltaTime;
            camera.transform.localRotation = Quaternion.Euler(camera.transform.localRotation.x, camera.transform.localRotation.y, Mathf.Lerp(startZTilt, targetZTilt, currentTime / camTiltLerpDuration));
            yield return null;

        }

        camera.transform.localRotation = Quaternion.Euler(camera.transform.localRotation.x, camera.transform.localRotation.y, targetZTilt);
        lerpCamTiltCoroutine = null;

    }

    private void HandleMovementState() {

        if (movementState == MovementState.Ziplining && ziplineEnabled) {

            animator.SetBool("isZiplining", true);
            animator.SetBool("isWalking", false);
            animator.SetBool("isSprinting", false);
            animator.SetBool("isCrouching", false);
            animator.SetBool("isSliding", false);
            animator.SetBool("isWallRunningLeft", false);
            animator.SetBool("isWallRunningRight", false);
            movementState = MovementState.Ziplining;

        } else if (movementState == MovementState.Swinging && swingEnabled) {

            movementState = MovementState.Swinging;
            desiredMoveSpeed = maxSwingSpeed;

        } else if (movementState == MovementState.WallRunning && wallRunEnabled) {

            movementState = MovementState.WallRunning;
            desiredMoveSpeed = wallRunSpeed;

        } else if (movementState == MovementState.Sliding && slideEnabled) {

            movementState = MovementState.Sliding;

            if (CheckSlope() == SlopeType.None || CheckSlope() == SlopeType.ValidUp && isGrounded)
                slideTimer -= Time.deltaTime;
            else if (CheckSlope() == SlopeType.ValidDown)
                desiredMoveSpeed = maxSlideSpeed;
            else
                desiredMoveSpeed = sprintSpeed;

            if (CheckSlope() == SlopeType.ValidUp)
                desiredMoveSpeed = maxSlideSpeed * upwardsSlideFactor;

            if (slideTimer <= 0f)
                StopSlide();

        } else if (movementState == MovementState.Crouching && crouchEnabled) {

            movementState = MovementState.Crouching;
            desiredMoveSpeed = crouchSpeed;

        } else if (isGrounded && movementDirection == Vector3.zero) {

            animator.SetBool("isWalking", false);
            animator.SetBool("isSprinting", false);
            animator.SetBool("isCrouching", false);
            animator.SetBool("isSliding", false);
            animator.SetBool("isWallRunningLeft", false);
            animator.SetBool("isWallRunningRight", false);
            animator.SetBool("isZiplining", false);
            movementState = MovementState.None;
            desiredMoveSpeed = walkSpeed;

        } else if (isGrounded && Input.GetKey(sprintKey) && sprintEnabled) {

            animator.SetBool("isSprinting", true);
            animator.SetBool("isWalking", false);
            animator.SetBool("isCrouching", false);
            animator.SetBool("isSliding", false);
            animator.SetBool("isWallRunningLeft", false);
            animator.SetBool("isWallRunningRight", false);
            animator.SetBool("isZiplining", false);
            movementState = MovementState.Sprinting;
            desiredMoveSpeed = sprintSpeed;

        } else if (isGrounded && walkEnabled) {

            animator.SetBool("isWalking", true);
            animator.SetBool("isSprinting", false);
            animator.SetBool("isCrouching", false);
            animator.SetBool("isSliding", false);
            animator.SetBool("isWallRunningLeft", false);
            animator.SetBool("isWallRunningRight", false);
            animator.SetBool("isZiplining", false);
            movementState = MovementState.Walking;
            desiredMoveSpeed = walkSpeed;

        } else if (!isGrounded && rb.velocity.magnitude > 0.01f) {

            animator.SetBool("isWalking", false);
            animator.SetBool("isSprinting", false);
            animator.SetBool("isCrouching", false);
            animator.SetBool("isSliding", false);
            animator.SetBool("isWallRunningLeft", false);
            animator.SetBool("isWallRunningRight", false);
            animator.SetBool("isZiplining", false);
            movementState = MovementState.Air;

        } else {

            animator.SetBool("isWalking", false);
            animator.SetBool("isSprinting", false);
            animator.SetBool("isCrouching", false);
            animator.SetBool("isSliding", false);
            animator.SetBool("isWallRunningLeft", false);
            animator.SetBool("isWallRunningRight", false);
            animator.SetBool("isZiplining", false);
            movementState = MovementState.None;

        }

        if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > sprintSpeed - walkSpeed && moveSpeed != 0f) {

            if (moveSpeedCoroutine != null)
                StopCoroutine(moveSpeedCoroutine);

            moveSpeedCoroutine = StartCoroutine(LerpMoveSpeed());

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

            if (CheckSlope() == SlopeType.ValidDown) {

                timer += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * (1f + (Vector3.Angle(Vector3.up, slopeHit.normal) / 90f));

            } else {

                timer += Time.deltaTime * speedIncreaseMultiplier;

            }

            yield return null;

        }

        moveSpeed = desiredMoveSpeed;

    }

    private void ControlSpeed() {

        if (CheckSlope() == SlopeType.ValidUp || CheckSlope() == SlopeType.ValidDown || CheckSlope() == SlopeType.Invalid && !exitingSlope) {

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

        if (movementState == MovementState.Ziplining) {

            currZipline.ResetZipline(false);
            jumpReady = false;
            exitingSlope = true;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            return;

        }

        jumpReady = false;
        exitingSlope = true;
        // rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

    }

    private void ResetJump() {

        jumpReady = true;
        exitingSlope = false;

    }

    private void Crouch() {

        if (uncrouchQueued)
            return;

        movementState = MovementState.Crouching;
        crosshair.gameObject.SetActive(true);
        animator.SetBool("isCrouching", true);
        animator.SetBool("isWalking", false);
        animator.SetBool("isSprinting", false);
        animator.SetBool("isSliding", false);
        ForceCrouch();

    }

    private void ForceCrouch() {

        transform.localScale = new Vector3(transform.localScale.x, crouchScale, transform.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

    }

    private void Uncrouch() {

        foreach (Transform checker in obstacleCheckers) {

            if (Physics.Raycast(checker.position, Vector3.up, startHeight + uncrouchMinClearing, environmentMask)) {

                uncrouchQueued = true;
                return;

            }
        }

        movementState = MovementState.None;
        animator.SetBool("isCrouching", false);
        uncrouchQueued = false;
        transform.localScale = new Vector3(transform.localScale.x, startScale, transform.localScale.z);

    }

    private void StartSlide() {

        movementState = MovementState.Sliding;
        crosshair.gameObject.SetActive(true);
        animator.SetBool("isSliding", true);
        ForceCrouch();
        slideTimer = maxSlideTime;

    }

    private void StopSlide() {

        movementState = MovementState.None;
        animator.SetBool("isSliding", false);
        Uncrouch();

    }

    private void CheckWall() {

        wallRight = Physics.Raycast(transform.position, transform.right, out rightWallHit, wallCheckDistance, wallMask);
        wallLeft = Physics.Raycast(transform.position, -transform.right, out leftWallHit, wallCheckDistance, wallMask);

    }

    private bool CanWallRun() {

        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, environmentMask);

    }

    private void HandleWallRunState() {

        wallRunningUpwards = Input.GetKey(upwardsWallRunKey);
        wallRunningDownwards = Input.GetKey(downwardsWallRunKey);

        if ((wallLeft || wallRight) && verticalInput > 0 && CanWallRun() && !exitingWall && !isGrounded) {

            if (movementState != MovementState.WallRunning)
                StartWallRun();

            if (useWallRunTimer) {

                if (wallRunTimer > 0f)
                    wallRunTimer -= Time.deltaTime;

                if (wallRunTimer <= 0f && movementState == MovementState.WallRunning)
                    WallJump();

            }

            if (Input.GetKeyDown(jumpKey) && jumpEnabled)
                WallJump();

        } else if (exitingWall) {

            if (movementState == MovementState.WallRunning)
                StopWallRun();

            if (exitWallTimer > 0f)
                exitWallTimer -= Time.deltaTime;

            if (exitWallTimer <= 0f)
                exitingWall = false;

        } else if (movementState == MovementState.WallRunning) {

            StopWallRun();

        }
    }

    private void StartWallRun() {

        if (!canWallRun || (wallLeft && lastWall == leftWallHit.transform) || (wallRight && lastWall == rightWallHit.transform))
            return;

        if (movementState == MovementState.Crouching)
            Uncrouch();

        if (movementState == MovementState.Sliding)
            StopSlide();

        if (movementState == MovementState.Swinging)
            StopSwing();

        movementState = MovementState.WallRunning;

        if (wallLeft) {

            animator.SetBool("isWallRunningLeft", true);
            wallNormal = leftWallHit.normal;
            lastWall = leftWallHit.transform;

        } else if (wallRight) {

            animator.SetBool("isWallRunningRight", true);
            wallNormal = rightWallHit.normal;
            lastWall = rightWallHit.transform;

        }

        wallForward = lastWall.forward;

        if ((transform.forward - wallForward).magnitude > (transform.forward - -wallForward).magnitude)
            wallForward = -wallForward;

        Quaternion rotation = Quaternion.LookRotation(wallForward, Vector3.up);

        if (yRotation < rotation.eulerAngles.y - wallSideCameraClamp || yRotation > rotation.eulerAngles.y + wallOutsideCameraClamp)
            lookRotationLerpCoroutine = StartCoroutine(LerpLookRotation(rotation));

        rb.useGravity = false;
        crosshair.gameObject.SetActive(true);

        if (useWallRunTimer)
            wallRunTimer = maxWallRunTime;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (useWallRunGravity)
            gravityDelayCoroutine = StartCoroutine(HandleWallRunGravity());

        StartLerpCameraFOV(wallRunCameraFOV);

        if (wallLeft)
            StartLerpCameraTilt(-wallRunCamTilt);

        if (wallRight)
            StartLerpCameraTilt(wallRunCamTilt);

    }

    private IEnumerator HandleWallRunGravity() {

        yield return new WaitForSeconds(gravityDelay);

        while (movementState == MovementState.WallRunning) {

            rb.useGravity = true;
            rb.AddForce(transform.up * gravityCounterForce, ForceMode.Force);
            yield return new WaitForSeconds(gravityDecrementRate);
            rb.useGravity = false;
            gravityCounterForce -= gravityDecrementAmount;

        }
    }

    private void HandleWallRunMovement() {

        if (rb.velocity.magnitude < 0.1f) {

            StopWallRun();
            return;

        }

        Vector3 direction = wallForward * wallRunForce;
        direction.y /= (200f / 60f);
        rb.AddForce(direction, ForceMode.Force);

        if (wallRunningUpwards)
            rb.velocity = new Vector3(rb.velocity.x, wallClimbSpeed, rb.velocity.z);

        if (wallRunningDownwards)
            rb.velocity = new Vector3(rb.velocity.x, -wallClimbSpeed, rb.velocity.z);

        if (!(wallLeft && horizontalInput > 0f) && !(wallRight && horizontalInput < 0f))
            rb.AddForce(-wallNormal * wallNormalForce, ForceMode.Force);

    }

    private IEnumerator LerpLookRotation(Quaternion targetRotation) {

        float currentTime = 0f;
        Quaternion startRotation = Quaternion.Euler(0f, yRotation, 0f);

        while (currentTime < lookRotationLerpDuration) {

            currentTime += Time.deltaTime;
            yRotation = Quaternion.Lerp(startRotation, targetRotation, currentTime / lookRotationLerpDuration).eulerAngles.y;
            yield return null;

        }

        yRotation = targetRotation.eulerAngles.y;
        lookRotationLerpCoroutine = null;

    }

    private void WallJump() {

        if (movementState != MovementState.WallRunning)
            return;

        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
        Vector3 force = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(force, ForceMode.Impulse);

    }

    private void StopWallRun() {

        if (lookRotationLerpCoroutine != null)
            StopCoroutine(lookRotationLerpCoroutine);

        movementState = MovementState.None;

        animator.SetBool("isWallRunningLeft", false);
        animator.SetBool("isWallRunningRight", false);

        StartLerpCameraFOV(startCameraFOV);
        StartLerpCameraTilt(startCameraZTilt);

        if (gravityDelayCoroutine != null)
            StopCoroutine(gravityDelayCoroutine);

    }

    public Transform GetSwingMuzzle() {

        return muzzle;

    }

    public Vector3 GetSwingPoint() {

        return swingPoint;

    }

    private void CheckSwingPoints() {

        if (joint != null || movementState == MovementState.WallRunning || movementState == MovementState.Swinging)
            return;

        RaycastHit raycastHit;
        Physics.Raycast(cameraPos.position, cameraPos.forward, out raycastHit, maxSwingDistance, swingableMask);

        if (raycastHit.point != Vector3.zero) {

            // Direct Hit
            predictionHit = raycastHit;
            crosshair.gameObject.SetActive(false);

        } else {

            RaycastHit sphereCastHit;
            Physics.SphereCast(cameraPos.position, predictionRadius, cameraPos.forward, out sphereCastHit, maxSwingDistance, swingableMask);

            if (sphereCastHit.point != Vector3.zero) {

                // Indirect / Predicted Hit
                predictionHit = sphereCastHit;
                crosshair.gameObject.SetActive(true);

            } else {

                // Miss
                predictionHit.point = Vector3.zero;
                crosshair.gameObject.SetActive(true);

            }
        }

        Vector3 hitPoint = predictionHit.point;

        if (hitPoint != Vector3.zero && swingEnabled) {

            predictionObj.gameObject.SetActive(true);
            predictionObj.position = hitPoint + (predictionHit.normal * 0.002f);
            predictionObj.rotation = Quaternion.LookRotation(-predictionHit.normal, transform.up);

        } else {

            predictionObj.gameObject.SetActive(false);

        }
    }

    private void StartSwing() {

        if (predictionHit.point == Vector3.zero)
            return;

        movementState = MovementState.Swinging;

        predictionObj.gameObject.SetActive(false);
        crosshair.gameObject.SetActive(true);

        swingPoint = predictionHit.point;
        joint = gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = swingPoint;

        float distanceFromPoint = Vector3.Distance(transform.position, swingPoint);

        joint.maxDistance = distanceFromPoint * 0.8f;
        joint.minDistance = distanceFromPoint * 0.25f;

        joint.spring = jointSpring;
        joint.damper = jointDamper;
        joint.massScale = jointMassScale;

    }

    private void DrawRope() {

        if (movementState != MovementState.Swinging) {

            currentSwingPosition = muzzle.position;
            spring.Reset();

            if (lineRenderer.positionCount > 0)
                lineRenderer.positionCount = 0;

            return;

        }

        if (lineRenderer.positionCount == 0) {

            spring.SetVelocity(velocity);
            lineRenderer.positionCount = quality + 1;

        }

        spring.SetDamper(damper);
        spring.SetStrength(strength);
        spring.Update(Time.deltaTime);

        Vector3 up = Quaternion.LookRotation(swingPoint - muzzle.position).normalized * Vector3.up;

        // TODO: Make 12f a variable?
        currentSwingPosition = Vector3.Lerp(currentSwingPosition, swingPoint, Time.deltaTime * 12f);

        for (int i = 0; i < quality + 1; i++) {

            var delta = i / (float) quality;
            Vector3 offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI * spring.Value * effectCurve.Evaluate(delta));

            lineRenderer.SetPosition(i, Vector3.Lerp(muzzle.position, currentSwingPosition, delta) + offset);

        }
    }

    private void HandleSwingMovement() {

        if (horizontalInput != 0f)
            rb.AddForce((horizontalInput > 0f ? transform.right : -transform.right) * horizontalThrustForce * Time.deltaTime);

        if (verticalInput > 0f)
            rb.AddForce(transform.forward * forwardThrustForce * Time.deltaTime);

        if (Input.GetKey(jumpKey) && jumpEnabled) {

            Vector3 directionToPoint = swingPoint - transform.position;
            rb.AddForce(directionToPoint.normalized * forwardThrustForce * Time.deltaTime);

            float distanceFromPoint = Vector3.Distance(transform.position, swingPoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

        }

        if (Input.GetKey(cableExtendKey)) {

            float extendedDistanceFromPoint = Vector3.Distance(transform.position, swingPoint) + cableExtendSpeed;

            joint.maxDistance = extendedDistanceFromPoint * 0.8f;
            joint.minDistance = extendedDistanceFromPoint * 0.25f;

        }
    }

    private void StopSwing() {

        movementState = MovementState.None;
        crosshair.gameObject.SetActive(true);
        Destroy(joint);

    }

    public void ResetZipline(bool jump) {

        if (jump) {

            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * ziplineExitForce, ForceMode.Impulse);

        }

        currZipline = null;
        movementState = MovementState.None;

    }

    private void HandleHeadbob() {

        if (movementState == MovementState.Ziplining) {

            timer += ziplineBobSpeed * Time.deltaTime;
            cameraPos.localPosition = new Vector3(cameraPos.localPosition.x, startCameraPos.y + Mathf.Sin(timer) * ziplineBobAmount, cameraPos.localPosition.z);
            return;

        }

        if (isGrounded) {

            if (Mathf.Abs(rb.velocity.x) > 0.1f || Mathf.Abs(rb.velocity.z) > 0.1f) {

                switch (movementState) {

                    case MovementState.Crouching:

                    timer += crouchBobSpeed * Time.deltaTime;
                    cameraPos.localPosition = new Vector3(cameraPos.localPosition.x, startCameraPos.y + Mathf.Sin(timer) * crouchBobAmount, cameraPos.localPosition.z);
                    break;

                    case MovementState.Sprinting:

                    timer += sprintBobSpeed * Time.deltaTime;
                    cameraPos.localPosition = new Vector3(cameraPos.localPosition.x, startCameraPos.y + Mathf.Sin(timer) * sprintBobAmount, cameraPos.localPosition.z);
                    break;

                    case MovementState.Walking:

                    timer += walkBobSpeed * Time.deltaTime;
                    cameraPos.localPosition = new Vector3(cameraPos.localPosition.x, startCameraPos.y + Mathf.Sin(timer) * walkBobAmount, cameraPos.localPosition.z);
                    break;

                }
            }
        }
    }

    private SlopeType CheckSlope() {

        if (Physics.Raycast(feet.position, Vector3.down, out slopeHit, slopeCheckDistance)) {

            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);

            if (angle != 0f) {

                if (angle <= maxSlopeAngle) {

                    float dot = Vector3.Dot(slopeHit.normal, transform.forward);

                    if (dot < 0f)
                        return SlopeType.ValidUp;
                    else if (dot > 0f)
                        return SlopeType.ValidDown;

                } else {

                    return SlopeType.Invalid;

                }
            }
        }

        return SlopeType.None;

    }

    private Vector3 GetSlopeMoveDirection() {

        return Vector3.ProjectOnPlane(movementDirection, slopeHit.normal).normalized;

    }

    public float GetPlayerHeight() {

        return startHeight;

    }

    public void ResetVelocity() {

        desiredMoveSpeed = 0f;
        rb.velocity = Vector3.zero;

    }

    public void SetLookRotations(float xRotation, float yRotation) {

        this.xRotation = xRotation;
        this.yRotation = yRotation;

    }

    public void EnableAllMovement() {

        if (levelWalkEnabled)
            walkEnabled = true;

        if (levelSprintEnabled)
            sprintEnabled = true;

        if (levelJumpEnabled)
            jumpEnabled = true;

        if (levelCrouchEnabled)
            crouchEnabled = true;

        if (levelSlideEnabled)
            slideEnabled = true;

        if (levelWallRunEnabled)
            wallRunEnabled = true;

        if (levelSwingEnabled)
            swingEnabled = true;

        if (levelZiplineEnabled)
            ziplineEnabled = true;

    }

    public void DisableAllMovement() {

        if (levelWalkEnabled)
            walkEnabled = false;

        if (levelSprintEnabled)
            sprintEnabled = false;

        if (levelJumpEnabled)
            jumpEnabled = false;

        if (levelCrouchEnabled)
            crouchEnabled = false;

        if (levelSlideEnabled)
            slideEnabled = false;

        if (levelWallRunEnabled)
            wallRunEnabled = false;

        if (levelSwingEnabled)
            swingEnabled = false;

        if (levelZiplineEnabled)
            ziplineEnabled = false;

        movementState = MovementState.None;
        animator.SetBool("isWalking", false);
        animator.SetBool("isSprinting", false);
        animator.SetBool("isCrouching", false);
        animator.SetBool("isSliding", false);
        animator.SetBool("isWallRunningLeft", false);
        animator.SetBool("isWallRunningRight", false);
        animator.SetBool("isZiplining", false);
        animator.SetBool("isGrounded", true);

    }

    public void EnableLook() {

        lookEnabled = true;

    }

    public void DisableLook() {

        lookEnabled = false;

    }

    public void EnableWalk() {

        walkEnabled = true;

    }

    public void DisableWalk() {

        walkEnabled = false;

    }

    public void EnableSprint() {

        sprintEnabled = true;

    }

    public void DisableSprint() {

        sprintEnabled = false;

    }

    public void EnableJump() {

        jumpEnabled = true;

    }

    public void DisableJump() {

        jumpEnabled = false;

    }

    private void EnableCrouch() {

        crouchEnabled = true;

    }

    private void DisableCrouch() {

        crouchEnabled = false;

    }

    public void EnableSlide() {

        slideEnabled = true;

    }

    public void DisableSlide() {

        slideEnabled = false;

    }

    public void EnableWallRun() {

        wallRunEnabled = true;

    }

    public void DisableWallRun() {

        wallRunEnabled = false;

    }

    public void EnableSwing() {

        swingEnabled = true;

    }

    public void DisableSwing() {

        swingEnabled = false;

    }

    public void EnableZipline() {

        ziplineEnabled = true;

    }

    public void DisableZipline() {

        ziplineEnabled = false;

    }
}
