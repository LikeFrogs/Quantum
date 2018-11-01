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

    public void OnCollisionEnter(Collision collision)
    {
        carriedObjects.Add(collision.rigidbody);

        carriedWeight += collision.rigidbody.mass;

        if(!active && carriedWeight >= activationWeight)
        {
            Activate();
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        carriedObjects.Remove(collision.rigidbody);

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
        active = true;
        StartCoroutine(ActivateNetwork());
    }

    /// <summary>
    /// Deactivates all objects along it's network
    /// </summary>
    public void Deactivate()
    {
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
