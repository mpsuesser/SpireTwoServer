using UnityEngine;
using System.Collections;

public abstract class EnemyAbility : Ability {
    public Enemy E { get; private set; }

    public EnemyAbility(Enemy _e) : base(_e) {
        E = _e;
    }

    public override void Done() {
        PutOnCooldown();
    }
}