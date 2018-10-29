using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls player movement. Player can move in the xz plane and jump. The player has inertia and reacts to outside forces. When the
/// player stands on a moving object, it will keeps its velocity relative to that object's velocity. The moving object MUST have a rigidbody
/// with its velocity set. The player can and should be set to use custom gravity forces which make the jump feel less floaty.
/// KNOWN BUG: If the player jumps exactly as the collider clips the edge of another collider the player will be launched into the air.
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
    // Should the controller apply its own custom gravity to the player instead of using the scene gravity
    [SerializeField] private bool useCustomGravity;
    // The up and down gravity multipliers if using custom gravity
    [SerializeField] private float upwardGravityMultiplier;
    [SerializeField] private float downwardGravityMultiplier;

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
    public bool UseCustomGravity { get { return useCustomGravity; } }

    // Methods
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        Vector3 jumpGravity = Physics.gravity;
        if (useCustomGravity)
        {
            rb.useGravity = false;
            jumpGravity *= upwardGravityMultiplier;
        }
        // Do some math to find our initial jump velocity based on the initial jump gravity
        jumpImpulse = (2 * jumpHeight) / (Mathf.Sqrt(2 * jumpHeight / jumpGravity.magnitude));
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
        if (useCustomGravity)
        {
            CustomGravity();
        }
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
        jump = Input.GetButtonDown("Jump");
    }

    private bool ProbeGround(out Rigidbody hitRigidbody)
    {
        RaycastHit hit;
        // THIS IS BROKEN
        // Sweep Test doesn't return hits with colliders the rigidbody is already colliding with
        // It only works right now because the capsule collider hovers above the ground and THAT collider is swept into the ground properly
        bool didHit = rb.SweepTest(Vector3.down, out hit, probeDist, QueryTriggerInteraction.Ignore);
        hitRigidbody = hit.rigidbody;
        return didHit;
    }

    private void DirectionalMovement(Rigidbody groundRigidbody)
    {
        Vector2 groundVelocity = groundRigidbody == null ? Vector2.zero : new Vector2(groundRigidbody.velocity.x, groundRigidbody.velocity.z);

        Vector2 currentRelativeVelocity = new Vector2(rb.velocity.x, rb.velocity.z) - groundVelocity;
        Vector2 targetRelativeVelocity = directionInput * maxSpeed;

        Vector2 difference = targetRelativeVelocity - currentRelativeVelocity;

        // If the current velocity is pointing over 90 degrees from the target velocity then treat the left over vector as being a stop force
        if (targetRelativeVelocity.sqrMagnitude > stopForceSlop && Vector2.Dot(currentRelativeVelocity, targetRelativeVelocity) < 0)
        {
            Vector2 frictionDif = Vector2.Dot(currentRelativeVelocity, targetRelativeVelocity) * targetRelativeVelocity.normalized * -1;
            // Add the stop force
            rb.AddForce(new Vector3(frictionDif.x, 0, frictionDif.y) * stopForceMultiplier, ForceMode.Acceleration);
            // Remove that part of the force from the difference
            difference -= frictionDif;
        }

        rb.AddForce(new Vector3(difference.x, 0, difference.y) * (targetRelativeVelocity.sqrMagnitude > stopForceSlop ? walkForceMultiplier : stopForceMultiplier), ForceMode.Acceleration);
    }

    private void CustomGravity()
    {
        // If you are moving up apply the upward gravity otherwise apply the downward gravity
        rb.AddForce(Physics.gravity * (rb.velocity.y > 0 ? upwardGravityMultiplier : downwardGravityMultiplier), ForceMode.Acceleration);
    }

    private void Jump()
    {
        if (jump)
        {
            rb.velocity.Set(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpImpulse, ForceMode.VelocityChange);
        }
    }
}
