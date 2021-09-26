using Sirenix.OdinInspector;
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
    [HideInInspector] protected InteractionCard interactCard;
    [HideInInspector] protected Rigidbody _rb;
    #endregion
    #region MB
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        BodyManager.Instance.AddNewBody(_rb);
    }

    private void Update()
    {
        if (interactCard != null)
            if (interactCard.playerActions.Player.Mount.triggered)
            {
                Debug.Log("Dismount");
                Dismount();
            }
    }
    #endregion
    #region TOGGLES
    public virtual void ToggleMount(InteractionCard interactCard)
    {
        Debug.Log("Toggle");
        if (this.interactCard.playerMovement == null)
        {
            this.interactCard = interactCard;
            this.interactCard.playerMovement.ToggleMounted(this);
            OnMount();
        }
    }
    protected virtual void OnMount()
    {
        Debug.Log("Mount");
        _mounter = interactCard.playerMovement.transform;
        _mounter.parent = _saddle;
        _mounter.position = _saddle.position;
    }
    protected virtual void Dismount()
    {
        _mounter.parent = null;
        _mounter.position += transform.TransformVector(Vector3.right);

        interactCard = null;
        _mounter = null;
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