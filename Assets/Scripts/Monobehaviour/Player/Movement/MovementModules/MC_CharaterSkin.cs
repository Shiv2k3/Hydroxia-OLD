using UnityEngine;

public class MC_CharaterSkin
{
    private Transform _transform;
    public MC_CharaterSkin(Transform skin)
    {
        _transform = skin;
    }
    float angle;
    public void RotateSkin(Vector2 lookInput, Vector2 moveInput, bool underWater, Transform camera)
    {
        if (moveInput != Vector2.zero)
        {
            if (lookInput.x != 0 && underWater) // ROTATE THE SKIN SO IT'S HEAD IS TOWARDS THE DIRECTION OF MOVEMENT IF UNDER WATER
            {
                Vector3 from = _transform.up;
                Vector3 toForward = camera.forward * moveInput.y;
                Vector3 toRight = _transform.parent.right * moveInput.x;

                _transform.rotation =
                   Quaternion.FromToRotation(from, toForward + toRight) * _transform.rotation;

                return;
            }
            else if (!underWater) // ROTATE SKIN TOWARDS THE DIRECTION OF MOVEMENT IF ON LAND
            {
                angle = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg;
                _transform.localRotation = Quaternion.Slerp(_transform.localRotation,
                    Quaternion.AngleAxis(angle, Vector3.up), .5f);
                return;
            }

        }
        else if (underWater) // ROTATE THE SKIN STRAIGHT UP IF NOTHING IS BEING PRESSED WHILE UNDER WATER
        {
            _transform.localRotation = Quaternion.Slerp(_transform.localRotation, Quaternion.identity, .07f);
        }
    }
}
