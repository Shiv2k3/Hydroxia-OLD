using UnityEngine;
public class ThirdPersonController
{
    // ============================================================================================//
    private readonly PlayerMovement _inputs;
    private readonly Transform _camera;
    private readonly CharaterSkin skin;
    private readonly Rigidbody _rb;
    // ============================================================================================//
    private readonly bool _applyPlanetRotation;
    private readonly float _damping;
    private readonly int _myPlanetID;
    private readonly float _maxSpeed;
    // ============================================================================================//
    private LayerMask _walkableLayers;
    private Quaternion _rotation;
    private Vector3 _forward;
    private Vector3 _right;
    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private Vector2 _lookInputDir;
    // ============================================================================================//
    public ThirdPersonController(PlayerMovement inputActions, Rigidbody rb, Transform playerSkin, Camera camera, float damping, int myID, bool applyPlanetRotation, LayerMask walkableLayers, float maxSpeed)
    {
        _rb = rb;
        _rotation = rb.rotation;
        _camera = camera.transform;
        _walkableLayers = walkableLayers;
        _maxSpeed = maxSpeed;
        _damping = damping;
        _myPlanetID = myID;
        _applyPlanetRotation = applyPlanetRotation;

        // WATER MOVEMENT INIT
        skin = new CharaterSkin(playerSkin);
        _inputs = inputActions;
    }
    // ============================================================================================//
    public void Move()
    {
        Rotate();
        GetDirection();
        ApplyMove();
    }
    // ============================================================================================//
    void Rotate()
    {
        // ROTATE DEPENDING ON IF PLAYER IS UNDER WATER OR GROUND
        if (IsMoving)
            if (IsUnderWater)
                _rotation = Quaternion.AngleAxis(_lookInputDir.y, _rotation * Vector3.right) * _rotation;
            else
                _rotation = Quaternion.AngleAxis(_lookInputDir.x, _rotation * Vector3.up) * _rotation; // ROTATE PLAYER ON Y AXIS BY MOUSE X INPUt

        // FOR VISUAL PURPOSES
        skin.RotateSkin(_lookInputDir, _inputs.Player.Movement.ReadValue<Vector2>(), IsUnderWater, _camera);
    }
    // ============================================================================================//
    private void GetDirection()
    {
        _forward = _rotation * Vector3.forward * _moveInput.y;
        _right = _rotation * Vector3.right * _moveInput.x;

        if (IsUnderWater && _applyPlanetRotation) // NORMALIZE PLAYER IF UNDER WATER, SKIN WILL MAINTAIN ROTATION
            _rotation = Quaternion.FromToRotation(_rotation * Vector3.up, _rb.position.normalized) * Quaternion.AngleAxis(_lookInput.x, _rotation * Vector3.up) * _rotation;
    }
    // ============================================================================================//
    private void ApplyMove()
    {
        Vector3 up = _rotation * Vector3.up;
        Vector3 direction = _forward + _right;

        Ray ray = new Ray(_rb.position + up / 1.5f, direction);
        if (_rb.velocity.magnitude < _maxSpeed && !Physics.Raycast(ray, 1.1f, _walkableLayers))
            _rb.MovePosition(_rb.position + direction);


        if (_applyPlanetRotation) _rotation = Quaternion.FromToRotation(_rb.rotation * Vector3.up, _rb.position.normalized) * _rotation;
        _rb.MoveRotation(_moveInput == Vector2.zero ? _rb.rotation : _rotation);
    }
    // ============================================================================================//
    public void UpdateInput(Vector2 moveInput, Vector2 lookInput, Vector2 lookInputDir)
    {
        _moveInput = moveInput;
        _lookInput = lookInput;
        _lookInputDir = lookInputDir;
    }
    // ============================================================================================//
    bool IsMoving { get { return Mathf.Abs(_moveInput.normalized.x) + Mathf.Abs(_moveInput.normalized.y) > _damping; } }
    private bool IsUnderWater { get { return !BodyManager.IsBodyUnderWater(_myPlanetID); } }
    // ============================================================================================//
}