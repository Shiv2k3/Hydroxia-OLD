using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class MC_CharacterGrappling : MonoBehaviour
{
    // REF
    private Rigidbody _rb;
    private Camera _camera;
    private Transform _grappleT;
    private Transform _hookT;
    private LayerMask _grappelableLayers;

    // PARAMETERS
    public float _grappleStrength;
    public float _grappleRange;
    private int _bodyID;

    // GRAPPLE INFO
    public bool _throwingGrapple;
    public bool _retractingGrapple;
    public bool _movingToGrapple;
    public bool _grappled;
    public Vector2 _grappleClamp;
    Ray _cameraRay;

    Coroutine throwGrapple;
    Coroutine moveToGrapple;

    WaitForFixedUpdate fixedUpdate = new WaitForFixedUpdate();
    public bool UpdateState()
    {
        if (_grappled || _throwingGrapple || _movingToGrapple)
        {
            if (throwGrapple != null) StopCoroutine(throwGrapple);
            if (moveToGrapple != null) StopCoroutine(moveToGrapple);

            StartCoroutine(RetractGrapple());
        }
        else if (_retractingGrapple == false)
        {
            _cameraRay = _camera.ScreenPointToRay(Input.mousePosition);
            Vector3 grappleTo = _camera.transform.position + (_cameraRay.direction * _grappleRange);
            float angleToGraplle = Vector3.Angle(grappleTo, _cameraRay.direction) - 90;
            if (angleToGraplle < _grappleClamp.y && angleToGraplle > _grappleClamp.x)
                throwGrapple = StartCoroutine(ThrowGrapple(grappleTo));
        }

        bool usingGrapple = _movingToGrapple || _grappled;

        return usingGrapple;
    }
    #region COROTUINES
    private IEnumerator ThrowGrapple(Vector3 grappleTo)
    {
        _throwingGrapple = true;
        _hookT.parent = null;

        float distance = Vector3.Distance(_hookT.position, grappleTo);
        int steps = (int)(distance / _grappleStrength);
        Vector3 start = _hookT.position;

        for (float i = 0; i < steps; i++)
        {
            yield return fixedUpdate;
            if (Physics.CheckSphere(_hookT.position, 0.45f, _grappelableLayers))
            {
                moveToGrapple = StartCoroutine(MoveToGrapple(_hookT.position));
                _throwingGrapple = false;
                StopCoroutine(throwGrapple);
            }

            _hookT.position = Vector3.Lerp(start, grappleTo, (i + 1) / steps);
        }

        StartCoroutine(RetractGrapple());
    }
    private IEnumerator MoveToGrapple(Vector3 moveTo)
    {
        _movingToGrapple = true;

        BodyManager.Instance.ToggleUseGravity(_bodyID, false);
        _rb.isKinematic = true;

        float distance = Vector3.Distance(_rb.position, moveTo);
        int steps = (int)(distance / _grappleStrength);
        Vector3 start = _rb.position;

        for (float i = 0; i < steps; i++)
        {
            _rb.MovePosition(Vector3.Lerp(start, moveTo, (i + 1) / steps));
            yield return fixedUpdate;
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
        _rb.isKinematic = false;

        int steps = (int)(Vector3.Distance(_hookT.position, _grappleT.position) / _grappleStrength);
        Vector3 start = _hookT.position;

        for (float i = 0; i < steps; i++)
        {
            _hookT.position = Vector3.Lerp(start, _grappleT.position, (i + 1) / steps);
            yield return fixedUpdate;
        }

        _hookT.parent = _grappleT;
        _hookT.localPosition = Vector3.zero;
        _hookT.localRotation = Quaternion.identity;
        _retractingGrapple = false;

        for (int i = 0; i < 10; i++) // STABLIZE 
        {
            _rb.velocity /= (i + 1);
            yield return fixedUpdate;
        }
    }
    #endregion
    public void Construct(Camera camera, Rigidbody playerRigidbody, float grappelRange, float grappelStrength, LayerMask grappelableLayers, Transform grappelT, Transform hookT, Transform skin, int bodyID, Vector2 grappleClamp)
    {
        _camera = camera;
        _rb = playerRigidbody;
        _grappleRange = grappelRange;
        _grappleStrength = grappelStrength;
        _grappelableLayers = grappelableLayers;
        _grappleT = grappelT;
        _hookT = hookT;
        _bodyID = bodyID;
        _grappleClamp = grappleClamp;
    }
}
