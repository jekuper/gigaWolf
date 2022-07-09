using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ObstaclePosition {
    Left,
    Right,
    None
}

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody))]
public class AnimalAI : MonoBehaviour {
    [Serializable]
    private class AnimalConfig {
        public float moveSpeed;
        public float angleDelta;
        public float time = -1;

        public void Generate (float mnTime) {
            moveSpeed = UnityEngine.Random.Range (3f, 5f);
            time = UnityEngine.Random.Range (mnTime, 2 * mnTime);
            angleDelta = UnityEngine.Random.Range (-90, 90);
        }
    }


    public float maxSpeed = 2f;
    public float sensorsDistance = 5f;
    public float calmDistance = 4f;
    public float rotationTime;
    public LayerMask threadsMask;
    public LayerMask attackMask;
    public LayerMask obstacleLayerMask;
    [SerializeField] private AnimalConfig animalConfig;
    public Transform castPointLeft, castPointRight;

    private Rigidbody rb;
    private MeshRenderer mr;
    private Color initMaterialColor;
    private readonly float movementMultiplier = 10f;


    private bool isScared = false;
    private bool isStunned = false;

    private float obstacleOnLeftDist = 10000, obstacleOnRightDist = 10000;
    private ObstaclePosition firstObstacle = ObstaclePosition.Left;

    private List<Transform> threatRefs = new List<Transform>();
    private Transform attackTarget = null;

    public void RoarEffect (float time) {
        StartCoroutine (Stun (time));
    }
    private IEnumerator Stun (float time) {
        isStunned = true;
        while (time > 0) {
            time -= Time.deltaTime;
            yield return new WaitForEndOfFrame ();
        }
        isStunned = false;
    }

    private void Start () {
        animalConfig = new ();
        GetReferences ();
    }
    private void GetReferences () {
        rb = GetComponent<Rigidbody> ();
        mr = GetComponent<MeshRenderer> ();
        initMaterialColor = mr.material.color;
    }

    private void FixedUpdate () {
        HandleObstacles ();
        HandleScaredState ();
        AttackTargetChecker ();
        if (!isStunned)
            HandleMovementBehaviour ();
    }

    private void HandleScaredState () {
        if (isScared) {
            for(int i = 0; i < threatRefs.Count; i++) {
                if (Vector3.Distance (transform.position, threatRefs[i].position) > calmDistance) {
                    threatRefs.RemoveAt (i);
                    i--;
                }
            }

            if (threatRefs.Count == 0) {
                isScared = false;
                mr.material.color = initMaterialColor;
                animalConfig.Generate (rotationTime);
            }
        }
    }
    private void AttackTargetChecker () {
        if (attackTarget != null && Vector3.Distance (transform.position, attackTarget.position) > calmDistance) {
            attackTarget = null;
        }
    }
    private void HandleMovementBehaviour () {
        if (!isScared) {
            if (attackTarget != null) {
                float targetAngle = DirectionVectorToAngle(GetDir(attackTarget));
                if (firstObstacle == ObstaclePosition.None) {
                    transform.rotation = Quaternion.Euler (0f, Mathf.LerpAngle (transform.eulerAngles.y, targetAngle, .1f), 0f);
                }
            } else {
                if (animalConfig.time < 0) {
                    animalConfig.Generate (rotationTime);
                    StartCoroutine (Rotate (animalConfig.angleDelta + transform.eulerAngles.y));
                }
                animalConfig.time -= Time.fixedDeltaTime;
            }
        } else {
            float targetAngle = GetAverageAngle (threatRefs);
            if (firstObstacle == ObstaclePosition.None)
                transform.rotation = Quaternion.Euler (0f, Mathf.LerpAngle (transform.eulerAngles.y, targetAngle, .1f), 0f);
        }


        if ((obstacleOnLeftDist < 0.3f) || (obstacleOnRightDist < 0.3f)) {
            rb.AddForce (movementMultiplier * animalConfig.moveSpeed * transform.forward * -1f, ForceMode.Acceleration);
        } else {
            rb.AddForce (movementMultiplier * animalConfig.moveSpeed * transform.forward, ForceMode.Acceleration);
        }

        if (firstObstacle == ObstaclePosition.Left) {
            transform.Rotate (new Vector3 (0f, Time.fixedDeltaTime * 160f, 0f));
        } else if (firstObstacle == ObstaclePosition.Right) {
            transform.Rotate (new Vector3 (0f, Time.fixedDeltaTime * -160f, 0f));
        }

        if (rb.velocity.magnitude > maxSpeed) {
            rb.velocity = Vector3.ClampMagnitude (rb.velocity, maxSpeed);
        }
    }
    private void HandleObstacles () {
        Debug.DrawLine (castPointLeft.position, castPointLeft.position + castPointLeft.forward * sensorsDistance, Color.blue, 0.1f);
        Debug.DrawLine (castPointRight.position, castPointRight.position + castPointRight.forward * sensorsDistance, Color.blue, 0.1f);

        RaycastHit hit1, hit2;

        bool newLeft = Physics.Raycast (castPointLeft.position, castPointLeft.forward, out hit1, sensorsDistance, layerMask: obstacleLayerMask);
        bool newRight = Physics.Raycast (castPointRight.position, castPointRight.forward, out hit2, sensorsDistance, layerMask: obstacleLayerMask);

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
            threatRefs.Add(other.transform);
            mr.material.color = Color.red;
        }
        if (((1 << other.gameObject.layer) & attackMask.value) != 0) {
            attackTarget = other.transform;
            Debug.Log ("ENTERED--"+attackTarget.name);
        }
    }




    private IEnumerator Rotate (float targetAngle) {
        float timer = rotationTime;
        float delta = targetAngle - transform.eulerAngles.y;
        while (timer > Time.fixedDeltaTime) {
            if (firstObstacle != ObstaclePosition.None || isStunned || isScared) {
                yield break;
            }
            timer -= Time.fixedDeltaTime;
            transform.Rotate (0f, Time.fixedDeltaTime * delta / rotationTime, 0f);
            yield return new WaitForFixedUpdate ();
        }
        transform.rotation = Quaternion.Euler (transform.eulerAngles.x, targetAngle, transform.eulerAngles.z);
    }
    private float GetAverageAngle (List<Transform> threads) {
        if (threads.Count == 0) {
            Debug.LogError ("not enough threads");
            return 0;
        }
        Vector3 avr = GetOppossiteDir (threads[0]);
        for (int i = 1; i < threads.Count; i++) {
            avr += GetOppossiteDir (threads[i]);
        }
        avr /= threads.Count;
        return DirectionVectorToAngle (avr);
    }
    private Vector3 GetOppossiteDir (Transform target) {
        Vector3 oppositeDirection = (transform.position - target.position);
        oppositeDirection.Normalize ();
        oppositeDirection.y = 0;
        return oppositeDirection;
    }
    private Vector3 GetDir (Transform target) {
        Vector3 dir = (target.position - transform.position);
        dir.Normalize ();
        dir.y = 0;
        return dir;
    }
    private float DirectionVectorToAngle (Vector3 dir) {
        float targetAngle = Vector3.Angle (new Vector3 (0f, 0f, 1f), dir);
        if (dir.x < 0.0f)
            targetAngle = 360.0f - targetAngle;
        return targetAngle;
    }
    private float getXZDist (Vector3 a, Vector3 b) {
        return Vector2.Distance (new Vector2 (a.x, a.z), new Vector2 (b.x, b.z));
    }
}
