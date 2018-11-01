using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Characters { World1, World2, Both, SuperTestPosition }

/// <summary>
/// Handles the combining and uncombining of the player characters based on user input.
/// As well as handling death and respawning
/// </summary>
public class Combine : MonoBehaviour
{
    private Characters activeCharacters;

    [SerializeField] GameObject world1Character;
    [SerializeField] GameObject world2Character;

    [SerializeField] List<BoxCollider> checkpoints;

    private void Start()
    {
        activeCharacters = Characters.Both;
    }

    // Update is called once per frame
    void Update()
    {
        //kill characters if they are at a certain height
        if(world1Character.transform.position.y <= -10)
        {
            KillCharacter1();
        }
        if(world2Character.transform.position.y <= -10)
        {
            KillCharacter2();
        }


        //reactive the inactive character at the position of the character that is currently active
        if (Input.GetKeyDown(KeyCode.Alpha3) && IsSingleCharacterInCheckpoint())
        {
            EnterSuperPosition();
        }

        //uncomment to manually control kill a character
        ////combine ("measure out") into world1Character
        //if (Input.GetKeyDown(KeyCode.Alpha2) && activeCharacters == Characters.Both
        //    && world1Character.GetComponent<Rigidbody>().velocity.y == 0
        //    && world2Character.GetComponent<Rigidbody>().velocity.y == 0)
        //{
        //    KillCharacter2();
        //}
        //combine ("measure out") into world2Character
        //else if(Input.GetKeyDown(KeyCode.Alpha1) && activeCharacters == Characters.Both 
        //    && world1Character.GetComponent<Rigidbody>().velocity.y == 0 
        //    && world2Character.GetComponent<Rigidbody>().velocity.y == 0)
        //{
        //    KillCharacter1();
        //}
    }

    /// <summary>
    /// Kills the world 1 character
    /// </summary>
    private void KillCharacter1()
    {
        //checks to see if world 1 is the only surviving character
        //and reloads the scene if so
        if(activeCharacters == Characters.World1)
        {
            LoadCheckpoint();
        }

        //sets the active character to be only world 2
        activeCharacters = Characters.World2;

        //turn off world 1 character 
        world1Character.SetActive(false);
    }

    /// <summary>
    /// kills the world 2 character
    /// </summary>
    private void KillCharacter2()
    {
        //checks to see if world 2 is the oly surviving character
        //and reloads the scene if so
        if(activeCharacters == Characters.World2)
        {
            LoadCheckpoint();
        }

        //sets the active character to be only world 1
        activeCharacters = Characters.World1;

        //turn off world 2 character
        world2Character.SetActive(false);
    }

    /// <summary>
    /// Enter superposition (turn on the inactive character at the position of the active character)
    /// </summary>
    private void EnterSuperPosition()
    {
        //update the character tracking
        activeCharacters = Characters.Both;

        //enable character 2 if character 1 is active
        if (world1Character.activeInHierarchy)
        {
            world2Character.SetActive(true);
            world2Character.GetComponent<Rigidbody>().velocity = world1Character.GetComponent<Rigidbody>().velocity;
            world2Character.transform.position = world1Character.transform.position;
            world2Character.transform.rotation = world1Character.transform.rotation;
        }
        //enable character 1 if character 2 is active
        else
        {
            world1Character.SetActive(true);
            world1Character.GetComponent<Rigidbody>().velocity = world2Character.GetComponent<Rigidbody>().velocity;
            world1Character.transform.position = world2Character.transform.position;
            world1Character.transform.rotation = world2Character.transform.rotation;    
        }
    }

    //currently this just reloads the level
    /// <summary>
    /// Reload the game from the most recent checkpoint
    /// </summary>
    private void LoadCheckpoint()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Checks to see if both only a single character is active and if that character is within one of the checkpoints/spawn zones
    /// </summary>
    /// <returns>True if only a single character is active and within a checkpoint</returns>
    private bool IsSingleCharacterInCheckpoint()
    {
        //if both characters are active this is false
        if(activeCharacters == Characters.Both)
        {
            return false;
        }

        //loop through the checkpoints and check to see if the active character is within one of them
        CapsuleCollider collider = ((activeCharacters == Characters.World1) ? world1Character : world2Character).GetComponentInChildren<CapsuleCollider>();
        for (int i = 0; i < checkpoints.Count; i++)
        {
            if(collider.bounds.Intersects(checkpoints[i].bounds))
            {
                return true;
            }
        }

        return false;
    }
}
