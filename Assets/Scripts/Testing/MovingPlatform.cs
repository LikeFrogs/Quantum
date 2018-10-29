using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Vector3 direction;
    [SerializeField] private float dist;
    [SerializeField] private float cycleTime;

    private Rigidbody rb;

    private float timer;
    private Vector3 startPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        direction.Normalize();

        timer = 0f;
        startPosition = transform.position;
    }

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;

        rb.MovePosition(startPosition + direction * Mathf.PingPong(timer * 2 * dist / cycleTime, dist));
    }
}
