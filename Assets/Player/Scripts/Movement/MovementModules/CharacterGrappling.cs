using System;
using System.Collections;
using UnityEngine;

public class CharacterGrappling : MonoBehaviour
{
    // REF
    private Rigidbody _rb;
    private Camera _camera;
    private Movement _movement;
    private Transform _grappelOrigin;
    private LayerMask _grappelableLayers;

    // PARAMETERS
    public float _grappleStrength;
    public float _grappleRange;
    public float _grappleBreakDistance;
    private int _bodyID;

    // GRAPPLE INFO
    private Vector3 _grapplePos;
    public bool _usingGrapple;
    Ray _cameraRay;
    Ray _playerRay;
    RaycastHit rayHit;
    private bool _useGravityState;
    private bool _isKinematic;

    private void ToggleGrapple()
    {
        if (_usingGrapple == false)
        {
            _grapplePos = GetGrappledInfo();

            if (_grapplePos != Vector3.zero)
            {
                _usingGrapple = true;
            }
            else
            {
                _usingGrapple = false;
            }
        }
        else
        {
            _usingGrapple = false;
        }

        if (_usingGrapple)
        {
            StartCoroutine(UseGrapple());
        }
        else
        {
            StopCoroutine(UseGrapple());
            BodyManager.Instance.ToggleUseGravity(_bodyID, _useGravityState);
            _movement.ToggleMovement(true);
            _rb.isKinematic = _isKinematic;
        }
    }
    private IEnumerator UseGrapple()
    {
        _isKinematic = _rb.isKinematic;
        _useGravityState = BodyManager.Instance.GetUseGravity(_bodyID);
        float steps = Mathf.FloorToInt(rayHit.distance / _grappleStrength);
        Vector3 startPos = _rb.position;
        BodyManager.Instance.ToggleUseGravity(_bodyID, false);
        _movement.ToggleMovement(false);

        for (int i = 0; i < steps; i++)
        {
            _rb.MovePosition(Vector3.Lerp(startPos, _grapplePos, i / steps));
            yield return null;
        }
        _rb.isKinematic = true;
    }
    public void Construct(Camera camera, Rigidbody playerRigidbody, float grappelRange, float grappelStrength, LayerMask grappelableLayers, Transform grappelOrigin, float grappleBreakDistance, int bodyID)
    {
        _camera = camera;
        _rb = playerRigidbody;
        _grappleRange = grappelRange;
        _grappleStrength = grappelStrength;
        _grappelableLayers = grappelableLayers;
        _grappelOrigin = grappelOrigin;
        _grappleBreakDistance = grappleBreakDistance;
        _bodyID = bodyID;
        _useGravityState = true;

        _movement = playerRigidbody.GetComponent<Movement>();
    }
    public bool UpdateState(bool grappelInput, bool canMoveState)
    {
        if (grappelInput)
            ToggleGrapple();

        return _usingGrapple;
    }
    Vector3 GetGrappledInfo()
    {
        _cameraRay = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(_cameraRay, out rayHit, Mathf.Infinity, _grappelableLayers))
        {
            _playerRay = new Ray(_grappelOrigin.position, -(_grappelOrigin.position - rayHit.point).normalized);

            if (Physics.Raycast(_playerRay, out rayHit, _grappleRange, _grappelableLayers))
                if (Vector3.Distance(_rb.position, rayHit.point) > _grappleBreakDistance)
                    return rayHit.point;
                else
                    return Vector3.zero;
        }
        return Vector3.zero;
    }
}
