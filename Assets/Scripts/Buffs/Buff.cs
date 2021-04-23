using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff
{
    public BuffID ID { get; private set; }

    public Unit Attached { get; private set; }

    public float MaxDuration { get; private set; }
    public float DurationRemaining { get; private set; }

    private Coroutine buffTimerCoroutine;

    protected Buff(Unit _u, BuffID _buffId, float _duration) {
        ID = _buffId;

        Attached = _u;
        MaxDuration = _duration;
        DurationRemaining = _duration;
        
        if (_duration > 0) { // -1f should be passed into the constructor as duration for a non-timed buff
            buffTimerCoroutine = CoroutineRunner.instance.Run(BuffTimer());
        }
    }

    protected void Expire() {
        Purge();
    }

    public void Purge() {
        Attached.RemoveBuffReference(this);

        if (buffTimerCoroutine != null) {
            CoroutineRunner.instance.Stop(buffTimerCoroutine);
        }

        Purged();
    }

    protected virtual void Purged() {
        // For if a buff has cleanup to do or some effect
    }

    public IEnumerator BuffTimer() {
        Debug.Log("BuffTimer called!");
        while (DurationRemaining > 0f) {
            DurationRemaining -= Time.deltaTime;

            yield return null;
        }
        Debug.Log("BuffTimer expiring!");

        this.Expire();
    }

    #region Multipliers
    public virtual float DamageDealtMultiplier(float _dmg) {
        return _dmg;
    }

    public virtual float DamageTakenMultiplier(float _dmg) {
        return _dmg;
    }

    public virtual float MovementSpeedMultiplier(float _moveSpeed) {
        return _moveSpeed;
    }

    public virtual float AbilityCostMultiplier(float _cost) {
        return _cost;
    }

    public virtual float ManaGainMultiplier(float _gain) {
        return _gain;
    }
    #endregion
}
