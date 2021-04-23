using UnityEngine;

public class RighteousFuryBuff : Buff {
    private static float Dur = EnemyConstants.LEON_RIGHTEOUS_FURY_DURATION;
    public RighteousFuryBuff(Unit _u) : base(_u, BuffID.RighteousFury, Dur) { }

    private static float mxDamageDealt = EnemyConstants.LEON_RIGHTEOUS_FURY_DAMAGE_DEALT_MULTIPLIER;
    public override float DamageDealtMultiplier(float _dmg) {
        return _dmg * mxDamageDealt;
    }

    public static float mxDamageTaken = EnemyConstants.LEON_RIGHTEOUS_FURY_DAMAGE_TAKEN_MULTIPLIER;
    public override float DamageTakenMultiplier(float _dmg) {
        return _dmg * mxDamageTaken;
    }
}