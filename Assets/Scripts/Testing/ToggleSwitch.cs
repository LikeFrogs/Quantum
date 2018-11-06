using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSwitch : PoweredObject {

    [SerializeField]
    private List<PoweredObject> network;
    [SerializeField]
    private float activationDelay;
    private List<PlayerMovement> playersInRange;

    private void Start()
    {
        playersInRange = new List<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(playersInRange.Count > 0)
            {
                if (!powered)
                {
                    Activate();
                }
                else
                {
                    Deactivate();
                }
            }
        }
    }

    /// <summary>
    /// If a player enters the trigger range add them to the list
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            playersInRange.Add(other.GetComponent<PlayerMovement>());
        }
    }

    /// <summary>
    /// When a player leaves the trigger range remove them from the list
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            playersInRange.Remove(other.GetComponent<PlayerMovement>());
        }
    }

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
