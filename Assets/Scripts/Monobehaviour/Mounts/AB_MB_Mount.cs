using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class AB_MB_Mount : MonoBehaviour
{
    #region PARAMETERS
    [FoldoutGroup("References")]
    [SerializeField] private Transform _saddle;
    [FoldoutGroup("References")]
    [SerializeField, ReadOnly] private Transform _mounter;

    [FoldoutGroup("Movement Settings")]
    [SerializeField] protected float _moveSpeed = 1;
    [FoldoutGroup("Movement Settings")]
    [SerializeField] protected float _maxMoveSpeed = 1;
    [FoldoutGroup("Movement Settings")]

    // PRIVATE
    [HideInInspector] protected Rigidbody _rb;

    // MOUNTER 
    [HideInInspector] protected MB_Movement player;
    [HideInInspector] protected PlayerMovement movementInput;
    [HideInInspector] protected PlayerInteraction interactionInput;

    // BOOLS
    public bool Mounted { get { return player; } }

    #endregion
    #region SCRIPTS
    private MC_MountMovement movement;
    #endregion
    #region MB
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        BodyManager.Instance.AddNewBody(_rb);

        // SCRIPT INIT
        movement = new MC_MountMovement(_rb);
    }
    private void Update()
    {
        if (Mounted)
        {
            if (interactionInput.Player.Mount.triggered)
                Dismount();
        }
    }
    #endregion
    #region MOVEMENT METHODS
    public virtual void Move()
    {
        movement.Update(player.moveInput);
    }
    public virtual void Jump()
    {
        if (movementInput.Player.Jump.triggered)
            Debug.Log("Mount Jump");
    }
    public virtual void Crouch()
    {
        if (movementInput.Player.Crouch.ReadValue<float>() == 1)
            Debug.Log("Mount Crouch");
    }
    public virtual bool Grapple()
    {
        if (movementInput.Player.Grapple.triggered)
        {
            Debug.Log("Mount Grapple");
        }
        return false;
    }
    public virtual void Fly()
    {
        if (movementInput.Player.Fly.ReadValue<float>() == 1)
            Debug.Log("Mount Fly");
    }
    #endregion
    #region TOGGLES
    public virtual void ToggleMount(InteractionCard interactCard)
    {
        interactCard.playerMovement.Mount = this;

        player = interactCard.playerMovement;
        movementInput = interactCard.playerMovement.inputs;
        interactionInput = interactCard.interactionInput;

        OnMount();
    }
    protected virtual void OnMount()
    {
        _mounter = player.transform;

        _mounter.forward = transform.forward;
        _mounter.parent = _saddle;
        _mounter.position = _saddle.position;
        _mounter.GetComponent<Rigidbody>().isKinematic = true;
    }
    protected virtual void Dismount()
    {
        _mounter.position += transform.TransformVector(Vector3.right);
        _mounter.GetComponent<Rigidbody>().isKinematic = true;

        player.Mount = null;
        _mounter.parent = null;
        _mounter = null;
        movementInput = null;
        interactionInput = null;
        player = null;
    }
    #endregion
}