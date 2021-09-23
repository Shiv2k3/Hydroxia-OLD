using UnityEngine;

public class CharacterJumping
{
    private LayerMask _jumpableLayers;
    private readonly Rigidbody _rb;
    private readonly int _myPlanetID;

    private float _lastJumped;
    private readonly float _jumpPower;
    private readonly float _jumpCoolDown;

    private float _lastSwamUp;
    private readonly float _swimUpPower;
    private readonly float _swimUpCoolDown;
    public CharacterJumping(Rigidbody rb, float jumpPower, float swimUpPower, float jumpCoolDown, float swimUpCoolDown, LayerMask jumpableLayers, int planetID)
    {
        _rb = rb;
        _jumpableLayers = jumpableLayers;
        _myPlanetID = planetID;

        _jumpPower = jumpPower;
        _jumpCoolDown = jumpCoolDown;

        _swimUpPower = swimUpPower;
        _swimUpCoolDown = swimUpCoolDown;
    }

    void Jump()
    {
        _rb.AddForce(_rb.position.normalized * _jumpPower, ForceMode.Impulse);
    }

    void Rise()
    {
        _rb.AddForce(_rb.position.normalized * _swimUpPower, ForceMode.Force);
    }

    public void Update(float value)
    {
        if (value != 0)
        {
            if (BodyHelper.IsBodyUnderWater(_myPlanetID) && CanSwimUp())
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

