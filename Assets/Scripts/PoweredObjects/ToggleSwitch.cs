using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSwitch : PoweredObject {

    [SerializeField]
    private PowerNetwork network;
    private int playersInRange;

    private void Start()
    {
        playersInRange = 0;
    }

    private void Update()
    {
        powered = network.Powered;

        Debug.Log("Powered: " + powered + ", Players in range: " + playersInRange);

        if (Input.GetKeyDown(KeyCode.E))
        {
            if(playersInRange > 0)
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
            playersInRange++;
        }
    }

    /// <summary>
    /// When a player leaves the trigger range remove them from the list
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            playersInRange--;
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
