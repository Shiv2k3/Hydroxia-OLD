using Sirenix.OdinInspector;
using UnityEngine;

public class AB_MB_Mount : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 1;
    [SerializeField] protected float maxMoveSpeed = 1;
    [SerializeField] private MB_Movement mounter;
    [SerializeField, ReadOnly] private bool mounted;
    public virtual void ToggleMount(InteractionCard interactCard)
    {
        mounter = interactCard.playerMovement;
        mounter.ToggleMounted(mounted = !mounted);
    }
    public virtual void Move()
    {

    }
}