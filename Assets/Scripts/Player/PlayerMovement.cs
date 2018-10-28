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
    // The time it takes to go from 0 to max speed
    [SerializeField] private float accelerationTime;
    // The time it takes to go from max speed back to 0
    [SerializeField] private float deccelerationTime;
    // The max speed the player can move at
    [SerializeField] private float maxSpeed;
    // The height the player can jump
    [SerializeField] private float jumpHeight;

    // Component references
    private Rigidbody rb;

    private float walkImpulse;
    private float frictionImpulse;

    // Bools for taking in input to use when calculating player movement
    private Vector2 directionInput;
    private bool jump;
    

    // Properties
    public float AccelerationTime { get { return accelerationTime; } }
    public float MaxSpeed { get { return maxSpeed; } }
    public float JumpHeight { get { return jumpHeight; } }

    // Methods
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        walkImpulse = maxSpeed / accelerationTime;
        frictionImpulse = maxSpeed / deccelerationTime;
    }

    private void Update()
    {
        GetInputs();
    }

    private void FixedUpdate()
    {
        // Target speed based on player input
        Vector2 targetSpeed = directionInput * maxSpeed;

        /*
        // Friction Physics
        if ((targetSpeed.x == 0 || Mathf.Sign(targetSpeed.x) != Mathf.Sign(rb.velocity.x)) && Mathf.Abs(rb.velocity.x) > 0)
        {
            float velBefore = rb.velocity.x;
            rb.AddForce(Vector3.left * Mathf.Sign(rb.velocity.x) * frictionImpulse * Time.fixedDeltaTime, ForceMode.VelocityChange);
            if (Mathf.Sign(velBefore) != Mathf.Sign(rb.velocity.x))
            {
                rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
            }
        }
        if ((targetSpeed.y == 0 || Mathf.Sign(targetSpeed.y) != Mathf.Sign(rb.velocity.z)) && Mathf.Abs(rb.velocity.z) > 0)
        {
            float velBefore = rb.velocity.z;
            rb.AddForce(Vector3.back * Mathf.Sign(rb.velocity.z) * frictionImpulse * Time.fixedDeltaTime, ForceMode.VelocityChange);
            if (Mathf.Sign(velBefore) != Mathf.Sign(rb.velocity.z))
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
            }
        }
        */

        // Movement Physics
        if (directionInput.x > 0 && rb.velocity.x < targetSpeed.x)
        {
            rb.AddForce(Vector3.right * walkImpulse * Time.fixedDeltaTime, ForceMode.VelocityChange);
            if (rb.velocity.x > targetSpeed.x)
            {
                rb.velocity.Set(targetSpeed.x, rb.velocity.y, rb.velocity.z);
            }
        }
        else if (directionInput.x < 0 && rb.velocity.x > targetSpeed.x)
        {
            rb.AddForce(Vector3.left * walkImpulse * Time.fixedDeltaTime, ForceMode.VelocityChange);
            if (rb.velocity.x < targetSpeed.x)
            {
                rb.velocity.Set(targetSpeed.x, rb.velocity.y, rb.velocity.z);
            }
        }
        if (directionInput.y > 0 && rb.velocity.z < targetSpeed.y)
        {
            rb.AddForce(Vector3.forward * walkImpulse * Time.fixedDeltaTime, ForceMode.VelocityChange);
            if (rb.velocity.z > targetSpeed.y)
            {
                rb.velocity.Set(rb.velocity.x, rb.velocity.y, targetSpeed.y);
            }
        }
        else if (directionInput.y < 0 && rb.velocity.z > targetSpeed.y)
        {
            rb.AddForce(Vector3.back * walkImpulse * Time.fixedDeltaTime, ForceMode.VelocityChange);
            if (rb.velocity.z < targetSpeed.y)
            {
                rb.velocity.Set(rb.velocity.x, rb.velocity.y, targetSpeed.y);
            }
        }

        // Friction Physics
        if (targetSpeed.x >= 0 && rb.velocity.x < 0)
        {
            rb.AddForce(Vector3.right * frictionImpulse * Time.fixedDeltaTime, ForceMode.VelocityChange);
            if (rb.velocity.x > 0)
            {
                rb.velocity.Set(0, rb.velocity.y, rb.velocity.z);
            }
        }
        else if (targetSpeed.x <= 0 && rb.velocity.x > 0)
        {
            rb.AddForce(Vector3.left * frictionImpulse * Time.fixedDeltaTime, ForceMode.VelocityChange);
            if (rb.velocity.x < 0)
            {
                rb.velocity.Set(0, rb.velocity.y, rb.velocity.z);
            }
        }
        if (targetSpeed.y >= 0 && rb.velocity.z < 0)
        {
            rb.AddForce(Vector3.forward * frictionImpulse * Time.fixedDeltaTime, ForceMode.VelocityChange);
            if (rb.velocity.z > 0)
            {
                rb.velocity.Set(rb.velocity.x, rb.velocity.y, 0);
            }
        }
        else if (targetSpeed.y <= 0 && rb.velocity.z > 0)
        {
            rb.AddForce(Vector3.back * frictionImpulse * Time.fixedDeltaTime, ForceMode.VelocityChange);
            if (rb.velocity.z < 0)
            {
                rb.velocity.Set(rb.velocity.x, rb.velocity.y, 0);
            }
        }
    }

    private void GetInputs()
    {
        directionInput.x = Input.GetAxis("Horizontal");
        directionInput.y = Input.GetAxis("Vertical");
        directionInput = Vector2.ClampMagnitude(directionInput, 1.0f);
        jump = Input.GetAxis("Jump") > 0;
    }
}
