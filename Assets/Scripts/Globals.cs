using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals {

    public static Vector3 GetOppossiteDir (Transform from, Transform to) {
        Vector3 oppositeDirection = (from.position - to.position);
        oppositeDirection.Normalize ();
        oppositeDirection.y = 0;
        return oppositeDirection;
    }
    public static Vector3 GetDir (Transform from, Transform to) {
        Vector3 dir = (to.position - from.position);
        dir.Normalize ();
        dir.y = 0;
        return dir;
    }
    public static float DirectionVectorToAngle (Vector3 dir) {
        float targetAngle = Vector3.Angle (new Vector3 (0f, 0f, 1f), dir);
        if (dir.x < 0.0f)
            targetAngle = 360.0f - targetAngle;
        return targetAngle;
    }
    public static float getXZDist (Vector3 a, Vector3 b) {
        return Vector2.Distance (new Vector2 (a.x, a.z), new Vector2 (b.x, b.z));
    }
}
