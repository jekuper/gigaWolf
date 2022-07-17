using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    [SerializeField] private Transform target;
    [SerializeField] private float distanceZ = 20;
    [SerializeField] private float distanceY = 10f;
    [SerializeField] private float rotationAngle = 45;
    [SerializeField] private float smoothSpeed;

    private Vector3 refVelocity;


    void Update () {
        HandleCamera ();
    }

    //follow the target script
    private void HandleCamera () {
        if (!target) {
            return;
        }

        Vector3 worldPos = (Vector3.forward * -distanceZ) + (Vector3.up * distanceY);

        Vector3 rotationVector = Quaternion.AngleAxis (rotationAngle, Vector3.up) * worldPos;

        Vector3 flatTargetPosition = target.position;
        flatTargetPosition.y = 0f;
        Vector3 finalPosition = flatTargetPosition + rotationVector;

        transform.position = Vector3.SmoothDamp (transform.position, finalPosition, ref refVelocity, smoothSpeed);
        transform.LookAt (flatTargetPosition);

    }

}
