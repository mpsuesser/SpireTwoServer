using UnityEngine;

public class ThunderousFortitudeBuff : Buff {
    private static float Dur = AbilityConstants.THUNDEROUS_FORTITUDE_DURATION;
    private static float mxDamageTaken = AbilityConstants.THUNDEROUS_FORTITUDE_DAMAGE_TAKEN_MULTIPLIER;
    private static float mxLifesteal = AbilityConstants.THUNDEROUS_FORTITUDE_LIFESTEAL_MULTIPLIER;

    public ThunderousFortitudeBuff(Unit _u) : base(_u, BuffID.ThunderousFortitude, Dur) {}

    public override float DamageTakenMultiplier(float _dmg) {
        return _dmg * mxDamageTaken;
    }

    // public override float LifestealMultiplier(float _pct) {
        // TODO
    // }
}