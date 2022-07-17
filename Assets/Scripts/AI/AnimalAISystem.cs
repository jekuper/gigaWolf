using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IaiModule {
    void MainHandler ();
}
public interface IAliveEntity {
    void Die ();
}


public class AnimalAISystem : MonoBehaviour, IAliveEntity {
    public AIattackModule attackModule;
    public AIfearModule fearModule;
    public AImovementModule movementModule;
    public AIfollowModule followModule;
    public AIobstaclesModule obstacleModule;

    public Transform mainTransform;
    public Animator mainAnimator;
    public Rigidbody mainRb;

    public LayerMask attackMask;
    public LayerMask threatsMask;
    public LayerMask obstacleLayerMask;

    public float maxSpeed = 2f;
    public float movementMultiplier = 10f;
    public float rotationSpeed = 180f;

    public bool isRoarEffected = false;

    private float maxSpeedDublicate;

    private void Start () {
        maxSpeedDublicate = maxSpeed;
    }

    public void TurnOff () {
        if (fearModule != null)
            fearModule.enabled = false;

        if (attackModule != null)
            attackModule.enabled = false;

        if (followModule != null)
            followModule.enabled = false;

        if (movementModule != null)
            movementModule.enabled = false;

        if (obstacleModule != null)
            obstacleModule.enabled = false;
        enabled = false;
    }
    public void Die () {
        Instantiate (Resources.Load ("bloodParticles") as GameObject, transform.position, Quaternion.identity);
        Destroy (mainTransform.gameObject);
    }

    public void SendRotation (Quaternion rotation, rotationSource level) {
        movementModule.rotCommands.Add (new RotationCommand (rotation, level));
    }
    public void SendForce (Vector3 dir, ForceMode mode, forceSource level) {
        movementModule.forceCommands.Add (new ForceCommand (dir, mode, level));
    }

    private void FixedUpdate () {
        if (fearModule != null)
            fearModule.MainHandler ();

        if (attackModule != null)
            attackModule.MainHandler ();

        if (followModule != null)
            followModule.MainHandler ();

        if (movementModule != null)
            movementModule.MainHandler ();

        if (obstacleModule != null)
            obstacleModule.MainHandler ();
    }

    public void RoarEffect (float time) {
        if (!isRoarEffected) {
            return;
        }
        StartCoroutine (Stun (time));
    }
    public void ImpulseEffect (float time, Quaternion rotation) {
        StartCoroutine (Impulse (time, rotation));
    }
    private IEnumerator Impulse (float time, Quaternion rotation) {
        movementModule.Move (time, rotation);

        maxSpeed = 4;
        mainAnimator.SetTrigger ("rollEnter");
        yield return new WaitForSeconds (time);
        mainAnimator.SetTrigger ("rollExit");
        maxSpeed = maxSpeedDublicate;
    }

    private IEnumerator Stun (float time) {
        movementModule.isStunned = true;
        yield return new WaitForSeconds (time);
        movementModule.isStunned = false;
    }
}
