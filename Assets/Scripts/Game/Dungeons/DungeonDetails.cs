using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class DungeonDetails {
    public abstract DungeonID ID { get; protected set; }

    public event Action<DungeonDetails> UpdatedDetails;
    public event Action<DungeonDetails> DungeonCompleted;

    // Kill requirements
    public abstract int EnemiesRequired { get; protected set; }
    public int EnemiesKilled { get; protected set; }
    public abstract Dictionary<Enemies, bool> BossesKilled { get; protected set; }

    // Run timer
    public float StartTime { get; protected set; }
    public float TimeElapsed => Time.time - StartTime;
    public abstract Dictionary<RunMedal, float> MedalTimes { get; protected set; }

    public DungeonDetails() {
        EnemiesKilled = 0;

        UpdatedDetails += CheckForCompletion;
    }

    public void StartRun() {
        StartTime = Time.time;

        UpdatedDetails?.Invoke(this);
    }

    public void EnemyKilled(Enemy _e) {
        if (_e is Boss b) {
            if (BossesKilled.ContainsKey(b.Type)) {
                BossesKilled[b.Type] = true;
            } else {
                Debug.Log("A boss was killed but they weren't in the BossesKilled DungeonDetails dictionary!");
            }
        } else {
            EnemiesKilled++;
        }

        UpdatedDetails?.Invoke(this);
    }

    public void BossKilled(Enemies _enemyID) {
        if (!BossesKilled.ContainsKey(_enemyID)) {
            Debug.Log("Boss was killed but it was not registered in dungeon details!");
            return;
        }

        BossesKilled[_enemyID] = true;

        UpdatedDetails?.Invoke(this);
    }

    private void CheckForCompletion(DungeonDetails _dd) {
        // If enemy requirement has not been met
        if (EnemiesKilled < EnemiesRequired) {
            return;
        }

        // If not all bosses have been killed
        // Note: Will need updating if there is an optional boss
        if (BossesKilled.ContainsValue(false)) {
            return;
        }

        DungeonCompleted?.Invoke(this);
    }
}