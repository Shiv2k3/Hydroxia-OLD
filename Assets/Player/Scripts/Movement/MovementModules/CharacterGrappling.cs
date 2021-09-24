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
    public bool _throwingGrapple;
    public bool _retractingGrapple;
    public bool _movingToGrapple;
    public bool _grappled;
    Ray _cameraRay;
    Ray _playerRay;
    RaycastHit rayHit;
    private bool _useGravityState;
    private bool _isKinematic;
    Coroutine throwGrapple;
    Coroutine moveToGrapple;

    public bool UpdateState(bool grappelInput)
    {
        if (grappelInput)
        {
            if (_grappled || _throwingGrapple || _movingToGrapple)
            {
                if (throwGrapple != null) StopCoroutine(throwGrapple);
                if (moveToGrapple != null) StopCoroutine(moveToGrapple);

                StartCoroutine(RetractGrapple());
            }
            else if (_throwingGrapple == false && _retractingGrapple == false && _movingToGrapple == false)
            {
                Vector3 grappleTo = GetGrappledInfo();
                if (grappleTo != Vector3.zero) throwGrapple = StartCoroutine(ThrowGrapple(grappleTo));
            }

        }

        bool usingGrapple = _movingToGrapple || _grappled;

        return usingGrapple;
    }

    private IEnumerator ThrowGrapple(Vector3 grappleTo)
    {
        _throwingGrapple = true;
        _hookT.parent = null;

        int steps = (int)(rayHit.distance / _grappleStrength);
        Vector3 start = _hookT.position;

        for (float i = 0; i < steps; i++)
        {
            _hookT.position = Vector3.Lerp(start, grappleTo, (i + 1) / steps);
            yield return null;
        }

        _throwingGrapple = false;

        moveToGrapple = StartCoroutine(MoveToGrapple(grappleTo));
    }
    private IEnumerator MoveToGrapple(Vector3 moveTo)
    {
        _movingToGrapple = true;

        BodyManager.Instance.ToggleUseGravity(_bodyID, false);
        _rb.isKinematic = true;

        int steps = (int)(rayHit.distance / _grappleStrength);
        Vector3 start = _rb.position;

        for (float i = 0; i < steps; i++)
        {
            _rb.MovePosition(Vector3.Lerp(start, moveTo, (i + 1) / steps));
            yield return null;
        }

        _grappled = true;
        _movingToGrapple = false;
    }
    private IEnumerator RetractGrapple()
    {
        _retractingGrapple = true;
        _grappled = false;
        _movingToGrapple = false;
        _throwingGrapple = false;

        BodyManager.Instance.ToggleUseGravity(_bodyID, true);
        _rb.AddForce(_rb.velocity * _grappleStrength,ForceMode.VelocityChange); // HOW TO MAKE IT HAVE A GOOD FORCE
        _rb.isKinematic = false;

        int steps = (int)(Vector3.Distance(_hookT.position, _grappleT.position) / _grappleStrength);
        Vector3 start = _hookT.position;

        for (float i = 0; i < steps; i++)
        {
            _hookT.position = Vector3.Lerp(start, _grappleT.position, (i + 1) / steps);
            yield return null;
        }

        _hookT.parent = _grappleT;
        _hookT.localPosition = Vector3.zero;
        _hookT.localRotation = Quaternion.identity;
        _retractingGrapple = false;

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
