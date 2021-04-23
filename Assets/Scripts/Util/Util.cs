using System.Collections;
using UnityEngine;

public static class Util {
    public static bool IsCollisionWithGround(Collision _collision) {
        return _collision.gameObject.tag == Constants.GROUND_TAG;
    }

    public static Vector3 GetRandomPointInRange(Vector3 _origin, float _range) {
        Vector2 randomCircle = Random.insideUnitCircle;

        Vector3 randomDir = new Vector3(
            randomCircle.x,
            0f,
            randomCircle.y
        ).normalized;

        Vector3 randomDest = _origin + (randomDir * _range);
        return randomDest;
    }

    private static readonly int IterationLimit = 1000;
    public static Vector3 GetRandomPointInRangeNotBlocked(Vector3 _origin, float _range, Pathfinding _pathfindingInstance) {
        int count = 0;
        Vector3 randomPoint;
        do {
            randomPoint = GetRandomPointInRange(_origin, _range);
            count++;
        } while (_pathfindingInstance.CheckForBlockage(_origin, randomPoint) && count < IterationLimit);

        if (count == IterationLimit) {
            Debug.Log("Could not find a random point in range that was not blocked!");
            return _origin;
        }

        return randomPoint;
    }
}