using System.Collections;
using UnityEngine;

public class RoamingState : EnemyState {
    public override EnemyStateID ID { get => EnemyStateID.Roaming; }

    private Vector3 destination;

    private static readonly float RoamDistance = EnemyConstants.ROAM_DISTANCE;
    private static readonly float PositionBuffer = EnemyConstants.ROAM_POSITION_BUFFER;
    private bool AtOriginalPosition => (E.transform.position - E.OriginalPosition).magnitude < PositionBuffer;
    private bool FarAwayFromOriginalPosition => (E.transform.position - E.OriginalPosition).magnitude > (RoamDistance - PositionBuffer);

    private enum RoamStatus {
        Chilling,
        MovingTowardDestination,
        MovingTowardOriginalPosition
    }
    private RoamStatus roamStatus;

    private Coroutine roamCoroutine;

    public RoamingState(Enemy _e) : base(_e) {
        roamStatus = RoamStatus.Chilling;

        roamCoroutine = CoroutineRunner.instance.Run(Roam());
    }

    public override void Update() {
        if (CheckForHeroInProximity()) {
            return;
        }
    }

    public override void Leave() {
        CoroutineRunner.instance.Stop(roamCoroutine);
        E.StopMoving();
    }

    private IEnumerator Roam() {
        while (true) {
            // Determine where we should be moving to
            if (FarAwayFromOriginalPosition && roamStatus != RoamStatus.MovingTowardOriginalPosition) {
                roamStatus = RoamStatus.MovingTowardOriginalPosition;
            } else if (AtOriginalPosition && roamStatus != RoamStatus.MovingTowardDestination) {
                roamStatus = RoamStatus.MovingTowardDestination;
                destination = Util.GetRandomPointInRangeNotBlocked(E.transform.position, RoamDistance, Pathfinding.instance);
            }

            // Move
            switch(roamStatus) {
                case RoamStatus.MovingTowardOriginalPosition:
                    E.MoveTo(E.OriginalPosition);
                    break;

                case RoamStatus.MovingTowardDestination:
                    E.MoveTo(destination);
                    break;
            }

            yield return null;
        }
    }

    /* private static readonly int IterationLimit = 1000;
    private Vector3 FindRoamDestination() {
        Vector3 pos = E.transform.position;
        int count = 0;
        Vector3 randomDest;
        Vector2 randomCircle;
        Vector3 randomDir;
        do {
            randomCircle = Random.insideUnitCircle;
            randomDir = new Vector3(
                randomCircle.x,
                0f,
                randomCircle.y
            ).normalized;

            randomDest = pos + (randomDir * RoamDistance);
            count++;
        } while (Pathfinding.instance.CheckForBlockage(E, randomDest) && count < IterationLimit);

        // If we've hit the iteration limit, it means we couldn't find any random destination that wasn't blocked by something
        if (count >= IterationLimit) {
            Debug.Log("Reached iteration limit in finding new roam destination! Weird!");
            return pos;
        }

        return randomDest;
    } */
}