using UnityEngine;

public class RighteousDefenseBuff : Buff {
    private static float Dur = EnemyConstants.LEON_RIGHTEOUS_DEFENSE_DURATION;
    public RighteousDefenseBuff(Unit _u) : base(_u, BuffID.RighteousDefense, Dur) { }

    private static float mxDamageDealt = EnemyConstants.LEON_RIGHTEOUS_DEFENSE_DAMAGE_DEALT_MULTIPLIER;
    public override float DamageDealtMultiplier(float _dmg) {
        return _dmg * mxDamageDealt;
    }

    public static float mxDamageTaken = EnemyConstants.LEON_RIGHTEOUS_DEFENSE_DAMAGE_TAKEN_MULTIPLIER;
    public override float DamageTakenMultiplier(float _dmg) {
        return _dmg * mxDamageTaken;
    }
}