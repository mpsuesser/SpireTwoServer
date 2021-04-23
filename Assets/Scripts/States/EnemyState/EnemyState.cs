using System;
using UnityEngine;

public abstract class EnemyState {
    public abstract EnemyStateID ID { get; }

    protected Enemy E;

    protected EnemyState(Enemy _e) {
        E = _e;
    }

    public virtual void Update() { }

    public void Transition(EnemyStateID _stateId, Unit _u = null) {
        E.State.Leave();

        E.State = E.StateReferences[_stateId](E, _u);

        EventHandler.instance.EnemyStateChanged(E);
    }

    public virtual void Leave() {}

    protected bool CheckForHeroInProximity() {
        foreach (Hero h in GameState.instance.HeroesSpawned) {
            // Check if the hero is within this enemy's aggro radius
            float dist = (E.transform.position - h.transform.position).magnitude;
            if (dist > E.AggroRadius) {
                continue;
            }

            // Check if there are any objects of LOS layermask blocking our LOS
            if (Physics.Raycast(E.transform.position, h.transform.position - E.transform.position, dist, Constants.LINEOFSIGHT_LAYER_MASK)) {
                continue;
            }

            // If both of those conditions check out, let's transition to the appropriate state based on distance
            if (dist < E.AttackRange) {
                Transition(EnemyStateID.Attacking, h);
            } else {
                Transition(EnemyStateID.Chasing, h);
            }

            return true;
        }

        return false;
    }
}