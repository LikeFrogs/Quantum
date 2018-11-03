using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : PoweredObject {

    [SerializeField]
    private List<PoweredObject> network;
    private List<Rigidbody> carriedObjects;
    [SerializeField]
    private float activationDelay, activationWeight;
    private float carriedWeight;

    private void Start()
    {
        carriedObjects = new List<Rigidbody>();

        if(network == null)
        {
            network = new List<PoweredObject>();
        }
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
        Debug.Log("Pressure Plate Powered!");
        powered = true;
        StartCoroutine(ActivateNetwork());
    }

    /// <summary>
    /// Deactivates all objects along it's network
    /// </summary>
    public override void Deactivate()
    {
        Debug.Log("Pressure Plate Deactivated!");
        powered = false;
        StartCoroutine(DeactivateNetwork());
    }

    /// <summary>
    /// Activates objects in the network in order with a delay
    /// </summary>
    private IEnumerator ActivateNetwork()
    {
        for (int i = 0; i < network.Count; i++)
        {
            if (powered)
            {
                if (!network[i].Powered)
                {
                    network[i].Activate();
                    yield return new WaitForSeconds(activationDelay);
                }
            }
        }
    }

    /// <summary>
    /// Deactivates objects in the network in reverse order with a delay
    /// </summary>
    private IEnumerator DeactivateNetwork()
    {
        for (int i = network.Count - 1; i >= 0; i--)
        {
            if (!powered)
            {
                if (network[i].Powered)
                {
                    network[i].Deactivate();
                    yield return new WaitForSeconds(activationDelay);
                }
            }
        }
    }
}
