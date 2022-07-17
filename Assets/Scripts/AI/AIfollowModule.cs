using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Collider))]
public class AIfollowModule : MonoBehaviour, IaiModule {
    [SerializeField] private AnimalAISystem sys;

    public float calmDistance = 12;
    public Transform attackTarget = null;


    public void MainHandler () {
        if (attackTarget != null) {
            //deleting far targets
            if (Globals.getXZDist (sys.mainTransform.position, attackTarget.position) > calmDistance) {
                attackTarget = null;
                return;
            }
            //calculating and sending rotation towards attack target
            float targetAngle = Globals.DirectionVectorToAngle (Globals.GetDir (sys.mainTransform, attackTarget));
            Quaternion targetRotation = Quaternion.Euler (0f, Mathf.MoveTowardsAngle (sys.mainTransform.eulerAngles.y, targetAngle, Time.fixedDeltaTime * sys.rotationSpeed), 0f);
            sys.SendRotation (targetRotation, rotationSource.Follow);
        }

    }

    private void OnTriggerEnter (Collider other) {
        //adding atttack target
        if (((1 << other.gameObject.layer) & sys.attackMask.value) != 0) {
            attackTarget = other.transform;
        }
    }
}
