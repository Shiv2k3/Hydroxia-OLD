using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
public class BodyManager : MonoBehaviour
{
    #region SINGLETON
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
    [HideInInspector] public static LayerMask WaterLayer { get { return Instance._waterLayer; } }
    #endregion
    #region PARAMETERS
    [FoldoutGroup("Settings")]
    [SerializeField] private float gravityPower = -9.18f;
    [FoldoutGroup("Settings")]
    [SerializeField] private LayerMask _waterLayer;
    [FoldoutGroup("Settings")]
    [SerializeField] private float waterCollisionDamping = 0.05f;
    [FoldoutGroup("Settings")]
    [SerializeField] private bool onPlanet = false;

    [FoldoutGroup("Bodies Being Handled")]
    [SerializeField, ReadOnly, HideInEditorMode] private List<Body> bodies;

    #endregion
    #region MB
    void Awake()
    {
        bodies = new List<Body>(bodies);
    }
    void FixedUpdate()
    {
        for (int i = 0; i < bodies.Count; i++)
        {
            if (bodies[i].useGravity == false) continue;

            Vector3 bodyVelocity = (onPlanet ? bodies[i].rb.worldCenterOfMass.normalized : Vector3.up) * bodies[i].rb.mass;

            if (bodies[i].AboveWaterCheck)
            {
                if (gravityPower != 0)
                {
                    Vector3 force = bodyVelocity * gravityPower;
                    bodies[i].rb.AddForce(force, ForceMode.Force);
                    continue;
                }
            }
            else
            {
                Vector3 velocity = Vector3.Lerp(bodies[i].rb.velocity, Vector3.zero, waterCollisionDamping);
                bodies[i].rb.velocity = velocity;
                continue;
            }
        }

    }
    #endregion
    #region METHODS
    public int AddNewBody(Rigidbody rb)
    {
        bodies.Add(new Body(rb, _waterLayer));
        return bodies.Count - 1;
    }
    public bool IsBodyUnderWater(int index)
    {
        if (Instance)
            return Instance.bodies[index].AboveWaterCheck;
        else return false;
    }
    public void ToggleUseGravity(int i, bool state) => bodies[i].useGravity = state;
    public bool GetUseGravity(int i) => bodies[i].useGravity;
    public bool GetOnPlanet() => onPlanet;
    #endregion
}

[Serializable]
public class Body
{
    public bool AboveWaterCheck { get => AmIAboveWater(); }
    public static LayerMask _waterLayer;
    private Ray downRay;
    public Rigidbody rb;
    public Vector3 centerOfMassInWorld;
    public bool useGravity = true;

    private Vector3 massCenter;

    public Body(Rigidbody rb) // STANDARD CONSTRUCTER
    {
        this.rb = rb;
        massCenter = rb.centerOfMass;
    }

    public Body(Rigidbody rb, LayerMask newWaterLayer) // IF WE NEED TO ASSIGN A NEW WATER LAYER
    {
        this.rb = rb;
        massCenter = rb.centerOfMass;
        _waterLayer = newWaterLayer;
    }

    private void UpdatePos()
    {
        centerOfMassInWorld = rb.position + (rb.rotation * massCenter);
        downRay.origin = centerOfMassInWorld;
        downRay.direction = -centerOfMassInWorld.normalized;
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