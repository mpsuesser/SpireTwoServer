using UnityEngine;

public class HeroConstants
{
    public readonly static float JUMP_STRENGTH = 5f;
    public readonly static float JUMP_BUFFER = 0.1f;

    // How often regen ticks occur
    public readonly static float REGEN_INTERVAL = 1f;

    // Max / starting health
    public readonly static float ATLAS_HEALTH = 400f;
    public readonly static float PRIESTESS_HEALTH = 150f;

    // Health regen per second
    public readonly static float ATLAS_HEALTH_REGEN = 2f;
    public readonly static float PRIESTESS_HEALTH_REGEN = 1f;

    public readonly static float ATLAS_MOVE_SPEED = 10f;
    public readonly static float PRIESTESS_MOVE_SPEED = 8f;

    // Max / starting mana
    public readonly static float ATLAS_MAX_RAGE = 120f;
    public readonly static float PRIESTESS_MAX_MANA = 200f;

    // Mana regen per second
    public readonly static float ATLAS_MANA_REGEN = 1f;
    public readonly static float PRIESTESS_MANA_REGEN = 4f;

    public readonly static Vector3[] SPAWN_LOCATIONS = new Vector3[] {
        new Vector3(3f, 0f, 65f),
        new Vector3(-3f, 0f, 65f)
    };

    public readonly static Quaternion SPAWN_ROTATION = Quaternion.Euler(0, 180, 0);
}
