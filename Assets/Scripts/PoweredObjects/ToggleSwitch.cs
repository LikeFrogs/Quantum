using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSwitch : PoweredObject {

    [SerializeField]
    private PowerNetwork network;
    private List<PlayerMovement> playersInRange;

    private void Start()
    {
        playersInRange = new List<PlayerMovement>();
    }

    private void Update()
    {
        powered = network.Powered;

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
