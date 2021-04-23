using UnityEngine;
using System.Collections;

public class RestingStateNoRoam : EnemyState {
    public override EnemyStateID ID { get => EnemyStateID.Resting; }

    public RestingStateNoRoam(Enemy _e) : base(_e) {}

    public override void Update() {
        CheckForHeroInProximity();
    }
}