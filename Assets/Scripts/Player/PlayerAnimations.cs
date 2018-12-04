using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour {

    private PlayerMovement movementController;
    private Animator animator;
    private Rigidbody rb;
    private bool grounded;
    [SerializeField]
    private float defaultMoveSpeed, deadZone;

    /// <summary>
    /// The current move speed of the player in the XZ plane
    /// </summary>
    public float MoveSpeed
    {
        get { return animator.GetFloat("MoveVelocity"); }
        set { animator.SetFloat("MoveVelocity", value); }
    }

    /// <summary>
    /// The amount to multiply the move animation speed in the animator
    /// </summary>
    public float MoveAnimationMultiplier
    {
        get { return animator.GetFloat("MoveSpeedMultiplier"); }
        set { animator.SetFloat("MoveSpeedMultiplier", value); }
    }

    /// <summary>
    /// The current velocity of the player in the y direction
    /// </summary>
    public float JumpVelocity
    {
        get { return animator.GetFloat("JumpVelocity"); }
        set { animator.SetFloat("JumpVelocity", value); }
    }

    /// <summary>
    /// Whether or not the player is grounded
    /// </summary>
    public bool Grounded
    {
        get { return animator.GetBool("Grounded"); }
        set { animator.SetBool("Grounded", value); }
    }

    private void Start()
    {
        movementController = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Grounded = movementController.Grounded;

        MoveSpeed = movementController.Velocity2D.magnitude;

        MoveAnimationMultiplier = MoveSpeed / defaultMoveSpeed;

        JumpVelocity = rb.velocity.y;
    }
}
