using UnityEngine;

public class BodyHelper
{
    public static bool IsBodyUnderWater(int id) { return !BodyManager.IsBodyUnderWater(id); }

    public static float DistFromOceanSurface(Rigidbody _rb, LayerMask _waterLayer)
    {
        bool defualtHitBackFaces = Physics.queriesHitBackfaces;
        Physics.queriesHitBackfaces = false;

        Ray ray = new Ray(_rb.position, -_rb.position.normalized);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _waterLayer))
        {
            Physics.queriesHitBackfaces = defualtHitBackFaces;
            return hit.distance;
        }
        Physics.queriesHitBackfaces = true;

        ray.direction = _rb.position;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _waterLayer))
        {
            Physics.queriesHitBackfaces = defualtHitBackFaces;
            return hit.distance;
        }
        Physics.queriesHitBackfaces = defualtHitBackFaces;
        Debug.Log("Couldn't find an ocean, maybe water layer isn't set in faux gravity, returning 0", _rb);
        return 0;
    }

    public static bool IsGrounded(Rigidbody _rb, LayerMask _groundLayers)
    {
        Vector3 up = _rb.transform.up;
        Ray downRay = new Ray(_rb.position + (up / 1.47f), -up);// overlap will be a little under the player's feet with .45f

        if (Physics.Raycast(downRay, .8f, _groundLayers))
        {
            if (Physics.CheckSphere(downRay.origin, .8f, _groundLayers))
                return true;
            else
                return false;
        }
        else
        {
            return false;
        }
    }
}

