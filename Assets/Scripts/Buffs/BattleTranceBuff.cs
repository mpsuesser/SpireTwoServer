using UnityEngine;

public class BattleTranceBuff : Buff {
    private static float Dur = AbilityConstants.BATTLE_TRANCE_DURATION;
    public BattleTranceBuff(Unit _u) : base(_u, BuffID.BattleTrance, Dur) {}

    private static float mxMoveSpeed = AbilityConstants.BATTLE_TRANCE_MOVEMENT_SPEED_MULTIPLIER;
    public override float MovementSpeedMultiplier(float _moveSpeed) {
        return _moveSpeed * mxMoveSpeed;
    }

    private static float mxDamageDealt = AbilityConstants.BATTLE_TRANCE_DAMAGE_MULTIPLIER;
    public override float DamageDealtMultiplier(float _dmg) {
        return _dmg * mxDamageDealt;
    }

    private static float mxAbilityCost = AbilityConstants.BATTLE_TRANCE_ABILITY_COST_MULTIPLIER;
    public override float AbilityCostMultiplier(float _cost) {
        return _cost * mxAbilityCost;
    }

    private static float mxRageGeneration = AbilityConstants.BATTLE_TRANCE_RAGE_MULTIPLIER;
    public override float ManaGainMultiplier(float _rage) {
        return _rage * mxRageGeneration;
    }
}