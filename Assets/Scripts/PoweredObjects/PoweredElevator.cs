using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PoweredElevator : PoweredObject {

    [SerializeField]
    private float minHeight, maxHeight, moveForce;
    private float velocity;
    private Rigidbody rigidbody;

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.position.y > maxHeight)
        {
            transform.position = new Vector3(transform.position.x, maxHeight, transform.position.z);
            velocity = 0;
        }

        if (transform.position.y < minHeight)
        {
            transform.position = new Vector3(transform.position.x, minHeight, transform.position.z);
            velocity = 0;
        }

        rigidbody.MovePosition(transform.position + new Vector3(0, 1, 0) * velocity * Time.deltaTime);
    }

    public override void Activate()
    {
        powered = true;
        velocity = moveForce;
    }

    public override void Deactivate()
    {
        powered = false;
        velocity = moveForce * -1;
    }
}
