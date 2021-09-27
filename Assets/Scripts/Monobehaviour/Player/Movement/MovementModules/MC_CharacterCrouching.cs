using UnityEngine;

public class MC_CharacterCrouching
{
    private readonly Rigidbody _rb;
    private readonly int _myPlanetID;

    private readonly float _divePower;
    private readonly float _diveCoolDown;
    private float _lastDiveTime;

    public MC_CharacterCrouching(Rigidbody rb, float divePower, float diveCooldown, int planetID)
    {
        _rb = rb;
        _myPlanetID = planetID;

        _divePower = divePower;
        _diveCoolDown = diveCooldown;
    }

    void Dive()
    {
        _rb.AddForce(_rb.position.normalized * -_divePower * Time.fixedDeltaTime, ForceMode.Impulse);
    }

    public void Update(float value)
    {
        if (value != 0)
        {
            if (BodyHelper.IsBodyUnderWater(_myPlanetID) && CanDive())
            {
                Dive();
                return;
            }
        }
    }
    private bool CanDive()
    {
        if (Time.time > _lastDiveTime + _diveCoolDown)
        {
            _lastDiveTime = Time.time;
            return true;
        }
        else return false;
    }
}