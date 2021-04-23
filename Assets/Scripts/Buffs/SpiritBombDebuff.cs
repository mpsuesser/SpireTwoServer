using UnityEngine;

public class SpiritBombDebuff : Debuff {
    private static float Dur = 300f; // arbitrary -- gets purged, doesn't expire
    public SpiritBombDebuff(Unit _u) : base(_u, BuffID.SpiritBomb, Dur) { }

    public static float mxDamageTaken = EnemyConstants.FROSTY_SPIRIT_BOMB_DAMAGE_TAKEN_MULTIPLIER;
    public override float DamageTakenMultiplier(float _dmg) {
        return _dmg * mxDamageTaken;
    }
}