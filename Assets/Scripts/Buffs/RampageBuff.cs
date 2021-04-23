using UnityEngine;

public class RampageBuff : Buff {
    private Abilities.Atlas.Overload overloadAbilityReference;
    private Abilities.Atlas.Leap leapAbilityReference;
    private float originalOverloadCost;
    private float originalLeapCooldown;

    public RampageBuff(Unit _u) : base(_u, BuffID.Rampage, AbilityConstants.RAMPAGE_DURATION) {
        Hero h = _u as Hero;

        overloadAbilityReference = GetOverloadAbilityInstance(h);
        leapAbilityReference = GetLeapAbilityInstance(h);

        if (overloadAbilityReference == null || leapAbilityReference == null) {
            Debug.Log("An ability reference was null in the Rampage constructor!");
        }

        originalOverloadCost = overloadAbilityReference.ManaCost;
        overloadAbilityReference.ManaCost = 0f;

        originalLeapCooldown = leapAbilityReference.Cooldown;
        leapAbilityReference.Cooldown = 0f;
    }

    protected override void Purged() {
        overloadAbilityReference.ManaCost = originalOverloadCost;
        leapAbilityReference.Cooldown = originalLeapCooldown;
    }

    private Abilities.Atlas.Overload GetOverloadAbilityInstance(Hero _h) {
        foreach (Ability _a in _h.Abilities) {
            if (_a is Abilities.Atlas.Overload) {
                return _a as Abilities.Atlas.Overload;
            }
        }

        return null;
    }

    private Abilities.Atlas.Leap GetLeapAbilityInstance(Hero _h) {
        foreach (Ability _a in _h.Abilities) {
            if (_a is Abilities.Atlas.Leap) {
                return _a as Abilities.Atlas.Leap;
            }
        }

        return null;
    }
}