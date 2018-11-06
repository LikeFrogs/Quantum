using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : PoweredObject {

    [SerializeField]
    private PowerNetwork network;
    private List<Rigidbody> carriedObjects;
    [SerializeField]
    private float activationWeight;
    private float carriedWeight;

    private void Start()
    {
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
        network.Activate();
    }

    /// <summary>
    /// Deactivates all objects along it's network
    /// </summary>
    public override void Deactivate()
    {
        powered = false;
        network.Deactivate();
    }
}
