using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour, IPoweredObject {

    [SerializeField]
    private List<IPoweredObject> network;
    private List<Rigidbody> carriedObjects;
    [SerializeField]
    private float activationDelay, activationWeight;
    private float carriedWeight;
    private bool active = false;

    private void Start()
    {
        carriedObjects = new List<Rigidbody>();

        if(network == null)
        {
            network = new List<IPoweredObject>();
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        carriedObjects.Add(collision.collider.attachedRigidbody);

        carriedWeight += collision.rigidbody.mass;

        if(!active && carriedWeight >= activationWeight)
        {
            Activate();
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        carriedObjects.Remove(collision.collider.attachedRigidbody);

        carriedWeight -= collision.rigidbody.mass;

        if(active && carriedWeight < activationWeight)
        {
            Deactivate();
        }
    }

    /// <summary>
    /// Activates all objects on its network
    /// </summary>
    public void Activate()
    {
        Debug.Log("Pressure Plate Powered!");
        active = true;
        StartCoroutine(ActivateNetwork());
    }

    /// <summary>
    /// Deactivates all objects along it's network
    /// </summary>
    public void Deactivate()
    {
        Debug.Log("Pressure Plate Deactivated!");
        active = false;
        StartCoroutine(DeactivateNetwork());
    }

    /// <summary>
    /// Activates objects in the network in order with a delay
    /// </summary>
    private IEnumerator ActivateNetwork()
    {
        for (int i = 0; i < network.Count; i++)
        {
            network[i].Activate();
            yield return new WaitForSeconds(activationDelay);
        }
    }

    /// <summary>
    /// Deactivates objects in the network in reverse order with a delay
    /// </summary>
    private IEnumerator DeactivateNetwork()
    {
        for (int i = network.Count - 1; i >= 0; i--)
        {
            network[i].Deactivate();
            yield return new WaitForSeconds(activationDelay);
        }
    }
}
