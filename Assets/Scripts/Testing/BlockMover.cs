using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMover : MonoBehaviour
{
    [SerializeField] private BlockMover otherBlockMover;

    private GameObject heldBlock;
    private GameObject unheldBlock;

    /// <summary>
    /// Runs when colliding with another object with a trigger on it
    /// </summary>
    /// <param name="collider">The collider of the other object</param>
    private void OnTriggerEnter(Collider collider)
    {
        //if this is already holding the object it collides with, do not set unheldObject to it
        if(heldBlock != null && collider.gameObject == heldBlock.gameObject) { return; }

        //if the collided with object is a moveable block set unheldObject to be that object
        if(collider.gameObject.tag == "MoveableBlock")
        {
            unheldBlock = collider.gameObject;
        }
    }

    /// <summary>
    /// Runs when leaving the trigger of another object
    /// </summary>
    /// <param name="other">The object whose trigger was left</param>
    private void OnTriggerExit(Collider other)
    {
        //this can no longer hold the object because it is not in range
        if(unheldBlock == other.gameObject)
        {
            unheldBlock = null;
        }
    }

    /// <summary>
    /// Update runs once per frame
    /// </summary>
    private void Update()
    {
        //begin holding the unheldObject when pressing E
        if(Input.GetKeyDown(KeyCode.E) && unheldBlock != null)
        {
            //attach the unheld object to this
            unheldBlock.transform.SetParent(transform);
            //if the other player is in range of the object and also has it set as unheldObject, pretend that the other player is now holding the object
            if (otherBlockMover.unheldBlock == unheldBlock)
            {
                otherBlockMover.heldBlock = unheldBlock;
                otherBlockMover.unheldBlock = null;
            }
            heldBlock = unheldBlock;
            unheldBlock = null;
        }
        //stop holding the heldObject when pressing E (ensure that this is ACTUALLY holding the "heldObjec")
        else if(Input.GetKeyDown(KeyCode.E) && heldBlock != null && heldBlock.transform.parent == transform)
        {
            heldBlock.transform.SetParent(null);
            unheldBlock = heldBlock;
            heldBlock = null;
        }
        //this is a special case
        //if E is pressed and this object thinks it is holding something that it actually isn't we have to make sure that the other player isn't holding the same object
        //we only want to stop "holding" the object if this is the only player who isn't holding it
        else if(Input.GetKeyDown(KeyCode.E) && heldBlock != null && heldBlock.transform.parent != transform && !(otherBlockMover.heldBlock != null && otherBlockMover.heldBlock == heldBlock))
        {
            unheldBlock = heldBlock;
            heldBlock = null;
        }
    }
}
