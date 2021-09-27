using UnityEngine;

public class MC_CharacterJumping
{
    private LayerMask _jumpableLayers;
    private readonly Rigidbody _rb;
    private readonly int _bodyID;

    private float _lastJumped;
    private readonly float _jumpPower;
    private readonly float _jumpCoolDown;

    private float _lastSwamUp;
    private readonly float _swimUpPower;
    private readonly float _swimUpCoolDown;
    public MC_CharacterJumping(Rigidbody rb, float jumpPower, float swimUpPower, float jumpCoolDown, float swimUpCoolDown, LayerMask jumpableLayers, int bodyID)
    {
        _rb = rb;
        _jumpableLayers = jumpableLayers;
        _bodyID = bodyID;

        _jumpPower = jumpPower;
        _jumpCoolDown = jumpCoolDown;

        _swimUpPower = swimUpPower;
        _swimUpCoolDown = swimUpCoolDown;
    }

    void Jump()
    {
        Vector3 dir = BodyManager.Instance.GetOnPlanet() ? _rb.position.normalized : Vector3.up;
        _rb.AddForce(dir * _jumpPower, ForceMode.Impulse);
    }

    void Rise()
    {
        Vector3 dir = BodyManager.Instance.GetOnPlanet() ? _rb.position.normalized : Vector3.up;
        _rb.AddForce(dir * _swimUpPower, ForceMode.Force);
    }

    public void Update(float value)
    {
        if (value != 0)
        {
            if (BodyHelper.IsBodyUnderWater(_bodyID) && CanSwimUp())
            {
                Rise();
                return;
            }

            if (CanJump() && BodyHelper.IsGrounded(_rb, _jumpableLayers))
            {
                Jump();
                return;
            }
        }
    }
    private bool CanJump()
    {
        if (Time.time >= _lastJumped + _jumpCoolDown)
        {
            _lastJumped = Time.time;
            return true;
        }
        else return false;
    }
    private bool CanSwimUp()
    {
        if (Time.time > _lastSwamUp + _swimUpCoolDown)
        {
            _lastSwamUp = Time.time;
            return true;
        }
        else return false;
    }
}

