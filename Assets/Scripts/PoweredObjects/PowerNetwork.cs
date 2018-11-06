using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerNetwork : PoweredObject {

    [SerializeField]
    private List<PoweredObject> network;
    [SerializeField]
    private float activationDelay;

    /// <summary>
    /// Activates all objects on its network
    /// </summary>
    public override void Activate()
    {
        powered = true;
        StartCoroutine(ActivateNetwork());
    }

    /// <summary>
    /// Deactivates all objects along it's network
    /// </summary>
    public override void Deactivate()
    {
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
