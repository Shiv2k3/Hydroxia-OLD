using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class CharacterGrappling : MonoBehaviour
{
    // REF
    private Rigidbody _rb;
    private Camera _camera;
    private Movement _movement;
    private Transform _grappleT;
    private Transform _hookT;
    private LayerMask _grappelableLayers;

    // PARAMETERS
    public float _grappleStrength;
    public float _grappleRange;
    public float _grappleBreakDistance;
    private int _bodyID;

    // GRAPPLE INFO
    private Vector3 _grappleToPos;
    public bool _usingGrapple;
    Ray _cameraRay;
    Ray _playerRay;
    RaycastHit rayHit;
    private bool _useGravityState;
    private bool _isKinematic;

    Coroutine throwHook;
    void UseGrapple() => throwHook = StartCoroutine(ThrowHook());

    private IEnumerator ThrowHook()
    {
        Debug.Log("Throwing Hook");

        // GET HOOK READY
        _hookT.parent = null;
        hookThrown = true;

        // LERP INFO
        int steps = (int)(rayHit.distance / _grappleStrength);
        Vector3 hookPosition = _hookT.position;

        for (float i = 0; i < steps; i++)
        {
            _hookT.position = Vector3.Lerp(hookPosition, _grappleToPos, (i + 1) / steps);

            yield return null;
        }

        hookThrown = false;
        StartCoroutine(GoToGrapple());
    }
    private IEnumerator GoToGrapple()
    {
        // GET PLAYER READY
        _usingGrapple = true;
        BodyManager.Instance.ToggleUseGravity(_bodyID, false);

        // GET LERP INFO
        int steps = (int)(rayHit.distance / _grappleStrength);
        Vector3 playerPosition = _rb.position;

        for (float i = 0; i < steps; i++)
        {
            _rb.position = Vector3.Lerp(playerPosition, _grappleToPos, (i + 1) / steps);

            yield return null;
        }

        // AFTERMATH
        _rb.velocity = Vector3.zero;
    }
    private IEnumerator RetractGrapple()
    {
        if (throwHook != null) StopCoroutine(throwHook);

        Debug.Log("Retracting Hook");

        // GET HOOK READY
        BodyManager.Instance.ToggleUseGravity(_bodyID, true);
        _usingGrapple = false;
        _hookT.parent = _grappleT;
        retractingHook = true;

        // GET LERP INFO
        int steps = (int)Vector3.Distance(_hookT.position, _grappleT.position);
        Vector3 hookPosition = _hookT.localPosition;

        for (float i = 0; i < steps; i++)
        {
            _hookT.localPosition = Vector3.Lerp(hookPosition, Vector3.zero, (i + 1) / steps);

            yield return null;
        }

        // FINAL 
        retractingHook = false;
        hookThrown = false;
    }

    public bool canGrapple;
    public bool hookThrown;
    public bool retractingHook;
    public bool UpdateState(bool grappelInput)
    {
        if (grappelInput)
        {
            if (!_usingGrapple && !hookThrown && !retractingHook)
            {
                _grappleToPos = GetGrappledInfo();
                canGrapple = _grappleToPos != Vector3.zero;

                if (canGrapple)
                    UseGrapple();
            }
            else if (!retractingHook)
            {
                StartCoroutine(RetractGrapple());
            }

        }

        return _usingGrapple;
    }
    public void Construct(Camera camera, Rigidbody playerRigidbody, float grappelRange, float grappelStrength, LayerMask grappelableLayers, Transform grappelT, Transform hookT, float grappleBreakDistance, int bodyID)
    {
        _camera = camera;
        _rb = playerRigidbody;
        _grappleRange = grappelRange;
        _grappleStrength = grappelStrength;
        _grappelableLayers = grappelableLayers;
        _grappleT = grappelT;
        _hookT = hookT;
        _grappleBreakDistance = grappleBreakDistance;
        _bodyID = bodyID;
        _useGravityState = true;

        _movement = playerRigidbody.GetComponent<Movement>();
    }
    Vector3 GetGrappledInfo()
    {
        _cameraRay = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(_cameraRay, out rayHit, Mathf.Infinity, _grappelableLayers))
        {
            _playerRay = new Ray(_grappleT.position, -(_grappleT.position - rayHit.point).normalized);
            if (Physics.Raycast(_playerRay, out rayHit, _grappleRange, _grappelableLayers))
                if (Vector3.Distance(_rb.position, rayHit.point) > _grappleBreakDistance)
                    return rayHit.point;
                else
                    return Vector3.zero;
        }
        return Vector3.zero;
    }
}
