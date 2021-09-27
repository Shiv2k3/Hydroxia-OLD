using UnityEngine;

public class MC_MountMovement
{
    private Rigidbody mount;

    public MC_MountMovement(Rigidbody mount)
    {
        this.mount = mount;
    }

    private Vector3 _moveInput;

    public void Update(Vector2 moveInput)
    {
        _moveInput.x = moveInput.x;
        _moveInput.z = moveInput.y;

        mount.position += _moveInput;
    }

}
