using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Debuff : Buff {
    protected Debuff(Unit _u, BuffID _buffId, float _duration) : base(_u, _buffId, _duration) {}
}
