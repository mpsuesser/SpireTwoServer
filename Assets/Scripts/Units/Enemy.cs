using System;using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Unit {
    public abstract Enemies Type { get; protected set; }

    // Abilities
    public override abstract List<Ability> Abilities { get; protected set; }
    public bool GetAvailableAbility(out Ability _returnedAbility) {
        foreach (Ability _a in Abilities) {
            if (_a.Available()) {
                _returnedAbility = _a;
                return true;
            }
        }

        _returnedAbility = null;
        return false;
    }

    public EnemyState State { get; set; }
    public delegate EnemyState GetState(Enemy _e, Unit _u = null);
    public Dictionary<EnemyStateID, GetState> StateReferences { get; private set; }

    private static readonly float RoamingMoveSpeedMultiplier = EnemyConstants.ROAMING_MOVESPEED_MULTIPLIER;
    public override float AdjustedMoveSpeed {
        get {
            float ms = base.AdjustedMoveSpeed;
            if (State.ID == EnemyStateID.Roaming) {
                ms *= RoamingMoveSpeedMultiplier;
            }

            return ms;
        }
    }
    public float AggroRadius { get; protected set; }
    public float AttackRange { get; protected set; }

    public Vector3 OriginalPosition { get; private set; }

    protected override float GCDTime { get; set; }

    private Coroutine cooldownCoroutine;

    protected override void Initialize() {
        OriginalPosition = transform.position;

        GCDTime = AbilityConstants.ENEMY_GCD_TIME;
        IsGCDActive = false;

        AfterDamage += Damaged;

        // Since some enemies have unique states, we require each subclass to define their own state lists.
        StateReferences = new Dictionary<EnemyStateID, GetState>() {
            { EnemyStateID.Resting, GetRestingState },
            { EnemyStateID.Roaming, GetRoamingState },
            { EnemyStateID.Chasing, GetChasingState },
            { EnemyStateID.Attacking, GetAttackingState },
            { EnemyStateID.Dead, GetDeadState }
        };

        State = StateReferences[EnemyStateID.Resting](this);

        InitializeEnemy();

        EventHandler.instance.EnemySpawned(this);
    }

    protected abstract void InitializeEnemy();

    protected abstract EnemyState GetRestingState(Enemy _e, Unit _u = null);
    protected abstract EnemyState GetRoamingState(Enemy _e, Unit _u = null);
    protected abstract EnemyState GetChasingState(Enemy _e, Unit _u = null);
    protected abstract EnemyState GetAttackingState(Enemy _e, Unit _u = null);
    protected abstract EnemyState GetDeadState(Enemy _e, Unit _u = null);

    public virtual void FixedUpdate() {
        State.Update();

        ServerSend.EnemyPositionUpdate(ID, transform.position, transform.rotation);
    }

    public void Damaged(float _dmg) {
        ServerSend.EnemyStatusUpdate(ID, Health);

        if (Health <= 0f) {
            State.Transition(EnemyStateID.Dead);
        }
    }
}
