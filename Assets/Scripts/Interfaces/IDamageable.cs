using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public int Hp { get; }
    public int Defence { get; }
    public ToolType ToolType { get; }

    public void Attacked(AttackCard attackCard);
}
