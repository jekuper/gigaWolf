using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IAliveEntity {
    [Header ("Movement Configurations")]
    [SerializeField] float maxSpeed = 2f;
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float rotationSpeed = 180f;
    [SerializeField] Animator animator;
    [SerializeField] private Animator deathMenuAnimator;
    [SerializeField] private GameObject UIHolder;

    public bool IsGrounded { get; private set; }

    private float horizontalMovement;
    private float verticalMovement;
    private readonly float movementMultiplier = 10f;
    private Rigidbody rb;
    private GameObject bloodParticle;


    void Start () {
        rb = GetComponent<Rigidbody> ();
        bloodParticle = Resources.Load ("bloodParticles") as GameObject;
    }

    void Update () {
        MyInput ();
    }
    private void FixedUpdate () {
        MoveAndRotatePlayer ();
    }



    void MyInput () {
        horizontalMovement = Input.GetAxisRaw ("Horizontal");
        verticalMovement = Input.GetAxisRaw ("Vertical");
        if (verticalMovement < 0) {
            verticalMovement = 0;
        }

    }

    void MoveAndRotatePlayer () {
        rb.AddForce (movementMultiplier * moveSpeed * verticalMovement * transform.forward, ForceMode.Acceleration);

        if (rb.velocity.magnitude > maxSpeed) {
            rb.velocity = Vector3.ClampMagnitude (rb.velocity, maxSpeed);
        }
        animator.SetFloat ("speed", verticalMovement, 0.2f, Time.deltaTime);
        //Rotation
        transform.Rotate (new Vector3 (0f, horizontalMovement * Time.deltaTime * rotationSpeed, 0f));
    }


    public void Die () {
        deathMenuAnimator.SetTrigger ("show");
        UIHolder.SetActive (false);
        Instantiate (bloodParticle, transform.position, Quaternion.identity);
        Destroy (gameObject);
    }
}
