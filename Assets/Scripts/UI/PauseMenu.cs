using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    /// <summary>
    /// controls the pause menu
    /// </summary>

    protected bool paused = false;

    [SerializeField] private RectTransform topSection;
    [SerializeField] private RectTransform bottomSection;

	// Use this for initialization
	void Start () {
        topSection.position += new Vector3(-250f, 0);
        bottomSection.position += new Vector3(0, -Screen.height);
	}
	
	// Update is called once per frame
	void Update () {

        //triggers for toggling pause
        bool pauseInput = Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape);

        if (pauseInput)
        {
            paused = !paused;
            if (paused)
            {
                topSection.localPosition = Vector2.zero;
            }
        }
	}
}
