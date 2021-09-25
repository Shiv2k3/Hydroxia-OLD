using System;
using System.Reflection;
using UnityEngine;

public class ThirdPersonCamera
{
    private Camera cam;
    private readonly Transform _transform;
    private readonly LayerMask _collisionLayers;
    private readonly LayerMask _playerLayer;

    private Vector3 _playerPosition;
    private Vector3 _targetRotation;
    private Vector2 _offset;
    private Vector2 _clamp;
    private Vector2 _lookInput;
    private Vector2 _lookInputDir;
    private readonly float _zDist;
    private readonly float _collisionDistance;
    public ThirdPersonCamera(Camera camera, Vector3 offset, LayerMask collisionLayers, LayerMask playerLayer, float damping, float collisionDistance, Vector2 clamp)
    {
        _transform = camera.transform;
        _collisionLayers = collisionLayers;
        _playerLayer = playerLayer;
        _offset = offset;
        _clamp = clamp;
        _zDist = Mathf.Abs(offset.z);
        _collisionDistance = collisionDistance;
    }

    public void Move(AB_MB_Mount mount)
    {
        MoveToRotation();
    }
    private void MoveToRotation()
    {
        _targetRotation.x = _lookInputDir.y;
        _targetRotation.y = _lookInputDir.x;

        _transform.localEulerAngles = _targetRotation;

        _transform.localPosition = Vector3.up * _offset.y + Vector3.right * _offset.x + _transform.localRotation * -Vector3.forward * _zDist;

        Physics.queriesHitBackfaces = true;
        Physics.Linecast(_playerPosition + _playerPosition.normalized, _transform.position, out RaycastHit hit);
        if (hit.point != Vector3.zero)
            _transform.position = hit.point;


    }

    public void UpdateInput(Vector2 lookInput, Vector2 lookInputDir, Vector3 playerPos)
    {
        _lookInput = lookInput;
        _lookInputDir = lookInputDir;
        _playerPosition = playerPos;
    }
}
