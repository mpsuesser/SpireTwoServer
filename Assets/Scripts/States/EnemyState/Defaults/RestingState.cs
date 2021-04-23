using UnityEngine;
using System.Collections;

public class RestingState : EnemyState {
    public override EnemyStateID ID { get => EnemyStateID.Resting; }

    private readonly float RestDuration = 2f;

    public RestingState(Enemy _e) : base(_e) {
        CoroutineRunner.instance.Run(TransitionToRoaming());
    }

    private IEnumerator TransitionToRoaming() {
        float waitTime = RestDuration;
        while (waitTime > 0f) {
            waitTime -= Time.deltaTime;

            if (CheckForHeroInProximity()) {
                yield break;
            }

            yield return null;
        }

        Transition(EnemyStateID.Roaming);
    }
}