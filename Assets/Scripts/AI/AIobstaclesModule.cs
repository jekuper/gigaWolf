using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ObstaclePosition {
    Left,
    Right,
    None
}

public class AIobstaclesModule : MonoBehaviour, IaiModule {
    [SerializeField] private AnimalAISystem sys;

    public float sensorsDistance = 5f;
    [SerializeField] private Transform castPointLeft;
    [SerializeField] private Transform castPointRight;

    private float obstacleOnLeftDist = 10000;
    private float obstacleOnRightDist = 10000;

    private ObstaclePosition firstObstacle;
    public float addTimer = -1;


    public void MainHandler () {
        HandleObstacles ();

        //checking if current object is going to touch obstacle
        if ((obstacleOnLeftDist < 0.3f) || (obstacleOnRightDist < 0.3f)) {
            sys.SendForce (sys.movementMultiplier * sys.maxSpeed * sys.mainTransform.forward * -1f, ForceMode.Acceleration, forceSource.Obstacles);
        } else {
            sys.SendForce (sys.movementMultiplier * sys.maxSpeed * sys.mainTransform.forward, ForceMode.Acceleration, forceSource.Obstacles);
        }

        if (addTimer >= 0) {
            addTimer -= Time.fixedDeltaTime;
            if (addTimer < 0)
                firstObstacle = ObstaclePosition.None;
        }

        //avoiding obstacles
        if (firstObstacle == ObstaclePosition.Left) {
            Vector3 newEuler = new Vector3 (0f, sys.mainTransform.eulerAngles.y + (Time.fixedDeltaTime * sys.rotationSpeed), 0f);
            sys.SendRotation (Quaternion.Euler (newEuler), rotationSource.Obstacles);
        } else if (firstObstacle == ObstaclePosition.Right) {
            Vector3 newEuler = new Vector3 (0f, sys.mainTransform.eulerAngles.y + (Time.fixedDeltaTime * -sys.rotationSpeed), 0f);
            sys.SendRotation (Quaternion.Euler (newEuler), rotationSource.Obstacles);
        }
    }

    //updating obstacles information
    private void HandleObstacles () {
        Debug.DrawLine (castPointLeft.position, castPointLeft.position + castPointLeft.forward * sensorsDistance, Color.blue, 0.1f);
        Debug.DrawLine (castPointRight.position, castPointRight.position + castPointRight.forward * sensorsDistance, Color.blue, 0.1f);

        RaycastHit hit1, hit2;

        bool newLeft = Physics.Raycast (castPointLeft.position, castPointLeft.forward, out hit1, sensorsDistance, layerMask: sys.obstacleLayerMask);
        bool newRight = Physics.Raycast (castPointRight.position, castPointRight.forward, out hit2, sensorsDistance, layerMask: sys.obstacleLayerMask);

        //setting additional timer when obstacle dissapear in order to make movement smoother
        if (firstObstacle == ObstaclePosition.Left && newLeft == false && addTimer < 0) {
            addTimer = .1f;
        }
        if (firstObstacle == ObstaclePosition.Right && newRight == false && addTimer < 0) {
            addTimer = .1f;
        }

        if (firstObstacle == ObstaclePosition.None) {
            if (newLeft == true) {
                firstObstacle = ObstaclePosition.Left;
                sys.movementModule.config.Generate (2, sys.mainTransform.eulerAngles.y);
            }
            if (newRight == true) {
                firstObstacle = ObstaclePosition.Right;
                sys.movementModule.config.Generate (2, sys.mainTransform.eulerAngles.y);
            }
        }

        //updating distances
        if (newLeft) {
            obstacleOnLeftDist = hit1.distance;
        } else {
            obstacleOnLeftDist = 10000;
        }
        if (newRight) {
            obstacleOnRightDist = hit2.distance;
        } else {
            obstacleOnRightDist = 10000;
        }
    }
}
