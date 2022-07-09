using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header ("Movement Configurations")]
    [SerializeField] float maxSpeed = 2f;
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float torque = 4f;


    [Header ("Ground Detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundDistance = 0.2f;

    public bool IsGrounded { get; private set; }

    private float horizontalMovement;
    private float verticalMovement;
    private readonly float movementMultiplier = 10f;
    private Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody> ();
//        rb.freezeRotation = true;
    }

    void Update()
    {
        IsGrounded = Physics.CheckSphere (groundCheck.position, groundDistance, groundMask);

        MyInput ();
    }
    private void FixedUpdate () {
        MoveAndRotatePlayer ();
    }



    void MyInput () {
        horizontalMovement = Input.GetAxisRaw ("Horizontal");
        verticalMovement = Input.GetAxisRaw ("Vertical");

    }

    void MoveAndRotatePlayer () {
        rb.AddForce (movementMultiplier * moveSpeed * verticalMovement * transform.forward, ForceMode.Acceleration);

        if (rb.velocity.magnitude > maxSpeed) {
            rb.velocity = Vector3.ClampMagnitude (rb.velocity, maxSpeed);
        }

        //Rotation
        rb.AddTorque (0, horizontalMovement * torque, 0, ForceMode.VelocityChange); 
    }
}
