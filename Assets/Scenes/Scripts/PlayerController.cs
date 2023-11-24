using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(RigBuilder))]
public class PlayerController : MonoBehaviour
{

    #region VARIABLES
    [Header("References")]
    [SerializeField] private Transform cameraPos;
    [SerializeField] private Transform muzzle;
    [SerializeField] private Transform[] obstacleCheckers;
    [SerializeField] private Animator animator;
    private GameManager gameManager;
    private GameAudioManager audioManager;
    private GameUIController UIController;
    private Camera mainCamera;
    private Rigidbody rb;
    private LineRenderer lineRenderer;
    private Spring spring;
    private float startScale;
    private float startHeight;
    private Vector3 startGravity;

    [Header("Toggles")]
    private bool levelLookEnabled;
    private bool levelWalkEnabled;
    private bool levelSprintEnabled;
    private bool levelJumpEnabled;
    private bool levelCrouchEnabled;
    private bool levelSlideEnabled;
    private bool levelWallRunEnabled;
    private bool levelSwingEnabled;
    private bool levelZiplineEnabled;
    private bool levelGrabEnabled;
    private bool lookEnabled;
    private bool walkEnabled;
    private bool sprintEnabled;
    private bool jumpEnabled;
    private bool crouchEnabled;
    private bool slideEnabled;
    private bool wallRunEnabled;
    private bool swingEnabled;
    private bool ziplineEnabled;
    private bool grabEnabled;

    [Header("Rigs")]
    [SerializeField] private int leftHandIKRigIndex;
    [SerializeField] private int rightHandIKRigIndex;
    [SerializeField] private int rightHandFollowRigIndex;
    [SerializeField] private int leftFootIKRigIndex;
    [SerializeField] private int rightFootIKRigIndex;
    private RigBuilder rigBuilder;

    [Header("Looking")]
    [SerializeField][Range(0f, 100f)] private float xSensitivity;
    [SerializeField][Range(0f, 100f)] private float ySensitivity;
    [SerializeField][Range(0f, 90f)] private float topCameraClamp;
    [SerializeField][Range(0f, 90f)] private float bottomCameraClamp;
    [SerializeField][Range(0f, 90f)] private float wallSideCameraClamp;
    [SerializeField][Range(0f, 90f)] private float wallOutsideCameraClamp;
    private float xRotation;
    private float yRotation;

    [Header("Movement")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float speedIncreaseMultiplier;
    [SerializeField] private float slopeIncreaseMultiplier;
    [SerializeField] private float landGroundFriction;
    [SerializeField] private float minMovementVelocity;
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
    [SerializeField] private float crouchDownwardsForce;
    [SerializeField] private float crouchScale;
    [SerializeField] private float uncrouchMinClearing;
    private bool uncrouchQueued;

    [Header("Sliding")]
    [SerializeField] private float maxSlideSpeed;
    [SerializeField] private float slideDownwardsForce;
    [SerializeField] private float slideForwardForce;
    [SerializeField] private float maxSlideTime;
    [SerializeField] private float upwardsSlideFactor;
    private float slideTimer;
    private bool slideQueued;

    [Header("Wall Running")]
    [SerializeField] private float wallRunSpeed;
    [SerializeField] private float wallRunForce;
    [SerializeField] private float wallRunNormalForce;
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private float exitWallTime;
    private float exitWallTimer;
    private Transform lastWall;
    private bool exitingWall;
    private Vector3 wallForward;

    [Header("Wall Run Timer")]
    [SerializeField] private bool wallRunTimerEnabled;
    [SerializeField] private float maxWallRunDuration;
    private float wallRunTimer;

    [Header("Wall Run Scaling")]
    [SerializeField] private bool wallRunScalingEnabled;
    [SerializeField] private float wallScaleSpeed;
    private bool runningDownWall;
    private bool runningUpWall;

    [Header("Wall Jumping")]
    [SerializeField] private float wallJumpUpForce;
    [SerializeField] private float wallJumpSideForce;

    [Header("Wall Detection")]
    [SerializeField] private float wallCheckRadius;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float minJumpHeight;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft;
    private bool wallRight;

    [Header("Wall Run Camera Effects")]
    [SerializeField] private float wallRunFOV;
    [SerializeField] private float wallRunFOVLerpDuration;
    [SerializeField] private float wallRunTilt;
    [SerializeField] private float wallRunTiltLerpDuration;
    [SerializeField] private float lookRotationLerpDuration;
    private float startFOV;
    private Vector3 startTilt;
    private Coroutine wallRunFOVCoroutine;
    private Coroutine wallRunTiltCoroutine;
    private Coroutine lookRotationLerpCoroutine;

    [Header("Wall Run Gravity")]
    [SerializeField] private bool useWallRunGravity;
    [SerializeField] private float gravityDelay;
    [SerializeField] private float gravityIncrementRate;
    private Coroutine gravityDelayCoroutine;

    [Header("Swinging")]
    [SerializeField] private float maxSwingSpeed;
    [SerializeField] private float maxSwingDistance;
    [SerializeField] private float horizontalThrustForce;
    [SerializeField] private float forwardThrustForce;
    [SerializeField] private float cableExtendSpeed;
    [SerializeField] private float jointSpring;
    [SerializeField] private float jointDamper;
    [SerializeField] private float jointMassScale;
    [SerializeField] private Transform swingIKTarget;
    [SerializeField] private Transform swingPoint;
    [SerializeField] private LayerMask swingMask;
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
    [SerializeField] private float predictionRadius;
    private Transform predictionObj;
    private RaycastHit predictionHit;

    [Header("Ziplining")]
    [SerializeField] private float ziplineCheckRadius;
    [SerializeField] private float ziplineExitUpwardsFactor;
    [SerializeField] private float ziplineExitForwardFactor;
    [SerializeField] private LayerMask ziplineMask;
    private Zipline currZipline;

    [Header("Elevator")]
    private bool inElevator;
    private bool elevatorMoving;

    [Header("Grabbing")]
    [SerializeField] private float grabDistance;
    [SerializeField] private Transform grabPoint;
    private Rigidbody currGrabbedObj;
    private string currObjTag;

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

    [Header("Interacting")]
    [SerializeField] private float interactDistance;

    [Header("Effects")]
    private List<Effect> currentEffects;
    private bool inGravityZone;

    [Header("Ground Check")]
    [SerializeField] private Transform feet;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask environmentMask;
    private bool isGrounded;
    private bool lastIsGrounded;

    [Header("Slope Handling")]
    [SerializeField] private float maxSlopeAngle;
    [SerializeField] private float slopeCheckDistance;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Drag Control")]
    [SerializeField] private float groundDrag;

    [Header("Keybinds")]
    [SerializeField] private KeyCode sprintKey;
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private KeyCode slideKey;
    [SerializeField] private KeyCode swingKey;
    [SerializeField] private KeyCode upwardsWallRunKey;
    [SerializeField] private KeyCode downwardsWallRunKey;
    [SerializeField] private KeyCode cableExtendKey;
    [SerializeField] private KeyCode interactKey;
    [SerializeField] private KeyCode interactKeyAlt;
    [SerializeField] private KeyCode resetKey;
    [SerializeField] private KeyCode pauseKey;
    [SerializeField] private KeyCode grabKey;
    #endregion

    #region ENUMS
    public enum MovementState
    {

        None, Walking, Sprinting, Crouching, Sliding, WallRunningLeft, WallRunningRight, Swinging, Ziplining, Air, Killed

    }

    public enum SlopeType
    {

        None, ValidUp, ValidDown, Invalid

    }
    #endregion

    #region CORE
    private void Awake()
    {

        gameManager = FindObjectOfType<GameManager>();
        audioManager = FindObjectOfType<GameAudioManager>();
        UIController = FindObjectOfType<GameUIController>();
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
        rigBuilder = GetComponent<RigBuilder>();
        predictionObj = FindObjectOfType<SwingPredictor>().transform;
        spring = new Spring();
        startGravity = Physics.gravity;

        rigBuilder.layers[leftHandIKRigIndex].active = true;
        rigBuilder.layers[rightHandIKRigIndex].active = true;
        rigBuilder.layers[rightHandFollowRigIndex].active = false;
        rigBuilder.layers[leftFootIKRigIndex].active = true;
        rigBuilder.layers[rightFootIKRigIndex].active = true;

        rb.freezeRotation = true;
        spring.SetTarget(0);

        movementState = MovementState.None;

        jumpReady = true;

        startScale = transform.localScale.y;
        startHeight = GetComponent<CapsuleCollider>().height;
        startCameraPos = cameraPos.localPosition;

        predictionObj.gameObject.SetActive(false);

        currentEffects = new List<Effect>();

        Level level = gameManager.GetCurrentLevel();
        lookEnabled = levelLookEnabled = level.GetLookEnabled();
        walkEnabled = levelWalkEnabled = level.GetWalkEnabled();
        sprintEnabled = levelSprintEnabled = level.GetSprintEnabled();
        jumpEnabled = levelJumpEnabled = level.GetJumpEnabled();
        crouchEnabled = levelCrouchEnabled = level.GetCrouchEnabled();
        slideEnabled = levelSlideEnabled = level.GetSlideEnabled();
        wallRunEnabled = levelWallRunEnabled = level.GetWallRunEnabled();
        swingEnabled = levelSwingEnabled = level.GetSwingEnabled();
        ziplineEnabled = levelZiplineEnabled = level.GetZiplineEnabled();
        grabEnabled = levelGrabEnabled = level.GetGrabEnabled();

        startFOV = mainCamera.fieldOfView;
        startTilt = mainCamera.transform.localRotation.eulerAngles;

    }

    private void Update()
    {

        // looking
        if (lookEnabled)
        {

            float mouseX = Input.GetAxisRaw("Mouse X") * xSensitivity * 10f * Time.fixedDeltaTime;
            float mouseY = Input.GetAxisRaw("Mouse Y") * ySensitivity * 10f * Time.fixedDeltaTime;

            yRotation += mouseX;

            if (movementState != MovementState.WallRunningLeft && movementState != MovementState.WallRunningRight)
            {

                transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

            }
            else
            {

                if (lookRotationLerpCoroutine == null)
                {

                    Vector3 rotation = Quaternion.LookRotation(wallForward).eulerAngles;

                    // cancel wall run if player looks out of outer clamp range (away from wall)
                    if ((wallLeft && yRotation > rotation.y + wallOutsideCameraClamp) || (wallRight && yRotation < rotation.y - wallOutsideCameraClamp))
                        StopWallRun();

                    // lock camera during wall run & follow wall
                    // stop exiting look rotation lerp coroutine if it exists
                    //if (lookRotationLerpCoroutine != null)
                    //    StopCoroutine(lookRotationLerpCoroutine);
                    //
                    // lookRotationLerpCoroutine = StartCoroutine(LerpLookRotation(Quaternion.Euler(rotation)));

                    if (wallLeft)
                        yRotation = Mathf.Clamp(yRotation, rotation.y - wallSideCameraClamp, float.MaxValue);
                    else
                        yRotation = Mathf.Clamp(yRotation, float.MinValue, rotation.y + wallSideCameraClamp);

                }
            }

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -topCameraClamp, bottomCameraClamp);

            cameraPos.rotation = Quaternion.Euler(xRotation, yRotation, 0f);

        }

        // ground check
        isGrounded = Physics.CheckSphere(feet.position, groundCheckRadius, environmentMask);
        animator.SetBool("isGrounded", isGrounded);

        // reset last wall
        if (isGrounded && lastWall)
            lastWall = null;

        // handling velocity when landing
        if (isGrounded && !lastIsGrounded)
        { // just landed

            if (movementDirection == Vector3.zero)
            {

                ResetVelocity();

            }
            else
            {

                moveSpeed /= landGroundFriction;
                desiredMoveSpeed /= landGroundFriction;

            }

            audioManager.PlaySound(GameAudioManager.GameSoundEffectType.Land);
            exitingSlope = false;

        }

        /*
        if (isGrounded && lastWall != null)
            lastWall = null;
        */

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // speed control
        ControlSpeed();

        // slide queueing
        if (slideQueued && jumpReady && Input.GetKey(slideKey) && isGrounded && movementState == MovementState.Air)
        {

            StartSlide();
            slideQueued = false;

        }

        // jumping
        if (Input.GetKeyDown(jumpKey) && jumpReady && jumpEnabled && (isGrounded || movementState == MovementState.Ziplining))
        {

            if (movementState == MovementState.Sliding)
            {

                StopSlide();
                slideQueued = true;

            }

            Jump();

        }

        HandleMovementState();
        HandleHeadbob();

        // uncrouching
        if (uncrouchQueued)
        {

            foreach (Transform checker in obstacleCheckers)
            {

                if (!Physics.Raycast(checker.position, Vector3.up, startHeight + uncrouchMinClearing, environmentMask))
                {

                    Uncrouch();
                    break;

                }
            }
        }

        // sliding
        if (Input.GetKeyDown(slideKey) && movementState != MovementState.Crouching && movementState != MovementState.Swinging && movementState != MovementState.WallRunningLeft && movementState != MovementState.WallRunningRight && movementState != MovementState.Ziplining)
        {

            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if ((movementState == MovementState.Sprinting || movementState == MovementState.Air) && slideEnabled && flatVel.magnitude >= minMovementVelocity)
                StartSlide();
            else if (crouchEnabled)
                Crouch();

        }
        else if (Input.GetKeyUp(slideKey))
        {

            if (movementState == MovementState.Sliding)
                StopSlide();
            else if (movementState == MovementState.Crouching)
                Uncrouch();

        }

        HandleWallRunState();

        // swinging
        if (swingEnabled && movementState != MovementState.WallRunningLeft && movementState != MovementState.WallRunningRight && movementState != MovementState.Ziplining)
            if (Input.GetKeyDown(swingKey))
                StartSwing();
            else
                CheckSwingPoints();

        if (Input.GetKeyUp(swingKey) && movementState == MovementState.Swinging)
            StopSwing();

        if (isGrounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0f;

        // level resetting
        if (Input.GetKeyDown(resetKey))
        {

            transform.parent = null;
            gameManager.KillPlayer();

        }

        // pausing
        if (Input.GetKeyDown(pauseKey))
        {

            if (gameManager.GetGamePaused())
                UIController.ResumeGame();
            else
                UIController.PauseGame();

        }

        // crosshair interactable checks
        RaycastHit interactableHitInfo;
        RaycastHit grabbableHitInfo;
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);

        Collider[] colliders = Physics.OverlapSphere(transform.position, ziplineCheckRadius, ziplineMask);

        if (colliders.Length > 0 && movementState != MovementState.Ziplining)
        {

            Zipline zipline = colliders[0].GetComponent<Zipline>();

            if (ziplineEnabled && zipline.CanZipline())
            {

                if (Input.GetKeyDown(interactKey))
                {

                    StopSwing();
                    currZipline = zipline;
                    movementState = MovementState.Ziplining;
                    currZipline.StartZipline();

                }

                UIController.EnableInteractCrosshair("Zipline");

            }
        }
        else if (Physics.Raycast(ray, out interactableHitInfo, interactDistance) && interactableHitInfo.transform.CompareTag("Interactable"))
        {

            if (interactableHitInfo.collider.GetComponent<Interactable>() != null)
            {

                Interactable interactable = interactableHitInfo.collider.GetComponent<Interactable>();
                UIController.EnableInteractCrosshair(interactable.interactText);

                if (Input.GetKeyDown(interactKey) || Input.GetKeyDown(interactKeyAlt))
                    interactable.BaseInteract();

            }
        }
        else if (Physics.Raycast(ray, out grabbableHitInfo, grabDistance) && grabbableHitInfo.transform.CompareTag("Grabbable") && !currGrabbedObj && grabbableHitInfo.collider.GetComponent<Rigidbody>() != null)
        {

            if (Input.GetKey(grabKey))
            {

                // grabs object
                currGrabbedObj = grabbableHitInfo.rigidbody;
                currGrabbedObj.freezeRotation = true;
                currGrabbedObj.useGravity = false;

            }
            else
            {

                // looking at object but isn't grabbing
                UIController.EnableInteractCrosshair("Grab");

            }
        }
        else if (currGrabbedObj)
        {

            if (Input.GetKey(grabKey))
            {

                // still holding onto grabbed object
                UIController.EnableInteractCrosshair("");

            }
            else
            {

                // lets go of grabbed object
                currGrabbedObj.useGravity = true;
                currGrabbedObj.freezeRotation = false;
                currGrabbedObj = null;

            }
        }
        else
        {

            UIController.DisableInteractCrosshair();

        }

        lastIsGrounded = isGrounded;

    }

    private void FixedUpdate()
    {

        // grabbing
        if (currGrabbedObj)
        {

            Vector3 directionToPoint = grabPoint.position - currGrabbedObj.position;
            currGrabbedObj.velocity = directionToPoint * 12f * (directionToPoint.magnitude);
            float newSpeed = moveSpeed;
            newSpeed /= (currGrabbedObj.mass);

            if (newSpeed < moveSpeed)
                moveSpeed = newSpeed;

        }

        if (movementState == MovementState.Ziplining)
            return;

        if (elevatorMoving)
            rb.AddForce(Vector3.down * 2000f, ForceMode.VelocityChange);

        movementDirection = transform.forward * verticalInput + transform.right * horizontalInput;

        if (movementDirection != Vector3.zero && !firstMovementDetected)
        {

            gameManager.StartTimer();
            firstMovementDetected = true;

        }

        // swinging
        if (movementState == MovementState.Swinging && joint != null)
        {

            HandleSwingMovement();

        }
        // wall running
        else if (movementState == MovementState.WallRunningLeft || movementState == MovementState.WallRunningRight)
        {

            HandleWallRunMovement();

        }
        // on valid slope
        else if (CheckSlope() == SlopeType.ValidUp || CheckSlope() == SlopeType.ValidDown && !exitingSlope)
        {

            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 30f, ForceMode.Force);

        }
        // on invalid slope
        else if (CheckSlope() == SlopeType.Invalid)
        {

            rb.AddForce(Vector3.Cross(slopeHit.normal, Vector3.Cross(slopeHit.normal, Vector3.up)) * 500f, ForceMode.Acceleration);

        }
        // on ground (walking, sprinting, crouching, sliding)
        else if (isGrounded && movementState != MovementState.None)
        {

            rb.AddForce(movementDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        }
        // in air
        else if (movementState != MovementState.None)
        {

            rb.AddForce(movementDirection.normalized * moveSpeed * airMultiplier * 10f, ForceMode.Force);

        }

        // no gravity on slopes
        if (!inGravityZone && movementState != MovementState.WallRunningLeft && movementState != MovementState.WallRunningRight && movementState != MovementState.Ziplining)
        {

            // use gravity if player is on invalid slope or no slope (no gravity if player is on slope)
            rb.useGravity = CheckSlope() == SlopeType.None || CheckSlope() == SlopeType.Invalid;

        }
    }

    private void LateUpdate()
    {

        DrawRope();

    }

    private void OnTriggerEnter(Collider collider)
    {

        // check if collides with death zone
        if (collider.CompareTag("DeathZone"))
        {

            movementState = MovementState.Killed;
            gameManager.KillPlayer();

        }
    }

    private void OnTriggerStay(Collider collider)
    {

        // check if collides with effect zone
        if (collider.CompareTag("EffectZone"))
        {

            // loop through each effect on the player
            foreach (Effect effect in currentEffects)
            {

                // check if effect type is low gravity
                if (effect.GetEffectType() == EffectType.Gravity)
                {

                    // check if gravity hasn't been disabled
                    if (rb.useGravity)
                    {

                        // disable gravity and flag that player is in gravity zone
                        rb.useGravity = false;
                        inGravityZone = true;

                    }

                    // add downwards "gravity" force
                    rb.AddForce(transform.up * Physics.gravity.y * effect.GetMultiplier(), ForceMode.Force);

                }
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {

        // check if collides with effect zone
        if (collider.CompareTag("EffectZone") && inGravityZone)
        {

            rb.useGravity = true;
            inGravityZone = false;

        }
    }
    #endregion

    #region LOOKING
    public void SetLookRotations(float xRotation, float yRotation)
    {

        this.xRotation = xRotation;
        this.yRotation = yRotation;

    }
    #endregion

    #region MOVEMENT
    private void HandleMovementState()
    {

        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (movementState == MovementState.Ziplining && ziplineEnabled)
        {

            // ziplining
            movementState = MovementState.Ziplining;
            ResetAnimations();
            animator.SetBool("isZiplining", true);

        }
        else if (movementState == MovementState.Swinging && swingEnabled)
        {

            // swinging
            movementState = MovementState.Swinging;
            desiredMoveSpeed = maxSwingSpeed;

            if (flatVel.magnitude >= minMovementVelocity)
            {

                ResetAnimations();
                animator.SetBool("isSwinging", true);

            }
            else
            {

                ResetAnimations();

            }
        }
        else if (movementState == MovementState.WallRunningLeft && wallRunEnabled)
        {

            // wall running left
            movementState = MovementState.WallRunningLeft;
            desiredMoveSpeed = wallRunSpeed;

            if (flatVel.magnitude >= minMovementVelocity)
            {

                ResetAnimations();
                animator.SetBool("isWallRunningLeft", true);

            }
            else
            {

                ResetAnimations();

            }
        }
        else if (movementState == MovementState.WallRunningRight && wallRunEnabled)
        {

            // wall running right
            movementState = MovementState.WallRunningRight;
            desiredMoveSpeed = wallRunSpeed;

            if (flatVel.magnitude >= minMovementVelocity)
            {

                ResetAnimations();
                animator.SetBool("isWallRunningRight", true);

            }
            else
            {

                ResetAnimations();

            }
        }
        else if (movementState == MovementState.Sliding && slideEnabled)
        {

            // sliding
            movementState = MovementState.Sliding;

            // state 1: player is on a flat surface
            if (CheckSlope() == SlopeType.None && isGrounded)
            {

                desiredMoveSpeed = slideForwardForce;

                // decrement slide timer
                slideTimer -= Time.deltaTime;


            }
            // state 2: player is going up slope
            else if (CheckSlope() == SlopeType.ValidUp)
            {

                // desired move speed is the max slide speed x upwards slide factor
                desiredMoveSpeed = maxSlideSpeed * upwardsSlideFactor;

                // decrement slide timer
                slideTimer -= Time.deltaTime;

            }
            // state 3: player is going down slope
            else if (CheckSlope() == SlopeType.ValidDown)
            {

                // desired move speed is the maximum slide speed
                desiredMoveSpeed = maxSlideSpeed;


            }
            // state 4: none (in air, etc)
            else
            {

                // desired move speed is sprint speed
                desiredMoveSpeed = sprintSpeed;

            }

            if (slideTimer <= 0f)
                StopSlide();

            if (flatVel.magnitude >= minMovementVelocity)
            {

                ResetAnimations();
                animator.SetBool("isSliding", true);

            }
            else
            {

                ResetAnimations();

            }
        }
        else if (movementState == MovementState.Crouching && crouchEnabled)
        {

            // crouching
            movementState = MovementState.Crouching;
            desiredMoveSpeed = crouchSpeed;

            if (flatVel.magnitude >= minMovementVelocity || rb.velocity.magnitude > 0.01f)
            {

                ResetAnimations();
                animator.SetBool("isCrouching", true);

            }
            else
            {

                ResetAnimations();

            }
        }
        else if (isGrounded && movementDirection == Vector3.zero)
        {

            // idle
            movementState = MovementState.None;
            desiredMoveSpeed = walkSpeed;

            if (flatVel.magnitude >= minMovementVelocity)
            {

                ResetAnimations();

            }
            else
            {

                ResetAnimations();

            }
        }
        else if (isGrounded && Input.GetKey(sprintKey) && sprintEnabled)
        {

            // sprinting
            movementState = MovementState.Sprinting;
            audioManager.PlaySound(GameAudioManager.GameSoundEffectType.SprintFootstep);
            desiredMoveSpeed = sprintSpeed;

            if (flatVel.magnitude >= minMovementVelocity)
            {

                ResetAnimations();
                animator.SetBool("isSprinting", true);

            }
            else
            {

                ResetAnimations();

            }
        }
        else if (isGrounded && walkEnabled)
        {

            // walking
            movementState = MovementState.Walking;
            audioManager.PlaySound(GameAudioManager.GameSoundEffectType.WalkFootstep);
            desiredMoveSpeed = walkSpeed;

            if (flatVel.magnitude >= minMovementVelocity)
            {

                ResetAnimations();
                animator.SetBool("isWalking", true);

            }
            else
            {

                ResetAnimations();

            }
        }
        else if (!isGrounded && (flatVel.magnitude >= minMovementVelocity || rb.velocity.magnitude > 0.01f) && movementState != MovementState.Crouching && movementState != MovementState.Sliding)
        {

            // air
            movementState = MovementState.Air;
            ResetAnimations();

        }
        else
        {

            // default case
            movementState = MovementState.None;
            ResetAnimations();

        }

        if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > sprintSpeed - walkSpeed && moveSpeed != 0f)
        {

            if (moveSpeedCoroutine != null)
                StopCoroutine(moveSpeedCoroutine);

            moveSpeedCoroutine = StartCoroutine(LerpMoveSpeed());

        }
        else
        {

            moveSpeed = desiredMoveSpeed;

        }

        foreach (Effect effect in currentEffects)
            if (effect.GetEffectType() == EffectType.Speed)
                moveSpeed *= effect.GetMultiplier();

        lastDesiredMoveSpeed = desiredMoveSpeed;

    }

    private IEnumerator LerpMoveSpeed()
    {

        float timer = 0f;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (timer < difference)
        {

            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, timer / difference);

            if (CheckSlope() == SlopeType.ValidDown)
                timer += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * (1f + (Vector3.Angle(Vector3.up, slopeHit.normal) / 90f));
            else
                timer += Time.deltaTime * speedIncreaseMultiplier;


            yield return null;

        }

        moveSpeed = desiredMoveSpeed;

    }

    private void ControlSpeed()
    {

        // check if player is on any type of slope and isn't exiting it
        if (CheckSlope() == SlopeType.ValidUp || CheckSlope() == SlopeType.ValidDown || CheckSlope() == SlopeType.Invalid && !exitingSlope)
        {

            // normalize & limit velocity
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;

        }
        else
        {

            // reset y velocity
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {

                Vector3 controlledVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(controlledVel.x, rb.velocity.y, controlledVel.z);

            }
        }
    }

    public void ResetVelocity()
    {

        moveSpeed = 0f;
        lastDesiredMoveSpeed = 0f;
        desiredMoveSpeed = 0f;
        rb.velocity = Vector3.zero;

    }
    #endregion

    #region ANIMATIONS
    private void ResetAnimations()
    {

        animator.SetBool("isWalking", false);
        animator.SetBool("isSprinting", false);
        animator.SetBool("isCrouching", false);
        animator.SetBool("isSliding", false);
        animator.SetBool("isWallRunningLeft", false);
        animator.SetBool("isWallRunningRight", false);
        animator.SetBool("isSwinging", false);
        animator.SetBool("isZiplining", false);

    }
    #endregion

    #region JUMPING
    private void Jump()
    {

        if (inElevator)
            return;

        if (movementState == MovementState.Ziplining)
        {

            currZipline.ResetZipline();
            return;

        }

        jumpReady = false;
        Invoke(nameof(ResetJump), jumpCooldown);

        if (CheckSlope() == SlopeType.ValidUp || CheckSlope() == SlopeType.ValidDown)
        {

            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            exitingSlope = true;
            return;

        }

        float force = jumpForce;

        foreach (Effect effect in currentEffects)
            if (effect.GetEffectType() == EffectType.Jump)
                force *= effect.GetMultiplier();

        rb.velocity = new Vector3(rb.velocity.x, force, rb.velocity.z);

    }

    private void ZiplineJump()
    {

        if (movementState != MovementState.Ziplining)
            return;

        jumpReady = false;
        Invoke(nameof(ResetJump), jumpCooldown);

        if (CheckSlope() == SlopeType.ValidUp || CheckSlope() == SlopeType.ValidDown)
            exitingSlope = true;

        float yVel = rb.velocity.y;
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.velocity = Vector3.zero;
        rb.AddForce((transform.up * (ziplineExitUpwardsFactor * (yVel >= 0 ? yVel : (-yVel / 2f)))) + (transform.forward * (ziplineExitForwardFactor * flatVel.magnitude)), ForceMode.Acceleration);
        currZipline = null;
        movementState = MovementState.None;

    }

    private void ResetJump()
    {

        jumpReady = true;

    }
    #endregion

    #region CROUCH
    private void Crouch()
    {

        if (uncrouchQueued || inElevator)
            return;

        movementState = MovementState.Crouching;
        UIController.EnableCrosshair();
        ForceCrouch(crouchDownwardsForce);

    }

    private void ForceCrouch(float force)
    {

        transform.position += new Vector3(0f, startHeight / 2f, 0f);
        transform.localScale = new Vector3(transform.localScale.x, crouchScale, transform.localScale.z);
        cameraPos.localScale = new Vector3(cameraPos.localScale.x, Mathf.Pow(crouchScale, -1f), cameraPos.localScale.z);
        rb.AddForce(-transform.up * force, ForceMode.Force);

    }

    public void Uncrouch()
    {

        foreach (Transform checker in obstacleCheckers)
        {

            if (Physics.Raycast(checker.position, Vector3.up, startHeight + uncrouchMinClearing, environmentMask))
            {

                uncrouchQueued = true;
                return;

            }
        }

        movementState = MovementState.None;
        uncrouchQueued = false;
        transform.localScale = new Vector3(transform.localScale.x, startScale, transform.localScale.z);
        cameraPos.localScale = Vector3.one;

    }
    #endregion

    #region SLIDING
    private void StartSlide()
    {

        if (inElevator)
            return;

        movementState = MovementState.Sliding;
        UIController.EnableCrosshair();
        ForceCrouch(slideDownwardsForce);
        slideTimer = maxSlideTime;

    }

    private void StopSlide()
    {

        movementState = MovementState.None;
        Uncrouch();

    }
    #endregion

    #region WALL RUNNING
    private void CheckForWall()
    {

        // check for wall to the left
        wallLeft = Physics.SphereCast(transform.position, wallCheckRadius, -transform.right, out leftWallHit, wallCheckDistance, wallMask);

        // check for wall to the right
        wallRight = Physics.SphereCast(transform.position, wallCheckRadius, transform.right, out rightWallHit, wallCheckDistance, wallMask);

    }

    private bool CanWallRun()
    {

        // check all the flags for wall running
        return (!Physics.Raycast(feet.position, Vector3.down, minJumpHeight, environmentMask)) && (movementState != MovementState.WallRunningLeft && movementState != MovementState.WallRunningRight) && lastWall != (wallRight ? rightWallHit.transform : leftWallHit.transform);

    }

    private void HandleWallRunState()
    {

        // update wall check
        CheckForWall();

        // wall scaling
        if (wallRunScalingEnabled)
        {

            runningUpWall = Input.GetKey(upwardsWallRunKey);
            runningDownWall = Input.GetKey(downwardsWallRunKey);

        }

        // state 1: wall running
        if ((wallLeft || wallRight) && verticalInput > 0 && !isGrounded && !exitingWall)
        {

            // start wall run
            if (movementState != MovementState.WallRunningLeft && movementState != MovementState.WallRunningRight)
                StartWallRun();

            // wall run timer (if enabled)
            if (wallRunTimerEnabled)
            {

                if (wallRunTimer > 0f)
                    wallRunTimer -= Time.deltaTime;

                // timer runs out
                if (wallRunTimer <= 0f && (movementState == MovementState.WallRunningLeft || movementState == MovementState.WallRunningRight))
                {

                    WallJump();
                    // exitingWall = true;
                    exitWallTimer = exitWallTime;

                }
            }

            // wall jumping
            if (Input.GetKeyDown(jumpKey))
                WallJump();

        }
        // state 2: exiting wall
        else if (exitingWall)
        {

            // if player is walling running, stop it
            if (movementState == MovementState.WallRunningLeft || movementState == MovementState.WallRunningRight)
                StopWallRun();

            // decrement exit wall timer
            if (exitWallTimer > 0f)
                exitWallTimer -= Time.deltaTime;

            // check if exit wall timer is over and reset boolean
            if (exitWallTimer <= 0f)
                exitingWall = false;

        }
        // state 3: none
        else
        {

            // if player is wall running, stop it
            if (movementState == MovementState.WallRunningLeft || movementState == MovementState.WallRunningRight)
                StopWallRun();

        }
    }

    private void StartWallRun()
    {

        // make sure player can wall run
        if (!CanWallRun())
            return;

        // disable gravity
        rb.useGravity = false;

        // get wall normal
        RaycastHit wallHit = wallRight ? rightWallHit : leftWallHit;

        // reset last wall
        lastWall = wallHit.transform;

        // set movement state based on which side the wall is on
        movementState = wallRight ? MovementState.WallRunningRight : MovementState.WallRunningLeft;

        // initialize timer if timer is enabled
        if (wallRunTimerEnabled)
            wallRunTimer = maxWallRunDuration;

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // apply camera effects
        LerpFOV(wallRunFOV, wallRunFOVLerpDuration);

        // constrain camera
        if (lookRotationLerpCoroutine != null)
            StopCoroutine(lookRotationLerpCoroutine);

        // make tilt negative when wall running on the left side
        if (wallLeft)
            LerpTilt(new Vector3(0f, 0f, -wallRunTilt), wallRunTiltLerpDuration);
        if (wallRight)
            LerpTilt(new Vector3(0f, 0f, wallRunTilt), wallRunTiltLerpDuration);

        // limit player rotation
        wallForward = Vector3.Cross(wallHit.normal, Vector3.up);

        // check if player is wall running in opposite direction
        if ((transform.forward - wallForward).magnitude > (transform.forward + wallForward).magnitude)
        {

            // flip rotation vector & wall forward
            wallForward *= -1;

        }

        Quaternion rotation = Quaternion.LookRotation(wallForward, Vector3.up);

        // stop exiting look rotation lerp coroutine if it exists
        if (lookRotationLerpCoroutine != null)
            StopCoroutine(lookRotationLerpCoroutine);

        // check if player is out of wall run look clamps
        //if (yRotation < rotation.eulerAngles.y - wallSideCameraClamp || yRotation > rotation.eulerAngles.y + wallOutsideCameraClamp)
        // adjust look clamps
        lookRotationLerpCoroutine = StartCoroutine(LerpLookRotation(rotation));

        // weaken gravity effect
        if (useWallRunGravity)
            gravityDelayCoroutine = StartCoroutine(HandleWallRunGravity());

    }

    private void HandleWallRunMovement()
    {

        if (rb.velocity.magnitude < 0.1f)
        {

            StopWallRun();
            return;

        }

        // calculate wall forward using Vector3.Cross and wall normal
        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
        wallForward = Vector3.Cross(wallNormal, Vector3.up);

        // check if player is wall running in opposite direction
        if ((transform.forward - wallForward).magnitude > (transform.forward + wallForward).magnitude)
            // flip wall forward
            wallForward *= -1;

        // forward force
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        // upwards/downwards force
        if (runningUpWall)
            rb.velocity = new Vector3(rb.velocity.x, wallScaleSpeed, rb.velocity.z);
        if (runningDownWall)
            rb.velocity = new Vector3(rb.velocity.x, -wallScaleSpeed, rb.velocity.z);

        // check if player isn't moving away from the wall
        if (!(wallLeft && horizontalInput > 0f) && !(wallRight && horizontalInput < 0f))
            // push player into wall
            rb.AddForce(-wallNormal * wallRunNormalForce, ForceMode.Force);

    }

    private IEnumerator HandleWallRunGravity()
    {

        // counter force initially fully counters gravity
        float counterForce = -Physics.gravity.y;

        // wait for gravity delay
        yield return new WaitForSeconds(gravityDelay);

        // enable gravity
        rb.useGravity = true;

        // while player is wall running
        while (movementState == MovementState.WallRunningLeft || movementState == MovementState.WallRunningRight)
        {

            // add counter force
            rb.AddForce(transform.up * counterForce, ForceMode.Force);

            // wait for duration using rate
            yield return new WaitForSeconds(1f / gravityIncrementRate);

            // decrement counter force
            counterForce--;

        }

        // reset coroutine
        gravityDelayCoroutine = null;

    }

    public void StopWallRun()
    {

        // reset movement state
        movementState = MovementState.None;

        // reset camera effects
        LerpFOV(startFOV, wallRunFOVLerpDuration);
        LerpTilt(startTilt, wallRunTiltLerpDuration);

    }

    private void WallJump()
    {

        // enter exit state
        exitingWall = true;
        exitWallTimer = exitWallTime;

        // calculate wall normal based on which side the wall is on
        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        // calculate wall jump force
        Vector3 force = (transform.up * wallJumpUpForce) + (wallNormal * wallJumpSideForce);

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.y);

        // add force to player
        rb.AddForce(force, ForceMode.Impulse);

    }
    #endregion

    #region SWINGING
    public Transform GetSwingMuzzle()
    {

        return muzzle;

    }

    public Vector3 GetSwingPoint()
    {

        return swingPoint.position;

    }

    private void CheckSwingPoints()
    {

        if (!swingEnabled || joint != null || (movementState == MovementState.WallRunningLeft || movementState == MovementState.WallRunningRight) || movementState == MovementState.Swinging)
            return;

        Physics.Raycast(cameraPos.position, cameraPos.forward, out RaycastHit raycastHit, maxSwingDistance);

        if (raycastHit.point != Vector3.zero && (swingMask & (1 << raycastHit.transform.gameObject.layer)) != 0)
        {

            // direct hit
            predictionHit = raycastHit;

            // place prediction object
            predictionObj.position = predictionHit.point + (predictionHit.normal * 0.002f);
            predictionObj.rotation = Quaternion.LookRotation(-predictionHit.normal, Vector3.up);
            predictionObj.gameObject.SetActive(true);

            // disable crosshair
            UIController.DisableCrosshair();

        }
        else
        {

            Physics.SphereCast(cameraPos.position, predictionRadius, cameraPos.forward, out RaycastHit sphereCastHit, maxSwingDistance, swingMask);

            if (sphereCastHit.point != Vector3.zero)
            {

                // indirect / predicted hit
                predictionHit = sphereCastHit;

                // place prediction object
                predictionObj.position = predictionHit.point + (predictionHit.normal * 0.002f);
                predictionObj.rotation = Quaternion.LookRotation(-predictionHit.normal, Vector3.up);
                predictionObj.gameObject.SetActive(true);

                // enable crosshair
                UIController.EnableCrosshair();

            }
            else
            {

                // miss
                predictionHit.point = Vector3.zero;
                predictionObj.gameObject.SetActive(false);
                UIController.EnableCrosshair();

            }
        }
    }

    private void StartSwing()
    {

        if (predictionHit.point == Vector3.zero)
            return;

        movementState = MovementState.Swinging;
        rigBuilder.layers[leftHandIKRigIndex].active = false;
        rigBuilder.layers[rightHandIKRigIndex].active = false;
        rigBuilder.layers[rightHandFollowRigIndex].active = true;

        predictionObj.gameObject.SetActive(false);
        UIController.EnableCrosshair();

        swingPoint.position = predictionHit.point;
        swingIKTarget.position = swingPoint.position;
        swingPoint.parent = predictionHit.transform;
        joint = gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = swingPoint.position;

        float distanceFromPoint = Vector3.Distance(transform.position, swingPoint.position);

        joint.maxDistance = distanceFromPoint * 0.8f;
        joint.minDistance = distanceFromPoint * 0.25f;

        joint.spring = jointSpring;
        joint.damper = jointDamper;
        joint.massScale = jointMassScale;

        audioManager.PlaySound(GameAudioManager.GameSoundEffectType.Grapple);

    }

    private void DrawRope()
    {

        if (movementState != MovementState.Swinging)
        {

            currentSwingPosition = muzzle.position;
            spring.Reset();

            if (lineRenderer.positionCount > 0)
                lineRenderer.positionCount = 0;

            return;

        }

        if (lineRenderer.positionCount == 0)
        {

            spring.SetVelocity(velocity);
            lineRenderer.positionCount = quality + 1;

        }

        spring.SetDamper(damper);
        spring.SetStrength(strength);
        spring.Update(Time.deltaTime);

        Vector3 up = Quaternion.LookRotation(swingPoint.position - muzzle.position).normalized * Vector3.up;

        currentSwingPosition = Vector3.Lerp(currentSwingPosition, swingPoint.position, Time.deltaTime * 12f);

        for (int i = 0; i < quality + 1; i++)
        {

            var delta = i / (float)quality;
            Vector3 offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI * spring.Value * effectCurve.Evaluate(delta));

            lineRenderer.SetPosition(i, Vector3.Lerp(muzzle.position, currentSwingPosition, delta) + offset);

        }
    }

    private void HandleSwingMovement()
    {

        swingIKTarget.position = swingPoint.position;
        joint.connectedAnchor = swingPoint.position;

        if (horizontalInput != 0f)
            rb.AddForce((horizontalInput > 0f ? transform.right : -transform.right) * horizontalThrustForce * Time.deltaTime);

        if (verticalInput > 0f)
            rb.AddForce(transform.forward * forwardThrustForce * Time.deltaTime);

        if (Input.GetKey(jumpKey) && jumpEnabled)
        {

            Vector3 directionToPoint = swingPoint.position - transform.position;
            rb.AddForce(directionToPoint.normalized * forwardThrustForce * Time.deltaTime);

            float distanceFromPoint = Vector3.Distance(transform.position, swingPoint.position);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

        }

        if (Input.GetKey(cableExtendKey))
        {

            float extendedDistanceFromPoint = Vector3.Distance(transform.position, swingPoint.position) + cableExtendSpeed;

            joint.maxDistance = extendedDistanceFromPoint * 0.8f;
            joint.minDistance = extendedDistanceFromPoint * 0.25f;

        }
    }

    public void StopSwing()
    {

        movementState = MovementState.None;
        rigBuilder.layers[leftHandIKRigIndex].active = true;
        rigBuilder.layers[rightHandIKRigIndex].active = true;
        rigBuilder.layers[rightHandFollowRigIndex].active = false;
        UIController.EnableCrosshair();
        Destroy(joint);

    }
    #endregion

    #region ZIPLINING
    public void ResetZipline()
    {

        ZiplineJump();

    }
    #endregion

    #region HEADBOB
    private void HandleHeadbob()
    {

        if (movementState == MovementState.Ziplining)
        {

            timer += ziplineBobSpeed * Time.deltaTime;
            cameraPos.localPosition = new Vector3(cameraPos.localPosition.x, startCameraPos.y + Mathf.Sin(timer) * ziplineBobAmount, cameraPos.localPosition.z);
            return;

        }

        if (isGrounded)
        {

            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude >= minMovementVelocity)
            {

                switch (movementState)
                {

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
    #endregion

    #region SLOPES
    private SlopeType CheckSlope()
    {

        if (Physics.Raycast(feet.position, Vector3.down, out slopeHit, slopeCheckDistance))
        {

            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);

            if (angle != 0f)
            {

                if (angle <= maxSlopeAngle)
                {

                    float dot = Vector3.Dot(slopeHit.normal, transform.forward);

                    if (dot < 0f)
                        return SlopeType.ValidUp;
                    else if (dot > 0f)
                        return SlopeType.ValidDown;

                }
                else
                {

                    return SlopeType.Invalid;

                }
            }
        }

        return SlopeType.None;

    }

    private Vector3 GetSlopeMoveDirection()
    {

        return Vector3.ProjectOnPlane(movementDirection, slopeHit.normal).normalized;

    }
    #endregion

    #region CAMERA EFFECTS
    private void LerpFOV(float targetFOV, float duration)
    {

        if (wallRunFOVCoroutine != null)
            StopCoroutine(wallRunFOVCoroutine);

        wallRunFOVCoroutine = StartCoroutine(StartFOVLerp(mainCamera.fieldOfView, targetFOV, duration));

    }

    private IEnumerator StartFOVLerp(float startFOV, float targetFOV, float duration)
    {

        float currentTime = 0f;

        while (currentTime < duration)
        {

            currentTime += Time.unscaledDeltaTime;
            mainCamera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, currentTime / duration);
            yield return null;

        }

        mainCamera.fieldOfView = targetFOV;
        wallRunFOVCoroutine = null;

    }

    private void LerpTilt(Vector3 targetTilt, float duration)
    {

        if (wallRunTiltCoroutine != null)
            StopCoroutine(wallRunTiltCoroutine);

        wallRunTiltCoroutine = StartCoroutine(StartTiltLerp(mainCamera.transform.localRotation, Quaternion.Euler(targetTilt), duration));

    }

    private IEnumerator StartTiltLerp(Quaternion startTilt, Quaternion targetTilt, float duration)
    {

        float currentTime = 0f;

        while (currentTime < duration)
        {

            currentTime += Time.unscaledDeltaTime;
            mainCamera.transform.localRotation = Quaternion.Lerp(startTilt, targetTilt, currentTime / duration);
            yield return null;

        }

        mainCamera.transform.localRotation = targetTilt;
        wallRunTiltCoroutine = null;

    }

    private IEnumerator LerpLookRotation(Quaternion targetRotation)
    {

        float currentTime = 0f;
        Quaternion startRotation = Quaternion.Euler(0f, yRotation, 0f);

        while (currentTime < lookRotationLerpDuration)
        {

            currentTime += Time.deltaTime;
            yRotation = Quaternion.Lerp(startRotation, targetRotation, currentTime / lookRotationLerpDuration).eulerAngles.y;
            yield return null;

        }

        yRotation = targetRotation.eulerAngles.y;
        lookRotationLerpCoroutine = null;

    }
    #endregion

    #region MOVEMENT TOGGLES
    public void EnableAllMovement()
    {

        if (levelLookEnabled)
            lookEnabled = true;

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

        if (levelGrabEnabled)
            grabEnabled = true;

    }

    public void DisableAllMovement()
    {

        if (levelLookEnabled)
            lookEnabled = false;

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

        if (levelGrabEnabled)
            grabEnabled = false;

        StopSwing();

        movementState = MovementState.None;
        ResetAnimations();

    }

    public void EnableLook()
    {

        if (levelLookEnabled)
            lookEnabled = true;

    }

    public void DisableLook()
    {

        if (levelLookEnabled)
            lookEnabled = false;

    }

    public void EnableWalk()
    {

        walkEnabled = true;

    }

    public void DisableWalk()
    {

        walkEnabled = false;

    }

    public void EnableSprint()
    {

        sprintEnabled = true;

    }

    public void DisableSprint()
    {

        sprintEnabled = false;

    }

    public void EnableJump()
    {

        jumpEnabled = true;

    }

    public void DisableJump()
    {

        jumpEnabled = false;

    }

    public void EnableCrouch()
    {

        crouchEnabled = true;

    }

    public void DisableCrouch()
    {

        crouchEnabled = false;

    }

    public void EnableSlide()
    {

        slideEnabled = true;

    }

    public void DisableSlide()
    {

        slideEnabled = false;

    }

    public void EnableWallRun()
    {

        wallRunEnabled = true;

    }

    public void DisableWallRun()
    {

        wallRunEnabled = false;

    }

    public void EnableSwing()
    {

        swingEnabled = true;

    }

    public void DisableSwing()
    {

        swingEnabled = false;

    }

    public void EnableZipline()
    {

        ziplineEnabled = true;

    }

    public void DisableZipline()
    {

        ziplineEnabled = false;

    }

    public void EnableGrab()
    {

        grabEnabled = true;

    }

    public void DisableGrab()
    {

        grabEnabled = false;

    }
    #endregion

    #region ELEVATOR
    public void SetInElevator(bool inElevator)
    {

        if (inElevator)
        {

            StopSlide();
            Uncrouch();
            StopSwing();
            // StopWallRun();

        }

        this.inElevator = inElevator;

    }

    public void SetElevatorMoving(bool elevatorMoving)
    {

        this.elevatorMoving = elevatorMoving;

    }
    #endregion

    #region MISCELLANEOUS GETTERS & SETTERS
    public float GetPlayerHeight()
    {

        return startHeight;

    }
    #endregion

    #region EFFECTS
    public void AddEffect(Effect effect)
    {

        currentEffects.Add(effect);

    }

    public void RemoveEffect(Effect effect)
    {

        currentEffects.Remove(effect);

    }
    #endregion
}
