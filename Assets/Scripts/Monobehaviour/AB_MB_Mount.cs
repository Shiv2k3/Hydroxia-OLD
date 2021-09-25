using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class AB_MB_Mount : MonoBehaviour
{
    #region PARAMETERS
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
    public virtual void OnMount(InteractionCard interactCard) => interactCard.playerMovement.ToggleMounted(this);
    public virtual void OffMount() => _interactCard.playerMovement.ToggleMounted(null);

    internal void Grapple()
    {
        throw new NotImplementedException();
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
    public virtual void Fly()
    {
        Debug.Log("Mount Fly");
    }
    public virtual void Crouch()
    {
        Debug.Log("Mount Crouch");
    }
    #endregion

    
}