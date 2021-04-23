using UnityEngine;

public class AttackingState : EnemyState {
    public override EnemyStateID ID { get => EnemyStateID.Attacking; }

    public Unit Attacking { get; private set; }

    public AttackingState(Enemy _e, Unit _u) : base(_e) {
        Attacking = _u;
    }

    public override void Update() {
        Vector3 targetPosSameY = new Vector3(Attacking.transform.position.x, E.transform.position.y, Attacking.transform.position.z);
        E.transform.rotation = Quaternion.LookRotation((targetPosSameY - E.transform.position).normalized);

        // If we're casting, we don't want to interrupt that even if other abilities may be off cooldown or our target runs out of the initial attack range.
        if (E.Casting) {
            return;
        }

        // If the target is out of range, we need to catch up to it.
        // TODO: Convert this to be per-ability.
        if (TargetIsOutOfAttackRange) {
            Transition(EnemyStateID.Chasing, Attacking);
            return;
        }

        // If we're ready to attack, then get the highest priority available ability. If no abilities are available (GCD is active or all abilities are on CD), then do nothing.
        Ability a;
        if (!E.GetAvailableAbility(out a)) {
            return;
        }

        // Execute that ability, which entails putting it on CD and triggering the GCD.
        a.Execute();
    }

    private bool TargetIsOutOfAttackRange {
        get {
            return (E.transform.position - Attacking.transform.position).magnitude > E.AttackRange;
        }
    }
}