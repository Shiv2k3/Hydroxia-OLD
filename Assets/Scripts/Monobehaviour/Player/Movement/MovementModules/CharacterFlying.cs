using UnityEngine;

public class CharacterFlying
{
    private float _maxSpeed;
    private float _speed;
    private Rigidbody _rb;
    private float _cooldown;
    private float _lastFlapped;
    private Vector3 lastPos;
    public void Update(AB_MB_Mount mount, float input, bool planet)
    {
        if (mount)
        {
            mount.Fly();
            return;
        }

        if (input == 1)
        {
            if (FlyReady)
            {
                // SAME VALUE AS IF STATEMENT
                // Vector3 dir = (lastPos - _rb.position).normalized;
                // float mag = Vector3.Distance(Vector3.up, dir) > Vector3.Distance(Vector3.down, dir) ? 1 : -1;
                // float currSpeed = mag * _rb.velocity.magnitude; 
                // SAME VALUE AS IF STATEMENT

                if ((Vector3.Distance(Vector3.up, (lastPos - _rb.position).normalized) > Vector3.Distance(Vector3.down, (lastPos - _rb.position).normalized) ? 1 : -1) * _rb.velocity.magnitude < _maxSpeed) // SAME VALUE AS COMMENTED ABOVE
                {
                    _rb.AddForce((planet ? _rb.position.normalized : Vector3.up) * _speed, ForceMode.VelocityChange);

                    _lastFlapped = Time.time;
                }
            }
        }

        lastPos = _rb.position;
    }
    bool FlyReady => Time.time > _lastFlapped + _cooldown;

    public CharacterFlying(float speed, float maxSpeed, Rigidbody rigidbody, float cooldown)
    {
        _rb = rigidbody;
        _maxSpeed = maxSpeed;
        _speed = speed;
        _cooldown = cooldown;
    }
}
