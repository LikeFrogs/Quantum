using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoweredObject : MonoBehaviour{

    protected bool powered;
    
    public bool Powered
    {
        get { return powered; }
    }

    /// <summary>
    /// Does whatever the object should do when it recieves power.
    /// </summary>
    public abstract void Activate();

    /// <summary>
    /// Removes power from the object.
    /// </summary>
    public abstract void Deactivate();
}
