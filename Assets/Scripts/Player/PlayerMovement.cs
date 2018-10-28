using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls player movement.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    // Fields
    // How much the force towards the desired velocity should be scaled
    // Smaller numbers mean floatier control while larger numbers mean snappier response
    [SerializeField] private float walkForceMultiplier;
    // How much the force is scaled when the desired velocity is zero
    [SerializeField] private float stopForceMultiplier;
    // The max speed the player can move at
    [SerializeField] private float maxSpeed;
    // The height the player can jump
    [SerializeField] private float jumpHeight;

    // Component references
    private Rigidbody rb;

    private float jumpImpulse;

    // Bools for taking in input to use when calculating player movement
    private Vector2 directionInput;
    private bool jump;

    // How close the desired velocity needs to be to zero for the stop force to kick in
    private const float stopForceSlop = 0.01f;
    // How close the current velocity needs to be to zero for the velocity to be zeroed out
    private const float stopSlop = 0.1f;

    // The distance the probe should cast the rb colliders to test for ground collision
    private const float probeDist = 0.05f;

    // Properties
    public float WalkForceMultiplier { get { return walkForceMultiplier; } }
    public float StopForceMultipliertop { get { return stopForceMultiplier; } }
    public float MaxSpeed { get { return maxSpeed; } }
    public float JumpHeight { get { return jumpHeight; } }

    // Methods
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // Do some math to find our initial jump velocity based on the scene gravity
        jumpImpulse = (2 * jumpHeight) / (Mathf.Sqrt(-2 * jumpHeight / (Physics.gravity.y)));
    }

    private void Update()
    {
        GetInputs();
    }

    private void FixedUpdate()
    {
        Rigidbody groundRigidbody = null;
        bool grounded = ProbeGround(out groundRigidbody);

        DirectionalMovement(groundRigidbody);
        if (grounded)
        {
            Jump();
        }

        if (rb.velocity.x < stopSlop)
        {
            rb.velocity.Set(0, rb.velocity.y, rb.velocity.z);
        }
        if (rb.velocity.z < stopSlop)
        {
            rb.velocity.Set(rb.velocity.x, rb.velocity.y, 0);
        }
    }

    private void GetInputs()
    {
        directionInput.x = Input.GetAxis("Horizontal");
        directionInput.y = Input.GetAxis("Vertical");
        directionInput = Vector2.ClampMagnitude(directionInput, 1.0f);
        jump = Input.GetAxis("Jump") > 0;
    }

    private void DirectionalMovement(Rigidbody groundRigidbody)
    {
        Vector2 groundVelocity = groundRigidbody == null ? Vector2.zero : new Vector2(groundRigidbody.velocity.x, groundRigidbody.velocity.z);

        Vector2 currentRelativeVelocity = new Vector2(rb.velocity.x, rb.velocity.z) - groundVelocity;
        Vector2 targetRelativeVelocity = directionInput * maxSpeed;

        Vector2 difference = targetRelativeVelocity - currentRelativeVelocity;

        rb.AddForce(new Vector3(difference.x, 0, difference.y) * (targetRelativeVelocity.sqrMagnitude > stopForceSlop ? walkForceMultiplier : stopForceMultiplier));
    }

    private void Jump()
    {
        if (jump)
        {
            rb.velocity.Set(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpImpulse, ForceMode.VelocityChange);
        }
    }

    private bool ProbeGround(out Rigidbody hitRigidbody)
    {
        RaycastHit hit;
        bool didHit = rb.SweepTest(Vector3.down, out hit, probeDist, QueryTriggerInteraction.Ignore);
        hitRigidbody = hit.rigidbody;
        return didHit;
    }
}
