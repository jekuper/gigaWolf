using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum ObstaclePosition {
    Left,
    Right,
    None
}
public class SheepAI : MonoBehaviour
{
    [Serializable]
    private class SheepConfig {
        public float moveSpeed;
        public float angleDelta;
        public float time = -1;

        public void Generate () {
            moveSpeed = UnityEngine.Random.Range (3f, 5f);
            time = UnityEngine.Random.Range (1, 3);
            angleDelta = UnityEngine.Random.Range (-90, 90);
        }
    }


    public float maxSpeed = 2f;
    public float rotationTime;
    public LayerMask threadsMask;
    [SerializeField] private SheepConfig sheepConfig;
    public Transform castPoint1, castPoint2;

    private Rigidbody rb;
    private MeshRenderer mr;
    private readonly float movementMultiplier = 10f;

    private Transform threatRef = null;

    private bool isScared = false;
    private bool isStunned = false;

    private float obstacleOnLeftDist = 10000, obstacleOnRightDist = 10000;
    private ObstaclePosition firstObstacle = ObstaclePosition.Left;


    public void RoarEffect (float time) {
        StartCoroutine (Stun (time));
    }
    private IEnumerator Stun(float time) {
        isStunned = true;
        while(time > 0) {
            time -= Time.deltaTime;
            yield return new WaitForEndOfFrame ();
        }
        isStunned = false;
    }

    private void Start () {
        sheepConfig = new ();
        sheepConfig.Generate ();
        GetReferences ();
    }
    private void GetReferences () {
        rb = GetComponent<Rigidbody> ();
        mr = GetComponent<MeshRenderer> ();
    }

    private void FixedUpdate () {
        HandleObstacles ();
        HandleState ();
        if (!isStunned)
            HandleBehaviour ();
    }

    private void HandleState () {
        if (isScared && Vector3.Distance(transform.position, threatRef.position) > 4) {
            isScared = false;
            threatRef = null;
            mr.material.color = Color.white;
            sheepConfig.Generate ();
        }
    }
    private void HandleBehaviour () {
        if (!isScared) {
            if (sheepConfig.time < 0) {
                sheepConfig.Generate ();
                StartCoroutine (Rotate (sheepConfig.angleDelta + transform.eulerAngles.y));
            }
            sheepConfig.time -= Time.fixedDeltaTime;
        } else {
            float targetAngle = GetAngleToTarget (threatRef);

            if (firstObstacle == ObstaclePosition.None)
                transform.rotation = Quaternion.Euler(0f, Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, .1f), 0f);
        }
        if ((obstacleOnLeftDist < 0.3f) || (obstacleOnRightDist < 0.3f)) {
            rb.AddForce(movementMultiplier * sheepConfig.moveSpeed * transform.forward * -1f, ForceMode.Acceleration);
        } else {
            rb.AddForce (movementMultiplier * sheepConfig.moveSpeed * transform.forward, ForceMode.Acceleration);
        }
        
        if (firstObstacle == ObstaclePosition.Left) {
            transform.Rotate (new Vector3(0f, Time.fixedDeltaTime * 160f, 0f));
        }
        else if (firstObstacle == ObstaclePosition.Right) {
            transform.Rotate (new Vector3(0f, Time.fixedDeltaTime * -160f, 0f));
        }

        if (rb.velocity.magnitude > maxSpeed) {
            rb.velocity = Vector3.ClampMagnitude (rb.velocity, maxSpeed);
        }
    }
    private void HandleObstacles () {
        Debug.DrawLine (castPoint1.position, castPoint1.position + castPoint1.forward * 5, Color.blue, 0.1f);
        Debug.DrawLine (castPoint2.position, castPoint2.position + castPoint2.forward * 5, Color.blue, 0.1f);

        RaycastHit hit1, hit2;

        bool newLeft = Physics.Raycast (castPoint1.position, castPoint1.forward, out hit1, 5f);
        bool newRight = Physics.Raycast (castPoint2.position, castPoint2.forward, out hit2, 5f);
        
        if (firstObstacle == ObstaclePosition.Left && newLeft == false) {
            firstObstacle = ObstaclePosition.None;
        }
        if (firstObstacle == ObstaclePosition.Right && newRight == false) {
            firstObstacle = ObstaclePosition.None;
        }
        if (firstObstacle == ObstaclePosition.None) {
            if (newLeft == true) {
                firstObstacle = ObstaclePosition.Left;
            }
            if (newRight == true) {
                firstObstacle = ObstaclePosition.Right;
            }
        }

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


    private void OnTriggerEnter (Collider other) {
        if (((1 << other.gameObject.layer) & threadsMask.value) != 0) {
            isScared = true;
            threatRef = other.transform;
            mr.material.color = Color.red;
        }
    }

    private IEnumerator Rotate (float targetAngle) {
        float timer = rotationTime;
        float delta = targetAngle - transform.eulerAngles.y;
        while(timer > Time.fixedDeltaTime) {
            if (firstObstacle != ObstaclePosition.None || isStunned || isScared) {
                yield break;
            }
            timer -= Time.deltaTime;
            transform.Rotate (0f, Time.fixedDeltaTime * delta, 0f);
            yield return new WaitForFixedUpdate ();
        }
        transform.rotation = Quaternion.Euler (transform.eulerAngles.x, targetAngle, transform.eulerAngles.z);
    }


    private float GetAngleToTarget (Transform target) {
        Vector3 oppositeDirection = (transform.position - target.position);
        oppositeDirection.Normalize ();
        oppositeDirection.y = 0;
        float targetAngle = Vector3.Angle (new Vector3 (0f, 0f, 1f), oppositeDirection);
        if (oppositeDirection.x < 0.0f)
            targetAngle = 360.0f - targetAngle;
        return targetAngle;
    }
    private float getXZDist (Vector3 a, Vector3 b) {
        return Vector2.Distance (new Vector2 (a.x, a.z), new Vector2 (b.x, b.z));
    }
}
