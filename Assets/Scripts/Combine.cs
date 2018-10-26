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
        //reactive the inactive character at the position of the character that is
        //currently active
        if (Input.GetKeyDown(KeyCode.Alpha3) && activeCharacters != Characters.Both)
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

            world1Character.layer = 9;
            world2Character.layer = 10;
        }

        //combine ("measure out") into world1Character
        if (Input.GetKeyDown(KeyCode.Alpha1) && activeCharacters == Characters.Both)
        {
            activeCharacters = Characters.World1;

            world2Character.SetActive(false);

            world1Character.layer = 9;
        }
        //combine ("measure out") into world2Character
        else if(Input.GetKeyDown(KeyCode.Alpha2) && activeCharacters == Characters.Both)
        {
            activeCharacters = Characters.World2;
            
            world1Character.SetActive(false);

            world2Character.layer = 10;
        }
        
        //when only one character is active, pressing 4 causes that character to collide with all objects
        if(Input.GetKeyDown(KeyCode.Alpha4) &&activeCharacters != Characters.Both && activeCharacters != Characters.SuperTestPosition)
        {
            if (world1Character.activeInHierarchy)
            {
                world1Character.layer = 11;
            }
            else
            {
                world2Character.layer = 11;
            }
        }
    }
}
