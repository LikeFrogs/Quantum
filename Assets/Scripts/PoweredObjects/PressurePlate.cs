using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PressurePlate : PoweredObject {

    [SerializeField]
    private PowerNetwork network;
    private List<Rigidbody> carriedObjects;
    [SerializeField]
    private float activationWeight;
    private float carriedWeight;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

        carriedObjects = new List<Rigidbody>();
    }

    public void OnTriggerEnter(Collider collider)
    {
        // Sometimes both of the colliders on the player object hit the plate the third condition
        // prevents reading the same object twice effectively doubling its weight.
        if(!carriedObjects.Contains(collider.attachedRigidbody))
        {
            carriedObjects.Add(collider.attachedRigidbody);

            carriedWeight += collider.attachedRigidbody.mass;

            if (!powered && carriedWeight >= activationWeight)
            {
                Activate();
            }
        } 
    }

    public void OnTriggerExit(Collider collider)
    {
        // Sometimes both of the colliders on the player object hit the plate the third condition
        // prevents reading the same object twice effectively doubling its weight.
        if (carriedObjects.Contains(collider.attachedRigidbody))
        {
            carriedObjects.Remove(collider.attachedRigidbody);

            carriedWeight -= collider.attachedRigidbody.mass;

            if (powered && carriedWeight < activationWeight)
            {
                Deactivate();
            }
        }
    }

    /// <summary>
    /// Activates all objects on its network
    /// </summary>
    public override void Activate()
    {
        powered = true;
        animator.SetBool("Powered", powered);
        network.Activate();
    }

    /// <summary>
    /// Deactivates all objects along it's network
    /// </summary>
    public override void Deactivate()
    {
        powered = false;
        animator.SetBool("Powered", powered);
        network.Deactivate();
    }
}
