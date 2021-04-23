using UnityEngine;

public class ThunderClapDebuff : Debuff {
    private static float Dur = AbilityConstants.THUNDER_CLAP_SLOW_DURATION;
    public ThunderClapDebuff(Unit _u) : base(_u, BuffID.ThunderClap, Dur) { }

    private static float mxMoveSpeed = AbilityConstants.THUNDER_CLAP_SLOW_MULTIPLIER;
    public override float MovementSpeedMultiplier(float _moveSpeed) {
        return _moveSpeed * mxMoveSpeed;
    }
}