using System.Collections;
using UnityEngine;

public class ChasingState : EnemyState {
    public override EnemyStateID ID { get => EnemyStateID.Chasing; }

    private Unit Chasing { get; set; }

    private Pathfinding PF;
    private Vector3[] pathWaypoints;
    private int waypointIndex;
    private Coroutine updatePathRoutine;

    public ChasingState(Enemy _e, Unit _u) : base(_e) {
        Chasing = _u;

        PF = Pathfinding.instance;
        updatePathRoutine = CoroutineRunner.instance.Run(UpdatePathing());
    }

    public override void Update() {
        if (TargetIsWithinAttackRange) {
            Transition(EnemyStateID.Attacking, Chasing);
            return;
        }

        MoveAlongWaypoints();
    }

    private static readonly float WaypointBuffer = Constants.WAYPOINT_BUFFER;
    private void MoveAlongWaypoints() {
        if (pathWaypoints == null) {
            Debug.Log("Waypoints were null! This shouldn't happen!");

            return;
        }

        if (waypointIndex > pathWaypoints.Length) {
            return;
        }

        Vector3 waypoint = pathWaypoints[waypointIndex];
        float distanceToWaypoint = (waypoint - E.transform.position).magnitude;
        // * Assumes this is being called from FixedUpdate
        float distanceThisFrame = E.AdjustedMoveSpeed * Time.fixedDeltaTime;

        if ((distanceToWaypoint - WaypointBuffer) <= distanceThisFrame) {
            waypointIndex++;
            if (waypointIndex >= pathWaypoints.Length) {
                Debug.Log("Reached the end of the waypoint paths without reaching destination. Should not happen!");
                return;
            }

            waypoint = pathWaypoints[waypointIndex];
        }

        E.MoveTo(waypoint);
    }

    public override void Leave() {
        CoroutineRunner.instance.Stop(updatePathRoutine);
        E.StopMoving();
    }

    private static readonly float Wait = Constants.PATHFINDING_TICK;
    private IEnumerator UpdatePathing() {
        while (true) {
            // If there's something blocking our path then we will need to actually calculate a path
            if (PF.CheckForBlockage(E.transform.position, Chasing.transform.position)) {
                pathWaypoints = PF.FindPath(E, Chasing.transform.position, Chasing);
                waypointIndex = 0;
                yield return new WaitForSeconds(Wait);
            } else { // Otherwise we'll just go straight to the target
                pathWaypoints = new Vector3[] { Chasing.transform.position };
                waypointIndex = 0;
                yield return null;
            }
        }
    }

    private bool TargetIsWithinAttackRange {
        get {
            return (E.transform.position - Chasing.transform.position).magnitude < E.AttackRange;
        }
    }
}