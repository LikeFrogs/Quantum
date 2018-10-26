using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Characters { World1, World2, Both }

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
        if(Input.GetKeyDown(KeyCode.Alpha1) && activeCharacters == Characters.Both)
        {
            activeCharacters = Characters.World1;

            world2Character.transform.position = world1Character.transform.position;
            world2Character.SetActive(false);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2) && activeCharacters == Characters.Both)
        {
            activeCharacters = Characters.World2;

            world1Character.transform.position = world2Character.transform.position;
            world1Character.SetActive(false);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3) && activeCharacters != Characters.Both)
        {
            activeCharacters = Characters.Both;

            if(world1Character.activeInHierarchy)
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
    }
}
