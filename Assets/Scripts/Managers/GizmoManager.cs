using System.Collections.Generic;
using UnityEngine;

public class GizmoManager : MonoBehaviour
{
    private static List<SphereGizmo> spheres;

    private void Awake()
    {
        spheres = new List<SphereGizmo>();
    }

    public static void AddSphere(SphereGizmo gizmoInfo)
    {
        spheres.Add(gizmoInfo);
    }
    public static void UpdateSpherePosition(int sphereIndex, Vector3 position)
    {
        spheres[sphereIndex].center = position;
    }


    void OnDrawGizmos()
    {
        if (spheres != null)
            for (int i = 0; i < spheres.Count; i++)
            {
                Gizmos.color = spheres[i].color;
                Gizmos.DrawSphere(spheres[i].center, spheres[i].radius);
            }
    }
}

public class SphereGizmo
{
    public Color color;
    public Vector3 center;
    public float radius;

    public SphereGizmo(Color color, Vector3 center, float radius)
    {
        this.color = color;
        this.center = center;
        this.radius = radius;
    }
}
