using System;
using System.Collections;
using UnityEngine;

public abstract class Ability {
    // Hero who ability belongs to
    protected Unit U;
    protected bool UnitIsHero => U is Hero;

    // Ability cost / gain
    public float ManaCost { get; set; }
    public float AdjustedManaCost {
        get {
            float mc = ManaCost;
            foreach (Buff b in U.Buffs) {
                mc = b.AbilityCostMultiplier(mc);
            }

            return mc;
        }
    }

    public float ManaGain { get; protected set; }
    public float AdjustedManaGain {
        get {
            float mg = ManaGain;
            foreach (Buff b in U.Buffs) {
                mg = b.ManaGainMultiplier(mg);
            }

            return mg;
        }
    }

    // The initial cooldown value after the ability has been used.
    public float Cooldown { get; set; }
    public bool OnCooldown { get; private set; }

    private Coroutine cooldownCoroutine;

    protected Ability(Unit _u) {
        U = _u;

        // Defaults, to be set by the child class constructor where necessary
        ManaCost = 0f;
        ManaGain = 0f;
        Cooldown = 0f;

        OnCooldown = false;
    }

    public virtual bool Available() {
        if (OnCooldown) {
            return false;
        }

        if (U.IsGCDActive) {
            return false;
        }

        return true;
    }

    public abstract void Execute();
    public abstract void Done();

    protected void PutOnCooldown(bool _ignoreGCD = false, float _overrideTime = -1f) {
        if (!_ignoreGCD) {
            U.StartGCD();
        }

        // Added the _overrideTime just because Leon has a special use case where he overwrites the cooldown of Righteous Defense after Righteous Fury is executed.
        if (OnCooldown && _overrideTime < 0f) {
            Debug.Log("Cooldown was active when PutOnCooldown() was called, this should not happen!");
        }

        if (_overrideTime < 0f) {
            cooldownCoroutine = CoroutineRunner.instance.Run(CooldownTimer(Cooldown));
        } else {
            cooldownCoroutine = CoroutineRunner.instance.Run(CooldownTimer(_overrideTime));
        }
    }

    private IEnumerator CooldownTimer(float _cdTime) {
        OnCooldown = true;

        yield return new WaitForSeconds(_cdTime);

        OnCooldown = false;
    }

    public void TakeOffCooldown() {
        if (OnCooldown) {
            CoroutineRunner.instance.Stop(cooldownCoroutine);
        }

        OnCooldown = false;
    }

    public virtual void Reset() {

    }
}
