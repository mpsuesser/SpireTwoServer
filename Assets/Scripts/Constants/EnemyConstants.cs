using UnityEngine;

public class EnemyConstants {
    // The 8th layer, Enemy
    public static readonly int ENEMY_LAYER_MASK = 1 << 8;

    public static float ROAM_DISTANCE = 5f;
    public static float ROAM_POSITION_BUFFER = .5f;
    public static float ROAMING_MOVESPEED_MULTIPLIER = .3f;

    #region Grunt
    public static readonly float GRUNT_HEALTH = 5f;
    public static readonly float GRUNT_MOVE_SPEED = 5f;
    public static readonly float GRUNT_AGGRO_RADIUS = 10f;
    public static readonly float GRUNT_ATTACK_RANGE = 3f;
    public static readonly float GRUNT_SWING_DAMAGE = 5f;
    public static readonly float GRUNT_SWING_SPEED = .5f;
    public static readonly float GRUNT_SWING_FREQUENCY = 3f; // once per 3 seconds
    #endregion

    #region Spellslinger
    public static readonly float SPELLSLINGER_HEALTH = 3f;
    public static readonly float SPELLSLINGER_MOVE_SPEED = 5f;
    public static readonly float SPELLSLINGER_AGGRO_RADIUS = 10f;
    public static readonly float SPELLSLINGER_ATTACK_RANGE = 8f;
    public static readonly float SPELLSLINGER_FIREBALL_DAMAGE = 15f;
    public static readonly float SPELLSLINGER_FIREBALL_CAST_TIME = 3f;
    public static readonly float SPELLSLINGER_FIREBALL_TRAVEL_TIME = .5f;
    public static readonly float SPELLSLINGER_FIREBALL_COOLDOWN = 4f;
    #endregion

    #region Leon
    public static readonly float LEON_HEALTH = 10f;
    public static readonly float LEON_MOVE_SPEED = 8f;
    public static readonly float LEON_AGGRO_RADIUS = 25f;
    public static readonly float LEON_ATTACK_RANGE = 6f;

    public static readonly float LEON_SWING_DAMAGE = 35f;
    public static readonly float LEON_SWING_SPEED = .2f;
    public static readonly float LEON_SWING_FREQUENCY = 4f;

    public static readonly float LEON_RIGHTEOUS_DEFENSE_COOLDOWN = 40f;
    public static readonly float LEON_RIGHTEOUS_DEFENSE_DURATION = 30f; // shouldn't matter much, will be purged when righteous fury is cast
    public static readonly float LEON_RIGHTEOUS_DEFENSE_DELAY = 1f;
    public static readonly float LEON_RIGHTEOUS_DEFENSE_DAMAGE_TAKEN_MULTIPLIER = 0.75f;
    public static readonly float LEON_RIGHTEOUS_DEFENSE_DAMAGE_DEALT_MULTIPLIER = 0.75f;
    public static readonly float LEON_RIGHTEOUS_DEFENSE_MINIMUM_SPACING_TIME = 20f;
    public static readonly float LEON_RIGHTEOUS_DEFENSE_TRANSITION_DAMAGE = 200f;
    public static readonly float LEON_RIGHTEOUS_DEFENSE_TRANSITION_DAMAGE_RADIUS = 10f;

    public static readonly float LEON_RIGHTEOUS_FURY_COOLDOWN = 40f;
    public static readonly float LEON_RIGHTEOUS_FURY_DURATION = 30f; // ...
    public static readonly float LEON_RIGHTEOUS_FURY_DELAY = 0.5f;
    public static readonly float LEON_RIGHTEOUS_FURY_DAMAGE_TAKEN_MULTIPLIER = 1.25f;
    public static readonly float LEON_RIGHTEOUS_FURY_DAMAGE_DEALT_MULTIPLIER = 1.25f;
    #endregion

    #region Frosty
    public static readonly float FROSTY_HEALTH = 100f;
    public static readonly float FROSTY_MOVE_SPEED = 8f;
    public static readonly float FROSTY_AGGRO_RADIUS = 25f;
    public static readonly float FROSTY_ATTACK_RANGE = 6f;

    public static readonly float FROSTY_SWING_DAMAGE = 35f;
    public static readonly float FROSTY_SWING_SPEED = .5f;
    public static readonly float FROSTY_SWING_FREQUENCY = 3f;

    public static readonly float FROSTY_SPIRIT_STRIKE_COOLDOWN = 10f;
    public static readonly float FROSTY_SPIRIT_STRIKE_DELAY = 3f;
    public static readonly float FROSTY_SPIRIT_STRIKE_DAMAGE = 300f;
    public static readonly float FROSTY_SPIRIT_STRIKE_WIDTH = 3f;

    public static readonly float[] FROSTY_SPIRIT_BOMB_PERCENTAGES = new float[] { 80f, 20f }; // should be in descending order
    public static readonly float FROSTY_SPIRIT_BOMB_INITIAL_DELAY = 5f;
    public static readonly float FROSTY_SPIRIT_BOMB_TIME = 3f;
    public static readonly float FROSTY_SPIRIT_BOMB_RADIUS = 4f;
    public static readonly float FROSTY_SPIRIT_BOMB_SUBSEQUENT_DELAY = 6f;
    public static readonly float FROSTY_SPIRIT_BOMB_BASE_AOE_DAMAGE = 100f;
    public static readonly float FROSTY_SPIRIT_BOMB_AOE_MULTIPLIER = 1.5f;
    public static readonly float FROSTY_SPIRIT_BOMB_DAMAGE_TAKEN_MULTIPLIER = 1.5f;
    public static readonly float FROSTY_SPIRIT_BOMB_SOAK_DAMAGE = 300f;
    public static readonly float FROSTY_SPIRIT_BOMB_MAX_DISTANCE = 10f;
    #endregion
}
