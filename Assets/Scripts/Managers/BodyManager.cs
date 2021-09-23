using System;
using System.Collections.Generic;
using UnityEngine;
public class BodyManager : MonoBehaviour
{
    [Header("Gravity")]
    [SerializeField] private float gravityPower = -9.18f;

    [Header("Water")]
    [SerializeField] private LayerMask _waterLayer;
    [HideInInspector] public static LayerMask WaterLayer { get { return Instance._waterLayer; } }
    // Private
    [SerializeField] private List<Body> bodies;
    private Vector3 force;
    private static BodyManager _instance;
    public static BodyManager Instance
    {
        get
        {
            if (_instance == null)
            {
                return _instance = FindObjectOfType<BodyManager>();
            }
            else
                return _instance;
        }
        private set
        {
            if (_instance == null)
                _instance = value;
            else if (value != null)
                Destroy(value.gameObject);
        }
    }
    public void Awake()
    {
        bodies = new List<Body>(bodies);
    }
    Vector3 bodyVelocity;
    private void FixedUpdate()
    {
        for (int i = 0; i < bodies.Count; i++)
        {
            if (bodies[i].useGravity == false) continue;

            bodyVelocity = bodies[i].rb.worldCenterOfMass.normalized * bodies[i].rb.mass;

            if (bodies[i].AboveWaterCheck)
            {
                if (gravityPower != 0)
                {
                    force = bodyVelocity * gravityPower;
                    bodies[i].rb.AddForce(force);
                    continue;
                }
            }
            else
            {
                force = Vector3.Lerp(force, Vector3.zero, bodies[i]._waterCollisionDamping);
                bodies[i].rb.velocity = force;
                continue;
            }
        }

    }
    public void UpdateBodyRotation(int i)
    {
        Vector3 bodyUp = bodies[i].rb.rotation * Vector3.up;
        Vector3 gravityUp = bodies[i].rb.position.normalized;

        bodies[i].rb.MoveRotation(Quaternion.FromToRotation(bodyUp, gravityUp) * bodies[i].rb.rotation);
    }

    public int AddNewBody(Rigidbody rb, float waterCollisionDamping)
    {
        bodies.Add(new Body(rb, waterCollisionDamping, _waterLayer));
        return bodies.Count - 1;
    }
    public static bool IsBodyUnderWater(int index)
    {
        if (Instance)
            return Instance.bodies[index].AboveWaterCheck;
        else return false;
    }
    public void ToggleUseGravity(int i, bool state) => bodies[i].useGravity = state;
    public bool GetUseGravity(int i) => bodies[i].useGravity;
}

[Serializable]
public class Body
{
    public bool AboveWaterCheck { get => AmIAboveWater(); }
    public static LayerMask _waterLayer;
    private Ray downRay;
    public Rigidbody rb;
    public Vector3 centerOfMassInWorld;
    public float _waterCollisionDamping = .09f;
    public bool useGravity = true;

    private Vector3 massCenter;

    public Body(Rigidbody rb, float waterCollisionDamping) // STANDARD CONSTRUCTER
    {
        this.rb = rb;
        massCenter = rb.centerOfMass;
        _waterCollisionDamping = waterCollisionDamping;
    }

    public Body(Rigidbody rb, float waterCollisionDamping, LayerMask newWaterLayer) // IF WE NEED TO ASSIGN A NEW WATER LAYER
    {
        this.rb = rb;
        massCenter = rb.centerOfMass;
        _waterLayer = newWaterLayer;
        _waterCollisionDamping = waterCollisionDamping;
    }

    private void UpdatePos()
    {
        centerOfMassInWorld = rb.position + (rb.rotation * massCenter);
        downRay.origin = centerOfMassInWorld;
        downRay.direction = -centerOfMassInWorld.normalized;

        Debug.DrawRay(downRay.origin, downRay.direction * 10, Color.green);
    }
    private int lastFrameChecked;
    private bool lastCheckState;

    private bool AmIAboveWater()
    {
        if (lastFrameChecked != Time.frameCount)
        {
            lastFrameChecked = Time.frameCount;
            UpdatePos();
            return lastCheckState = Physics.Raycast(downRay, Vector3.Distance(downRay.origin, Vector3.zero), _waterLayer);
        }

        return lastCheckState;
    }
}