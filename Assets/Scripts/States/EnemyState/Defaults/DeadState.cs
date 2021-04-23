using System.Collections;
using UnityEngine;

public class DeadState : EnemyState {
    public override EnemyStateID ID { get => EnemyStateID.Dead; }
    private static float Delay = 5f;

    public DeadState(Enemy _e) : base(_e) {
        EventHandler.instance.EnemyKilled(E);

        CoroutineRunner.instance.Run(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay() {
        yield return new WaitForSeconds(Delay);

        Object.Destroy(E.gameObject);
    }
}