using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ColiseumDetails : DungeonDetails {
    public override DungeonID ID { get; protected set; }
    public override int EnemiesRequired { get; protected set; }
    public override Dictionary<Enemies, bool> BossesKilled { get; protected set; }
    public override Dictionary<RunMedal, float> MedalTimes { get; protected set; }

    public ColiseumDetails() : base() {
        ID = DungeonID.Coliseum;

        EnemiesRequired = 4;

        BossesKilled = new Dictionary<Enemies, bool>() {
            { Enemies.FROSTY, false },
            { Enemies.LEON, false }
        };

        MedalTimes = new Dictionary<RunMedal, float>() {
            { RunMedal.None, Mathf.Infinity },
            { RunMedal.Bronze, 30f },
            { RunMedal.Silver, 20f },
            { RunMedal.Gold, 10f }
        };
    }
}