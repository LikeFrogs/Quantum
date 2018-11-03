using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoweredElevator : PoweredObject {

    [SerializeField]
    private float minHeight, maxHeight, moveSpeed;
    private float velocity;

	// Use this for initialization
	void Start () {
		
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

        transform.position += new Vector3(0, 1, 0) * velocity * Time.deltaTime;
    }

    public override void Activate()
    {
        powered = true;
        velocity = moveSpeed;
    }

    public override void Deactivate()
    {
        powered = false;
        velocity = moveSpeed * -1;
    }
}
