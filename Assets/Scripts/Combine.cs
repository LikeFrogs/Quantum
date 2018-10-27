using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Characters { World1, World2, Both, SuperTestPosition }

/// <summary>
/// Handles the combining and uncombining of the player characters
/// based on user input
/// </summary>
public class Combine : MonoBehaviour
{
    private Characters activeCharacters;

    [SerializeField] GameObject world1Character;
    [SerializeField] GameObject world2Character;

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
        if (Input.GetKeyDown(KeyCode.Alpha3) && activeCharacters != Characters.Both)
        {
            EnterSuperPosition();
        }

        //combine ("measure out") into world1Character
        if (Input.GetKeyDown(KeyCode.Alpha2) && activeCharacters == Characters.Both
            && world1Character.GetComponent<Rigidbody>().velocity.y == 0
            && world2Character.GetComponent<Rigidbody>().velocity.y == 0)
        {
            KillCharacter2();
        }
        //combine ("measure out") into world2Character
        else if(Input.GetKeyDown(KeyCode.Alpha1) && activeCharacters == Characters.Both 
            && world1Character.GetComponent<Rigidbody>().velocity.y == 0 
            && world2Character.GetComponent<Rigidbody>().velocity.y == 0)
        {
            KillCharacter1();
        }
    }


    private void KillCharacter1()
    {
        if(activeCharacters == Characters.World1)
        {
            LoadCheckpoint();
        }

        activeCharacters = Characters.World2;

        world1Character.SetActive(false);
    }

    private void KillCharacter2()
    {
        if(activeCharacters == Characters.World2)
        {
            LoadCheckpoint();
        }

        activeCharacters = Characters.World1;

        world2Character.SetActive(false);
    }

    private void EnterSuperPosition()
    {
        activeCharacters = Characters.Both;

        if (world1Character.activeInHierarchy)
        {
            world2Character.SetActive(true);
            world2Character.transform.position = world1Character.transform.position;
            world2Character.transform.rotation = world1Character.transform.rotation;
        }
        else
        {
            world1Character.SetActive(true);
            world1Character.transform.position = world2Character.transform.position;
            world1Character.transform.rotation = world2Character.transform.rotation;
        }
    }

    private void LoadCheckpoint()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
