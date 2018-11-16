using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    #region Fields

    [SerializeField] private float maxWalkSpeed;
    // These modify how much force is applied in the direction of, orthogonal to, and in the opposite direction of the current velocity
    [SerializeField] private float accelerationMod;
    [SerializeField] private float turningMod;
    [SerializeField] private float brakingMod;
    [Space]
    [SerializeField] private float jumpHeight;
    [SerializeField] private float ascendingGravMod;
    [SerializeField] private float descendingGravMod;
    [Space]
    // The half extent size in the xz plane of the box that the player uses as its feet
    [SerializeField] private Vector2 feetHalfExtents;
    // The distance below the center of the player that the probe should sweep
    [SerializeField] private float probeDist;
    // The distance above the center of the player that the probe should begin sweeping from
    [SerializeField] private float probeVerticalOffset;
    [SerializeField] private LayerMask layerToIgnore;
    [Space]
    // Whether or not the movement should start enabled
    [SerializeField] private bool startEnabled;

    private Rigidbody rb;

    private Vector2 xzInput;
    private bool jumpInput;

    private float jumpImpulse;

    private RaycastHit groundHit;
    private const float feetHeight = 0.01f;

    private const float rotationStrength = 0.2f;

    #endregion

    #region Properties

    /// <summary>
    /// Whether or not the player should be allowed to jump. Allowed by default.
    /// </summary>
    public bool CanJump { get; set; }
    /// <summary>
    /// Whether or not the player should apply gravity.
    /// </summary>
    public bool UseGravity { get; set; }
    /// <summary>
    /// Whether or not movement should be enabled. Starts disabled!
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// The ground the player currently thinks it is standing on
    /// </summary>
    public GameObject Ground { get; private set; }
    /// <summary>
    /// Whether or not the player considers themselves to be on the ground
    /// </summary>
    public bool Grounded { get; private set; }

    /// <summary>
    /// The position of the player in the pure xz plane
    /// </summary>
    public Vector2 Position2D { get { return new Vector2(transform.position.x, transform.position.z); } }
    /// <summary>
    /// The velocity of the player in the pure xz plane
    /// </summary>
    public Vector2 Velocity2D { get { return new Vector2(rb.velocity.x, rb.velocity.z); } }

    /// <summary>
    /// The max speed the player can try to match
    /// </summary>
    public float MaxWalkSpeed { get { return maxWalkSpeed; } set { maxWalkSpeed = value >= 0 ? value : 0; } }
    /// <summary>
    /// The jump height of the player
    /// </summary>
    public float JumpHeight { get { return jumpHeight; } set { SetJumpImpulse(value); } }

    #endregion

    #region Methods

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        CanJump = true;
        UseGravity = true;
        Enabled = startEnabled;

        SetJumpImpulse(jumpHeight);
    }

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        ProbeFeet();

        if (Enabled)
        {
            XZMovement();

            if (CanJump)
            {
                Jump();
            }
        }

        if (UseGravity)
        {
            ApplyGravity();
        }

        RotateTowardsInput();

        ResetVerticalVelocityOnGrounded();

        ResetInput();
    }

    private void SetJumpImpulse(float jumpHeight)
    {
        this.jumpHeight = jumpHeight;
        float appliedGravity = Physics.gravity.magnitude * ascendingGravMod;
        jumpImpulse = jumpHeight > 0 ? (2 * jumpHeight) / Mathf.Sqrt(2 * jumpHeight / appliedGravity) : 0;
    }

    private void GetInput()
    {
        xzInput.x = Input.GetAxisRaw("Horizontal");
        xzInput.y = Input.GetAxisRaw("Vertical");
        xzInput = Vector2.ClampMagnitude(xzInput, 1);

        if (Input.GetButtonDown("Jump"))
        {
            jumpInput = true;
        }
    }

    private void ResetInput()
    {
        jumpInput = false;
    }

    /// <summary>
    /// Casts a box down from the feet to look for what ground if any the player is standing on
    /// </summary>
    private void ProbeFeet()
    {
        Vector3 center = transform.position + Vector3.up * probeVerticalOffset;
        Vector3 halfExtents = new Vector3(feetHalfExtents.x, feetHeight, feetHalfExtents.y);

        Grounded = Physics.BoxCast(center, halfExtents, Vector3.down, out groundHit, Quaternion.identity, probeDist + probeVerticalOffset, ~layerToIgnore, QueryTriggerInteraction.Ignore);

        Ground = Grounded ? groundHit.collider.gameObject : null;
    }

    /// <summary>
    /// Calculates and adds force for accurate movement in the XZ plane
    /// </summary>
    private void XZMovement()
    {
        Vector2 relativeVelocty = Velocity2D;

        if (Ground != null)
        {
            Rigidbody groundRigidbody = Ground.GetComponent<Rigidbody>();
            if (groundRigidbody != null)
            {
                relativeVelocty -= new Vector2(groundRigidbody.velocity.x, groundRigidbody.velocity.z);
            }
        }

        Vector2 desiredRelativeVelocity = xzInput * maxWalkSpeed;

        // Force needed is the force from current velocity to desired velocity
        Vector2 force = desiredRelativeVelocity - relativeVelocty;

        if (relativeVelocty != Vector2.zero)
        {
            // Decompose the force into its components relative to the current velocity
            Vector2 forceParallelToVel = Vector2.Dot(force, relativeVelocty.normalized) * relativeVelocty.normalized;
            Vector2 forceOrthogonalToVel = force - forceParallelToVel;
            // Put the components back together but scale them by the appropriate scaling modifier
            force = forceOrthogonalToVel * turningMod + forceParallelToVel * (Vector2.Dot(forceParallelToVel, relativeVelocty) >= 0 ? accelerationMod : brakingMod);
        }
        else
        {
            force *= accelerationMod;
        }

        rb.AddForce(new Vector3(force.x, 0, force.y));
    }

    /// <summary>
    /// Applies a jump impulse if applicable
    /// </summary>
    private void Jump()
    {
        if (jumpInput && Grounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpImpulse, ForceMode.VelocityChange);
        }
    }

    /// <summary>
    /// Applys a fake gravity force if the player is found to be not grounded
    /// </summary>
    private void ApplyGravity()
    {
        if (!Grounded || groundHit.distance > probeVerticalOffset)
        {
            rb.AddForce(Physics.gravity * (rb.velocity.y > 0 ? ascendingGravMod : descendingGravMod));
        }
    }

    private void RotateTowardsInput()
    {
        if (xzInput == Vector2.zero)
        {
            return;
        }

        transform.forward = Vector3.Slerp(transform.forward, new Vector3(xzInput.x, 0, xzInput.y), rotationStrength);
    }

    /// <summary>
    /// Resets the vertical component of the velocity if its on the ground. Also sets the transform to
    /// line up with the top of the ground collider.
    /// </summary>
    private void ResetVerticalVelocityOnGrounded()
    {
        if (Grounded)
        {
            Rigidbody groundBody = Ground.GetComponent<Rigidbody>();
            float relativeYVelocity = groundBody != null ? rb.velocity.y - groundBody.velocity.y : rb.velocity.y;

            if (relativeYVelocity < 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, groundBody != null ? groundBody.velocity.y : 0, rb.velocity.z);

                transform.position += Vector3.up * (probeVerticalOffset - groundHit.distance);
            }
        }
    }

    #endregion
}
