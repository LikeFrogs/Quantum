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
    /// Whether or not the player is grounded calls the corresponding triggers
    /// in the animator for when the player jumps or lands
    /// </summary>
    public bool Grounded
    {
        set
        {
            if(value != grounded)
            {
                if (value)
                {
                    animator.SetTrigger("Land");
                }
                else
                {
                    animator.SetTrigger("Jump");
                }

                grounded = value;
            }
        }
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

        // THIS SHOULD NOT BE HERE
        // Rotation should be handled by the player movement script
        // I have moved my rotation code there
        // - Matthew
        //if(MoveSpeed > deadZone)
        //{
        //    transform.rotation = Quaternion.Euler(0, Mathf.Atan2(movementController.Velocity2D.y * -1, movementController.Velocity2D.x) * Mathf.Rad2Deg - 90, 0);
        //}
    }
}
