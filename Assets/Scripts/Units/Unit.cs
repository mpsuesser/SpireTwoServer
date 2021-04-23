using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class Unit : MonoBehaviour {
    public int ID { get; private set; }

    public abstract List<Ability> Abilities { get; protected set; }

    public float Health { get; protected set; }
    public float MaxHealth { get; protected set; }
    public float HealthPercentage => Health / MaxHealth * 100f;

    public float MoveSpeed { get; protected set; }
    public virtual float AdjustedMoveSpeed {
        get {
            float ms = MoveSpeed;
            foreach (Buff b in Buffs) {
                ms = b.MovementSpeedMultiplier(ms);
            }

            return ms;
        }
    }

    public event Action<float> AfterDamage;
    public event Action Interrupted;

    // TODO, move things like health, move speed, mana, etc here
    // Add Actions e.g. DamageMultipliers() for buffs to perform operations on the initial values
    public List<Buff> Buffs { get; private set; }

    public bool Dead { get; set; }

    public bool Casting { get; set; }
    public bool IsGCDActive { get; set; }
    protected abstract float GCDTime { get; set; }

    public Rigidbody RB { get; private set; }
    private RigidbodyConstraints originalFreeze;

    private void Awake() {
        ID = GameState.instance.GetNewUnitID();

        Buffs = new List<Buff>();

        RB = gameObject.GetComponent<Rigidbody>();
        originalFreeze = RB.constraints;

        Dead = false;
        Casting = false;

        Initialize();
    }

    protected abstract void Initialize();

    #region Buffs
    // overwrite is true by default. If a buff should be able to have multiple instances applied, then make sure to set it as false.
    public void AddBuff(Buff _b, bool overwrite = true) {
        // If overwrite is true, find the first instance of this buff type in our list of buffs and purge it (not expire!) before adding the new buff.
        if (overwrite) {
            Buff existing = Buffs.Find(b => b.GetType() == _b.GetType());
            if (existing != null) {
                existing.Purge();
            }
        }

        Buffs.Add(_b);

        // TODO: include overwrite
        ServerSend.BuffApplied(_b);
    }

    public void RemoveBuffReference(Buff _b) {
        Buffs.Remove(_b);

        ServerSend.BuffPurged(_b);
    }

    public void PurgeBuffOfType<T>() where T : Buff {
        List<Buff> toRemove = new List<Buff>();
        foreach (Buff b in Buffs) {
            if (b is T) {
                toRemove.Add(b);
            }
        }

        foreach (Buff b in toRemove) {
            b.Purge();
        }
    }
    #endregion

    #region Damage
    public void DealDamage(Unit _u, float _dmg) {
        // Apply any multipliers from existing buffs
        foreach (Buff b in Buffs) {
            _dmg = b.DamageDealtMultiplier(_dmg);
        }

        _u.TakeDamage(_dmg);
    }
    public void TakeDamage(float _dmg) {
        // Apply any multipliers from existing buffs
        foreach (Buff b in Buffs) {
            _dmg = b.DamageTakenMultiplier(_dmg);
        }

        if (_dmg > Health) {
            Debug.Log("TODO Unit has died!");
        }

        Health = Mathf.Max(0f, Health - _dmg);

        if (AfterDamage != null) {
            AfterDamage(_dmg);
        }
    }
    #endregion

    #region Movement
    public void MoveTo(Vector3 _location) {
        FreezePosition(false);

        // To ensure we're not floating, only move unit on x/z axes
        _location.y = transform.position.y;

        Vector3 dir = (_location - transform.position).normalized;
        RB.velocity = dir * AdjustedMoveSpeed;

        transform.rotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x,
            Quaternion.LookRotation(dir).eulerAngles.y,
            transform.rotation.eulerAngles.z
        );
    }

    public void StopMoving() {
        RB.velocity = Vector3.zero;
        FreezePosition(true);
    }

    public void FreezePosition(bool freeze) {
        if (freeze) {
            RB.constraints =
                RigidbodyConstraints.FreezePosition
                | RigidbodyConstraints.FreezeRotation;
        } else {
            RB.constraints =
                originalFreeze
                | RigidbodyConstraints.FreezeRotationX
                | RigidbodyConstraints.FreezeRotationZ;
        }
    }
    #endregion

    #region GCD
    public void StartGCD() {
        IsGCDActive = true;

        StartCoroutine(GCDTicker());
    }

    private IEnumerator GCDTicker() {
        yield return new WaitForSeconds(GCDTime);
        IsGCDActive = false;
    }
    #endregion

    public void Interrupt() {
        if (Casting) {
            Casting = false;

            if (Interrupted != null) {
                Interrupted();
            }
        }
    }
}