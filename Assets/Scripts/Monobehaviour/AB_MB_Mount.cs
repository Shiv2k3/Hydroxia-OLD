using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class AB_MB_Mount : MonoBehaviour
{
    #region PARAMETERS
    [FoldoutGroup("References")]
    [SerializeField] private Transform saddle;
    [FoldoutGroup("References")]
    [SerializeField, ReadOnly] private Transform mounter;

    [FoldoutGroup("Movement Settings")]
    [SerializeField] protected float _moveSpeed = 1;
    [FoldoutGroup("Movement Settings")]
    [SerializeField] protected float _maxMoveSpeed = 1;
    [FoldoutGroup("Movement Settings")]
   
    // PRIVATE
    [HideInInspector] protected InteractionCard _interactCard;
    [HideInInspector] protected Rigidbody _rb; 
    #endregion
    #region START
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        BodyManager.Instance.AddNewBody(_rb);
    }
    #endregion
    #region TOGGLES
    public virtual void OnMount(InteractionCard interactCard)
    {
        interactCard.playerMovement.ToggleMounted(this);
        mounter = interactCard.playerMovement.transform;
        mounter.parent = saddle;
        mounter.position = saddle.position;
    }
    public virtual void OffMount()
    {
        _interactCard.playerMovement.ToggleMounted(null);
        mounter.parent = null;
        mounter.position += transform.TransformVector(Vector3.right);
    }
    #endregion
    #region MOVEMENT METHODS
    public virtual void Move()
    {
        Debug.Log("Mount Move");
    }
    public virtual void Jump()
    {
        Debug.Log("Mount Jump");
    }
    public virtual void Crouch()
    {
        Debug.Log("Mount Crouch");
    }
    public virtual void Grapple()
    {
        Debug.Log("Mount Grapple");
    }
    public virtual void Fly()
    {
        Debug.Log("Mount Fly");
    }
    #endregion

    
}