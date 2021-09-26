using Sirenix.OdinInspector;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class MB_Movement : MonoBehaviour
{
    #region PRAMETERS
    // ============================================================================================//
    [FoldoutGroup("References")]
    [SerializeField] private PlayerMovement inputs;
    [FoldoutGroup("References")]
    [SerializeField] private Rigidbody playerRigidbody;
    [FoldoutGroup("References")]
    [SerializeField] private new Camera camera;
    [FoldoutGroup("References")]
    [SerializeField] private Transform playerSkin;
    // ============================================================================================//
    [FoldoutGroup("Settings")]
    [SerializeField] private bool hideCursor = true;
    [FoldoutGroup("Settings")]
    [SerializeField] private LayerMask playerLayer;
    [FoldoutGroup("Settings")]
    [SerializeField] private float sensitivity = 0.14f;
    [FoldoutGroup("Settings")]
    [Range(0.5f, 1)] [SerializeField] private float movementDamping = 0.5f;
    // ============================================================================================//
    [FoldoutGroup("Movement Settings")]
    [SerializeField] private LayerMask walkableLayers;
    [FoldoutGroup("Movement Settings")]
    [SerializeField] private bool onPlanet = true;
    [FoldoutGroup("Movement Settings")]
    [SerializeField] private bool useGravity;
    [FoldoutGroup("Movement Settings")]
    [SerializeField] private float moveSpeed = 7;
    [FoldoutGroup("Movement Settings")]
    [SerializeField] private float maxSpeed = 7;
    [HideInInspector] private bool inventoryClosed;
    [HideInInspector] private bool canMove = true;
    // ============================================================================================//
    [FoldoutGroup("Camera Settings")]
    [SerializeField] private LayerMask cameraCollisionLayers;
    [FoldoutGroup("Camera Settings")]
    [SerializeField] private float cameraCollisionDistance = 0.8f;
    [FoldoutGroup("Camera Settings")]
    [SerializeField] private Vector3 offset;
    [FoldoutGroup("Camera Settings")]
    [SerializeField, MinMaxSlider(-180f, 180f, true)] private Vector2 clamp;
    // ============================================================================================//
    [FoldoutGroup("Jump Settings")]
    [SerializeField] private LayerMask jumpableLayers;
    [FoldoutGroup("Jump Settings")]
    [SerializeField] private float jumpPower = 7;
    [FoldoutGroup("Jump Settings")]
    [SerializeField] private float jumpCooldown = 0.70f;
    // ============================================================================================//
    [FoldoutGroup("Swim Settings")]
    [SerializeField] private float swimUpPower = 5;
    [FoldoutGroup("Swim Settings")]
    [SerializeField] private float swimUpCooldown = .2f;
    [FoldoutGroup("Swim Settings")]
    [SerializeField] private float divePower = 5f;
    [FoldoutGroup("Swim Settings")]
    [SerializeField] private float diveCooldown = .2f;
    // ============================================================================================//
    [FoldoutGroup("Grappel Settings")]
    [SerializeField] private LayerMask grappelableLayers;
    [FoldoutGroup("Grappel Settings")]
    [SerializeField] private Transform grappelMachineTransform;
    [FoldoutGroup("Grappel Settings")]
    [SerializeField] private Transform grappleHookTransform;
    [FoldoutGroup("Grappel Settings")]
    [SerializeField] private float grappelStrength = 10;
    [FoldoutGroup("Grappel Settings")]
    [SerializeField] private float grappelRange = 25;
    [FoldoutGroup("Grappel Settings")]
    [SerializeField, MinMaxSlider("@clamp", true)] Vector2 grappleClamp = new Vector2(-40, 40);
    [HideInInspector] private bool isUsingGrapple;
    // ============================================================================================//
    [FoldoutGroup("Flying Settings")]
    [SerializeField] private float flyingCooldown = 0.7f;
    [FoldoutGroup("Flying Settings")]
    [SerializeField] private float flySpeed = 8;
    [FoldoutGroup("Flying Settings")]
    [SerializeField] private float flyMaxSpeed = 10;
    // ============================================================================================//
    [FoldoutGroup("Mount Setting")]
    [SerializeField, ReadOnly] private AB_MB_Mount _mount;
    // ============================================================================================//
    [Header("Planet")]
    private int bodyID;
    // ============================================================================================//
    [Header("Scripts")]
    private ThirdPersonController player;
    private ThirdPersonCamera cam;
    private CharacterJumping jump;
    private CharacterCrouching crouch;
    private CharacterGrappling grappling;
    private CharacterFlying flying;
    // ============================================================================================//
    [Header("Input Variables")]
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector2 lookInputDir;
    // ============================================================================================//
    #endregion
    #region MB
    private void Awake()
    {
        // INPUT
        if (hideCursor) Cursor.lockState = CursorLockMode.Locked;
        inputs = new PlayerMovement();
        inputs.Enable();
    }
    private void Start()
    {
        // GRAVITY
        if (transform.GetChild(0).localPosition.y != 1) Debug.Log("Y of mesh is not set to 1, if it no longer needs this value change value on the script called BodyHelper.cs    ");
        bodyID = BodyManager.Instance.AddNewBody(GetComponent<Rigidbody>());
        BodyManager.Instance.ToggleUseGravity(bodyID, useGravity);
        onPlanet = BodyManager.Instance.GetOnPlanet();

        //  MOVEMENT CLASSES INITIALIZATION
        player = new ThirdPersonController(inputs, playerRigidbody, playerSkin, camera, movementDamping, bodyID, onPlanet, walkableLayers, maxSpeed);
        cam = new ThirdPersonCamera(camera, offset, cameraCollisionLayers, playerLayer, movementDamping, cameraCollisionDistance, clamp);
        jump = new CharacterJumping(playerRigidbody, jumpPower, swimUpPower, jumpCooldown, swimUpCooldown, jumpableLayers, bodyID);
        crouch = new CharacterCrouching(playerRigidbody, divePower, diveCooldown, bodyID);
        flying = new CharacterFlying(flySpeed, flyMaxSpeed, playerRigidbody, flyingCooldown);
        grappling = gameObject.AddComponent<CharacterGrappling>();
        grappling.Construct(camera, playerRigidbody, grappelRange, grappelStrength, grappelableLayers, grappelMachineTransform, grappleHookTransform, playerSkin, bodyID, grappleClamp);
    }
    private void FixedUpdate()
    {
        // GET AND PROCESS INPUT
        GetInput();

        #region EXEC MOVEMENT
        if (inventoryClosed && !isUsingGrapple)
        {
            // UPDATE AND MOVE PLAYER
            player.UpdateInput(moveInput, lookInput, lookInputDir);
            player.Move(_mount);

            flying.Update(_mount, inputs.Player.Fly.ReadValue<float>(), onPlanet);
        }

        // UPDATE AND MOVE CAMERA
        cam.UpdateInput(lookInput, lookInputDir, playerRigidbody.position);
        cam.Move(_mount);

        // MAX SPEED
        if (playerRigidbody.velocity.magnitude > maxSpeed)
        {
            playerRigidbody.velocity = playerRigidbody.velocity.normalized * maxSpeed;
        }
        #endregion
    }
    private void Update()
    {
        inventoryClosed = Cursor.lockState == CursorLockMode.Locked;

        // GRAPPELING
        isUsingGrapple = grappling.UpdateState(inventoryClosed && inputs.Player.Grapple.triggered, _mount);

        if (inventoryClosed && !isUsingGrapple)
        {
            // UPDATE JUMP AND CROUCH STATUS
            jump.Update(_mount, inputs.Player.Jump.ReadValue<float>());
            crouch.Update(_mount, inputs.Player.Crouch.ReadValue<float>());
        }
    }
    private void OnDisable()
    {
        inputs.Disable();
    }
    #endregion
    #region METHODS
    private void GetInput()
    {
        // GET INPUT
        moveInput = Vector2.Lerp(moveInput, inputs.Player.Movement.ReadValue<Vector2>() * moveSpeed * Time.deltaTime, movementDamping);
        lookInput = Vector2.Lerp(lookInput, inputs.Player.Look.ReadValue<Vector2>() * sensitivity, movementDamping);
        if (inventoryClosed) lookInputDir += lookInput;

        // PROCESS INPUT
        if (moveInput != Vector2.zero && canMove) lookInputDir.x = Mathf.Lerp(lookInputDir.x, 0, movementDamping); // ZERO Y ROTATION IF MOVING
        lookInputDir.y = Mathf.Clamp(lookInputDir.y, clamp.x, clamp.y); // CLAMP X ROTATION
    }
    public void ToggleMovement(bool setActive) => canMove = setActive;
    public void ToggleMounted(AB_MB_Mount mount)
    {
        _mount = mount;
    }
    #endregion
}