using UnityEngine;

public static class Constants
{
    #region Version
    public readonly static string VERSION = "0.0.1";
    #endregion

    #region Server
    public readonly static int SERVER_LISTEN_PORT = 26950;
    public readonly static int TICKS_PER_SEC = 30;
    public readonly static int MS_PER_TICK = 1000 / TICKS_PER_SEC;
    #endregion
    
    public static readonly string GROUND_TAG = "Ground";
    public static readonly int HERO_LAYER_MASK = 1 << 10;
    public static readonly int LINEOFSIGHT_LAYER_MASK = 1 << 9;

    // General projectile speeds for basically every spell projectile
    public static readonly float SPELL_PROJECTILE_SPEED = 25f;

    public static readonly string HEROES_PARENT_TAG = "HeroesParent";
    public static readonly string ENEMIES_PARENT_TAG = "EnemiesParent";

    // Find a new path every x seconds
    public static readonly float PATHFINDING_TICK = .5f;
    public static readonly float WAYPOINT_BUFFER = 0.5f;
}