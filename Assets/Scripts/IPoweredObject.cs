using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoweredObject{

    /// <summary>
    /// Does whatever the object should do when it recieves power.
    /// </summary>
    void Activate();

    /// <summary>
    /// Removes power from the object.
    /// </summary>
    void Deactivate();
}
