using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : Enemy
{
    public override abstract Enemies Type { get; protected set; }

    protected override abstract void InitializeEnemy();
}
